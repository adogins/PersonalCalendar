using PersonalCalendar.Models;
using PersonalCalendar.Services;

var service = new CalendarService();

while (true)
{
    Console.WriteLine("\n--- Personal Calendar ---");
    Console.WriteLine("1. View Events");
    Console.WriteLine("2. Add Event");
    Console.WriteLine("3. Delete Event");
    Console.WriteLine("4. Exit");
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
            return;
        
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
}