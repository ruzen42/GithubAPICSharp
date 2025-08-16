using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GithubAPICSharp.Models;
using Octokit; 

namespace GithubAPICSharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RequestController(ILogger<RequestController> logger) : ControllerBase
{
   private readonly GitHubClient _github = new(ProductHeaderValue.Parse("RuzenBot"));
   [HttpPost("getrepo")]
   public async Task<IActionResult> GetRepo([FromBody] QueryRepoInfoRequest request)
   {
      if (string.IsNullOrEmpty(QueryRepoInfoRequest.Url))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Received command:\n {Context}", request);
         var (owner, name) = ParseGitHubUrl(QueryRepoInfoRequest.Url);
         var repo = await _github.Repository.Get(owner, name);

         return Ok(new QueryRepoInfoResponse
         {
            RepoName = repo.Name,
            Stars = repo.StargazersCount,
            Username = owner, 
            Language = repo.Language
         });
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
      if (segments.Length < 3) return (null, null)!;
      
      var owner = segments[1].Trim('/');
      var name = segments[2].Trim('/');
      return (owner, name);
   }
}