namespace Portfolio.Api.Models;

public class Project
{
    public int Id { get; set; }                 // Primary key
    public string Title { get; set; } = "";
    public string? Description { get; set; }
    public string? TechStack { get; set; }      // e.g. "React, .NET, PostgreSQL"
    public string? RepoUrl { get; set; }        // GitHub link
    public string? LiveUrl { get; set; }        // Hosted link
    public DateOnly? StartDate { get; set; }
}
