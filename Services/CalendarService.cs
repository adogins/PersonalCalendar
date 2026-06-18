using System.Text.Json;
using PersonalCalendar.Models;

namespace PersonalCalendar.Services;

public class CalendarService
{
    private readonly string _filePath = Path.Combine("Data", "events.json");
    private List<CalendarEvent> _events = new();

    public CalendarService()
    {
        Load();
    }

    public IEnumerable<CalendarEvent> GetEvents() => _events;

    public bool HasConflict(CalendarEvent newEvent)
    {
        // expand recurring events using local method
        var expanded = ExpandRecurringEvents(_events);

        foreach (var ev in expanded)
        {
            // skip comparing event to itself
            if (ev.Id == newEvent.Id)
                continue;

            bool overlap =
                newEvent.Start < ev.End &&
                newEvent.End > ev.Start;

            if (overlap)
                return true;
        }
        return false;
    }

    public void AddEvent(CalendarEvent ev)
    {
        _events.Add(ev);
        Save();
    }

    public void DeleteEvent(Guid id)
    {
        _events.RemoveAll(e => e.Id == id);
        Save();
    }

    public void UpdateEvent(CalendarEvent updated)
    {
        var index = _events.FindIndex(e => e.Id == updated.Id);
        if (index != -1)
        {
            _events[index] = updated;
            Save();
        }
    }

    private void Load()
    {
        if (!File.Exists(_filePath))
        {
            Directory.CreateDirectory("Data");
            File.WriteAllText(_filePath, "[]");
        }

        var json = File.ReadAllText(_filePath);
        _events = JsonSerializer.Deserialize<List<CalendarEvent>>(json) ?? new();
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(
            _events,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(_filePath, json);
    }

    // ---------------------------------------------------------
    // RECURRING EVENT EXPANSION MOVED INTO THIS CLASS
    // ---------------------------------------------------------
    public IEnumerable<CalendarEvent> ExpandRecurringEvents(IEnumerable<CalendarEvent> events)
    {
        foreach (var ev in events)
        {
            if (ev.Recurrence == RecurrenceType.None)
            {
                yield return ev;
                continue;
            }

            var current = ev.Start;
            var end = ev.RecurrenceEnd ?? ev.Start.AddYears(1); // default: 1 year of repeats

            while (current <= end)
            {
                yield return new CalendarEvent
                {
                    Id = ev.Id,
                    Title = ev.Title,
                    Start = current,
                    End = current.Add(ev.End - ev.Start),
                    Recurrence = ev.Recurrence,
                    RecurrenceEnd = ev.RecurrenceEnd
                };

                current = ev.Recurrence switch
                {
                    RecurrenceType.Daily => current.AddDays(1),
                    RecurrenceType.Weekly => current.AddDays(7),
                    RecurrenceType.Monthly => current.AddMonths(1),
                    RecurrenceType.Yearly => current.AddYears(1),
                    _ => current
                };
            }
        }
    }
}
