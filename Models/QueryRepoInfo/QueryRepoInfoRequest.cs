namespace GithubAPICSharp.Models.QueryRepoInfo;

public readonly record struct QueryRepoInfoRequest
{
    public required string Url {get; init;}

    public (string owner, string name) ParseGitHubUrl()
    {
        if (!Uri.TryCreate(Url, UriKind.Absolute, out var uri) || !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)) return (null, null)!;
        var segments = uri.Segments;
        var owner = segments[1].Trim('/');
        var name = segments[2].Trim('/');
        return (owner, name);
    }
}