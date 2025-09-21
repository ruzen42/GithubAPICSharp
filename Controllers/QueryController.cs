using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc;
using GithubAPICSharp.Models.QueryRepoInfo;
using GithubAPICSharp.Models.UserInfo;
using Octokit; 

namespace GithubAPICSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QueryController(ILogger<QueryController> logger) : ControllerBase
{
   private readonly GitHubClient _github = new(ProductHeaderValue.Parse("RuzenBot"));
   
   [HttpPost("get-repo")]
   public async Task<IActionResult> GetRepo([FromBody] QueryRepoInfoRequest request)
   {
      if (string.IsNullOrEmpty(request.Url))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Request: {Request}", request.Url);
         
         var (owner, name) = ParseGitHubUrlRepo(request.Url); 
         var repo = await _github.Repository.Get(owner, name)!;
         if (repo == null) return BadRequest();
         
         List<string> tags = ["NormalUser"];
         
         if (repo.IsTemplate) tags.Add("Is Template");
         if (repo.Archived) tags.Add("Is Archived");
         if (repo.Fork) tags.Add("Is Fork");
         if (repo.HasDownloads) tags.Add("Has Downloads");
         
         var output = new QueryRepoInfoResponse
         {
            RepoName = repo.Name,
            Description = repo.Description,
            License = repo.License.Name + " " + repo.License.Url,
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
         logger.LogError("Error: {EMessage}", e);
         return BadRequest();
      }
   }

   [HttpPost("get-user")]
   public async Task<IActionResult> GetUser([FromBody] QueryUserInfoRequest request)
   {
      if (string.IsNullOrEmpty(request.Url))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Request:\n {Request}", request.Url);
         var user = await _github.User.Get(ParseGitHubUrlUser(request.Url));
         if (user == null) return BadRequest("Not found");
         
         List<string> tags = ["IsNormal"];
         if (user.SiteAdmin) tags.Add("Is Admin");
         if (user.Suspended) tags.Add("Is Suspend");

         var output = new QueryUserInfoResponse
         {
            Username = user.Name,
            Bio = user.Bio,
            Email = user.Email,
            DataCreated = user.CreatedAt.ToString(), 
            ReposCount = user.PublicRepos + user.TotalPrivateRepos,
            Followers = user.Followers, 
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
   
   public string ParseGitHubUrlUser(string url)
   {
      if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) ||
          !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)) return null!;
      var segments = uri.Segments;
      var owner = segments[0].Trim('/');
      return owner;
   }
   
   public (string owner, string name) ParseGitHubUrlRepo(string url)
   {
      if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)) return (null!, null!);
      var segments = uri.Segments;
      var owner = segments[1].Trim('/');
      var name = segments[2].Trim('/');
      return (owner, name);
   }
}
