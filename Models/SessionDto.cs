namespace JudoClubWeb.Models;

public class SessionStudentDto
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Belt { get; set; } = string.Empty;
}

public class SessionDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public int StudentCount { get; set; }
}