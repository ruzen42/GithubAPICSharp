using Octokit;

namespace GithubAPICSharp.Models;

public record QueryRepoInfoResponse
{
    public string Username { get; init; } = null!;
    public string RepoName { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string DataCreated { get; init; } = null!;
    public string License { get; init; } = null!;
    public int Stars { get; init; }
    
    public int Issues { get; init; }
    public string Language { get; init; } = null!;
    public List<string> Tags { get; init; } = null!;

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