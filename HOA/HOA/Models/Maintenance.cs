namespace HOA.Models;

public partial class Maintenance
{
    public int Id { get; set; }

    public string TaskName { get; set; }

    public string AssignedPersonnel { get; set; }

    public DateOnly DueDate { get; set; }

    public string Status { get; set; }
}