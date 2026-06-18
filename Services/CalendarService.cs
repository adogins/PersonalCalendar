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
}