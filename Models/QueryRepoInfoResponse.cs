using Octokit;

namespace GithubAPICSharp.Models;

public record QueryRepoInfoResponse
{
    public string Username { get; init; } = "";
    public string RepoName { get; init; } = "";
    public string Description { get; init; } = "";
    public string DataCreated { get; init; } = "";
    public string License { get; init; } = "";
    public int Stars { get; init; }
    
    public int Issues { get; init; }
    public string Language { get; init; } = "";
    public List<string> Tags { get; set; } = [];

    public override string ToString() => $"\tOwner:{Username}" +
                                         $"\n\tDescription:{Description}" +
                                         $"\n\tRepo:{RepoName}" +
                                         $"\n\tStars:{Stars}" +
                                         $"\n\tIssues:{Issues}" +
                                         $"\n\tLanguage:{Language}" +
                                         $"\n\tData Created:{DataCreated}" +
                                         $"\n\tTags:{Tags.Count}" +
                                         $"\n\tLicense:{License}";
}