using GithubAPICSharp.Models;
using GithubAPICSharp.Services;
using Microsoft.AspNetCore.Mvc;
using Octokit; 

namespace GithubAPICSharp.Controllers;

[ApiController]
[Route("api")]
public class ApiController(ILogger<ApiController> logger, IGitHubService githubService) : ControllerBase
{
   [HttpGet("repo")]
   public async Task<IActionResult> GetRepo([FromQuery] QueryRepoInfoRequest request)
   {
      var user = request.User;
      var repo = request.Repo;
      if (string.IsNullOrEmpty(request.Repo) ||  string.IsNullOrEmpty(user))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Request: {Request}", request);

         var output = await githubService.GetRepoInfoAsync(user, repo); 
         logger.LogInformation("Output:\n {Output}", output);
         return Ok(output);
      }
      catch (NotFoundException)
      {
         return NotFound("GitHub repository not found.");
      }
      catch (ArgumentException e)
      {
         return BadRequest(e.Message);
      }
      catch (Exception e)
      {
         logger.LogError("Error: {EMessage}", e);
         return StatusCode(500, "An unexpected error occurred.");
      }
   }

   [HttpGet("user")]
   public async Task<IActionResult> GetUser([FromQuery] QueryUserInfoRequest request)
   {
      var url = request.User;
      if (string.IsNullOrEmpty(url))
         return BadRequest("Url is empty");
      
      try
      {
         logger.LogInformation("Request:\n {Request}", url);
         var output = await githubService.GetUserInfoAsync(url);
         logger.LogInformation("Output:\n {Output}", output);
         return Ok(output);
      }
      catch (NotFoundException)
      {
         return NotFound("GitHub user not found.");
      }
      catch (ArgumentException e)
      {
         return BadRequest(e.Message);
      }
      catch (Exception e)
      {
         logger.LogError("Error: {EMessage}", e.Message);
         return StatusCode(500, "An unexpected error occurred.");
      }
   }
}

