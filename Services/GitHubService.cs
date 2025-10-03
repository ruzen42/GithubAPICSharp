using GithubAPICSharp.Models;
using Octokit;

namespace GithubAPICSharp.Services;

public class GitHubService(GitHubClient githubClient) : IGitHubService
{
    private const string GitHubHost = "github.com";
    
    private static bool TryParseGitHubUrl(string url, out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrWhiteSpace(url))
            return false;
            
        return Uri.TryCreate(url, UriKind.Absolute, out uri) && 
               uri.Host.Equals(GitHubHost, StringComparison.OrdinalIgnoreCase);
    }

    private static (string owner, string name) ParseGitHubUrlRepo(string url)
    {
        if (!TryParseGitHubUrl(url, out var uri))
            throw new ArgumentException("Invalid GitHub URL format for repository.");

        var segments = uri!.Segments;
        const int minSegmentsForRepo = 3;
        if (segments.Length < minSegmentsForRepo)
            throw new ArgumentException("GitHub URL must contain both owner and repository name.");
            
        var owner = segments[1].Trim('/');
        var name = segments[2].Trim('/');

        if (string.IsNullOrEmpty(owner) || string.IsNullOrEmpty(name))
            throw new ArgumentException("Owner and repository name cannot be empty.");

        return (owner, name);
    }
    
    private static string ParseGitHubUrlUser(string url)
    {
        if (!TryParseGitHubUrl(url, out var uri))
            throw new ArgumentException("Invalid GitHub URL format for user.");

        var segments = uri!.Segments;
        const int minSegmentsForUser = 2;
        if (segments.Length < minSegmentsForUser)
            throw new ArgumentException("GitHub URL must contain username.");
            
        var username = segments[1].Trim('/');

        return string.IsNullOrEmpty(username) ? throw new ArgumentException("Username cannot be empty.") : username;
    }

    public async Task<QueryRepoInfoResponse> GetRepoInfoAsync(string repoUrl)
    {
        var (owner, name) = ParseGitHubUrlRepo(repoUrl);
        
        var repo = await githubClient.Repository.Get(owner, name);
        
        var tags = new List<string> { "Normal" };
         
        if (repo.IsTemplate) tags.Add("Is Template");
        if (repo.Archived) tags.Add("Is Archived");
        if (repo.Fork) tags.Add("Is Fork");
        if (repo.HasDownloads) tags.Add("Has Downloads");
        
        var output = new QueryRepoInfoResponse(
            Username: repo.Owner.Login,
            RepoName: repo.Name,
            Description: repo.Description ?? string.Empty,
            DataCreated: repo.CreatedAt.ToString(),
            License: repo.License != null ? $"{repo.License.Name} {repo.License.Url}" : "No License",
            Stars: repo.StargazersCount,
            Issues: repo.OpenIssuesCount,
            Language: repo.Language ?? "Unknown",
            Tags: tags
        );
        
        return output;
    }

    public async Task<QueryUserInfoResponse> GetUserInfoAsync(string userUrl)
    {
        var username = ParseGitHubUrlUser(userUrl);

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