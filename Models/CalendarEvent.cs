namespace PersonalCalendar.Models;

public enum RecurrenceType
{
    None,
    Daily,
    Weekly,
    Monthly,
    Yearly
}

public class CalendarEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string? Description { get; set; }

    public RecurrenceType Recurrence { get; set; } = RecurrenceType.None;

    // when the recurrenct stops (optional)
    public DateTime? RecurrenceEnd {get; set; }
}


