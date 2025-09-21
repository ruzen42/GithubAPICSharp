namespace GithubAPICSharp.Models.UserInfo;

public readonly record struct QueryUserInfoRequest(string Url)
{
    public string ParseGitHubUrl()
    {
        if (!Uri.TryCreate(Url, UriKind.Absolute, out var uri) ||
            !uri.Host.Equals("github.com", StringComparison.OrdinalIgnoreCase)) return null!;
        var segments = uri.Segments;
        var owner = segments[1].Trim('/');
        return owner;
    }
}