using GithubAPICSharp.Models;
using Octokit;

namespace GithubAPICSharp.Services;

public class GitHubService(GitHubClient githubClient) : IGitHubService
{
    private const string GitHubHost = "github.com";
    
    public async Task<QueryRepoInfoResponse> GetRepoInfoAsync(string user, string repo)
    {
        var repository = await githubClient.Repository.Get(user, repo);
        
        var tags = new List<string> { "Normal" };
         
        if (repository.IsTemplate) tags.Add("Is Template");
        if (repository.Archived) tags.Add("Is Archived");
        if (repository.Fork) tags.Add("Is Fork");
        if (repository.HasDownloads) tags.Add("Has Downloads");
        
        var output = new QueryRepoInfoResponse(
            Username: repository.Owner.Login,
            RepoName: repository.Name,
            Description: repository.Description ?? string.Empty,
            DataCreated: repository.CreatedAt.ToString(),
            License: repository.License != null ? $"{repository.License.Name} {repository.License.Url}" : "No License",
            Stars: repository.StargazersCount,
            Issues: repository.OpenIssuesCount,
            Language: repository.Language ?? "Unknown",
            Tags: tags
        );
        
        return output;
    }

    public async Task<QueryUserInfoResponse> GetUserInfoAsync(string username)
    {
        var user = await githubClient.User.Get(username);

        List<string> tags = ["IsNormal"];
        if (user.SiteAdmin) tags.Add("Is Admin");
        
        var output = new QueryUserInfoResponse(
            Username: user.Login, 
            Bio: user.Bio ?? string.Empty,
            Email: user.Email ?? string.Empty,
            DataCreated: user.CreatedAt.ToString(), 
            ReposCount: user.PublicRepos + user.TotalPrivateRepos,
            Followers: user.Followers, 
            Tags: tags 
        );
        
        return output;
    }
}