using System.Diagnostics.CodeAnalysis;

namespace parsr.todo.Dtos;

public class LinkCollection<TValue>
{
	public enum HttpVerb
	{
		NONE = 0,
		GET = 1, POST = 2, PUT = 3, PATCH = 4, DELETE = 5
	}

	public TValue Value { get; set; }
	public List<Link> Links { get; set; }

	public LinkCollection(TValue value, [AllowNull] IEnumerable<Link> links = null)
	{
		links ??= Array.Empty<Link>();
		Value = value;
		Links = new List<Link>(links);
	}

	public void AddLink(string href, string rel, string method)
		=> Links.Add(new Link(href, rel, method));

	public void AddLink(string href, string rel, HttpVerb method)
		=> Links.Add(new Link(href, rel, method.ToString()));
}
