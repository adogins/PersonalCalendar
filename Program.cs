using PersonalCalendar.Models;
using PersonalCalendar.Services;

var service = new CalendarService();

while (true)
{
    Console.WriteLine("\n--- Personal Calendar ---");
    Console.WriteLine("1. View Events");
    Console.WriteLine("2. Add Event");
    Console.WriteLine("3. Delete Event");
    Console.WriteLine("4. Edit Event");
    Console.WriteLine("5. Exit");
    Console.Write("Choose: ");

    var choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            foreach (var ev in service.GetEvents())
            {
                Console.WriteLine($"{ev.Id} | {ev.Title} | {ev.Start:g} -> {ev.End:g}");
            }
            break;
        
        case "2":
            Console.Write("Title: ");
            var title = Console.ReadLine() ?? string.Empty;

            Console.Write("Start (yyyy-mm-dd hh:mm): ");
            var start = DateTime.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            Console.Write("End (yyyy-mm-dd hh:mm): ");
            var end = DateTime.Parse(Console.ReadLine() ?? throw new InvalidOperationException());

            service.AddEvent(new CalendarEvent
            {
                Title = title,
                Start = start,
                End = end
            });

            Console.WriteLine("Event added.");
            break;
        
        case "3":
            Console.Write("Enter event ID: ");
            var idInput = Console.ReadLine();
            if (Guid.TryParse(idInput, out var id))
            {
                service.DeleteEvent(id);
                Console.WriteLine("Event deleted.");
            }
            else
            {
                Console.WriteLine("Invalid ID.");
            }
            break;

        case "4":
            Console.Write("Enter event ID to edit: ");
            var editIdInput = Console.ReadLine();

            if (!Guid.TryParse(editIdInput, out var editId))
            {
                Console.WriteLine("Invalid ID.");
                break;
            }

            var existing = service.GetEvents().FirstOrDefault(e => e.Id == editId);
            if (existing == null)
            {
                Console.WriteLine("Event not found.");
                break;
            }

            Console.Write($"New title (leave blank to keep '{existing.Title}'): ");
            var newTitle = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newTitle))
                existing.Title = newTitle;

            Console.Write($"New start (yyyy-mm-dd hh:mm) or blank to keep {existing.Start:g}: ");
            var newStart = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newStart))
                existing.Start = DateTime.Parse(newStart);

            Console.Write($"New end (yyyy-mm-dd hh:mm) or blank to keep {existing.End:g}: ");
            var newEnd = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newEnd))
                existing.End = DateTime.Parse(newEnd);

            service.UpdateEvent(existing);
            Console.WriteLine("Event updated.");
            break;
        
        case "5":
            return;
        
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}