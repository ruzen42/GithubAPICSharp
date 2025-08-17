using Microsoft.AspNetCore.Mvc;
using GithubAPICSharp.Models;
using Octokit; 

namespace GithubAPICSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueryController(ILogger<QueryController> logger) : ControllerBase
{
   private readonly GitHubClient _github = new(ProductHeaderValue.Parse("RuzenBot"));
   
   [HttpPost("getrepo")]
   public async Task<IActionResult> GetRepo([FromBody] QueryRepoInfoRequest request)
   {
      if (string.IsNullOrEmpty(request.Url))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Request:\n {Request}", request.Url);
         var (owner, name) = ParseGitHubUrl(request.Url);
         var repo = await _github.Repository.Get(owner, name);
         if (repo == null) return NotFound();
         
         List<string> tags = [];
         
         if (repo.IsTemplate) tags.Add("Is Template");
         if (repo.Archived) tags.Add("Is Archived");
         if (repo.Fork) tags.Add("Is Fork");
         if (repo.HasDownloads) tags.Add("Has Downloads");
         
         var output = new QueryRepoInfoResponse
         {
            RepoName = repo.Name,
            Description = repo.Description,
            License = repo.License.Name,
            DataCreated = repo.CreatedAt.ToString(),
            Stars = repo.StargazersCount,
            Username = repo.Owner.Login, 
            Issues = repo.OpenIssuesCount,
            Language = repo.Language,
            Tags = tags 
         };
         
         logger.LogInformation("Output:\n {Output}", output);
         return Ok(output);
      }
      catch (Exception e)
      {
         logger.LogError("Error: {EMessage}", e.Message);
         return BadRequest();
      }
   }

   [HttpPost("getuser")]
   public async Task<IActionResult> GetUser([FromBody] QueryRepoInfoRequest request)
   {
      if (string.IsNullOrEmpty(request.Url))
         return BadRequest("Url is empty");
      try
      {
         logger.LogInformation("Request:\n {Request}", request.Url);
         var (owner, _) = ParseGitHubUrl(request.Url);
         var user = await _github.User.Get(owner);
         if (user == null) return NotFound("not found");
         
         List<string> tags = [];
         
         if (user.SiteAdmin) tags.Add("Is Admin");
         if (user.Suspended) tags.Add("Is Suspend");

         var output = new QueryUserInfoResponse
         {
            Username = user.Name,
            Bio = user.Bio,
            Email = user.Email,
            DataCreated = user.CreatedAt.ToString(), 
            ReposCount = user.PublicRepos + user.OwnedPrivateRepos,
            Followers = user.Followers, 
            DiskUsage = user.DiskUsage ?? 0,
            Tags = tags 
         };
         
         logger.LogInformation("Output:\n {Output}", output);
         return Ok(output);
      }
      catch (Exception e)
      {
         logger.LogError("Error: {EMessage}", e.Message);
         return BadRequest();
      }
   }

   private static (string owner, string name) ParseGitHubUrl(string url)
   {
      if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)) return (null, null)!;
      var segments = uri.Segments;
      var owner = segments[1].Trim('/');
      var name = segments[2].Trim('/');
      return (owner, name);
   }
}