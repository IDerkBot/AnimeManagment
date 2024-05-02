// See https://aka.ms/new-console-template for more information

using ConsoleApp1;
using ConsoleApp1.Saver;

CalcPercent calc = new CalcPercent();
var serials = new List<Serial>();
var saver = new DataSaver();

var filePath = @"C:\Users\ss_six\RiderProjects\NetCore\ConsoleApp1\ConsoleApp1\bin\Debug\net6.0\serials.txt";
var data = saver.Load<List<Serial>>(filePath);
if (data != null)
    serials = data;
Console.WriteLine("Calc Percent!");

while (true)
{
    Console.WriteLine("1. Add serial");
    Console.WriteLine("2. Get List");

    var key = Console.ReadLine();

    if (key == "1")
    {
        var serial = new Serial();
        Console.Write("Send title: ");
        var title = Console.ReadLine();
        serial.Title = title;
        Console.Write("Send max series: ");
        var maxSeries = Console.ReadLine();
        if (int.TryParse(maxSeries, out var valueMaxSeries))
        {
            serial.MaxSeries = valueMaxSeries;
        }

        Console.Write("Send actual series: ");
        var actualSeries = Console.ReadLine();
        if (int.TryParse(actualSeries, out var valueActualSeries))
        {
            serial.ActualSeries = valueActualSeries;
        }

        serial.WatchPercent = calc.GetPercent(serial.ActualSeries, serial.MaxSeries);

        serials.Add(serial);

        saver.Save(filePath, serials);
    }
    else if (key == "2")
    {
        foreach (var serial in serials.OrderByDescending(x => x.WatchPercent))
        {
            serial.WatchPercent = calc.GetPercent(serial.ActualSeries, serial.MaxSeries);
        }
        saver.Save(filePath, serials);

        Console.WriteLine();
        
        var serialsOrdered = serials.Where(x => x.WatchPercent < 100.0).OrderByDescending(x => x.WatchPercent).ToList();
        
        for (int i = 0; i < serialsOrdered.Count; i++)
        {
            var time = (serialsOrdered[i].MaxSeries - serialsOrdered[i].ActualSeries) * 22;
            Console.WriteLine($"{$"{i+1}.",-4} {serialsOrdered[i].Title,-75}| {serialsOrdered[i].ActualSeries,-3}/{serialsOrdered[i].MaxSeries,-3} | {$"{Math.Round(serialsOrdered[i].WatchPercent, 2)}%",-6} {time/60:00}:{time%60:00}");
        }
        
        // foreach (var serial in serials.OrderByDescending(x => x.WatchPercent))
        // {
        //     Console.WriteLine($"{serial.Title,-75}| {serial.ActualSeries,-3}/{serial.MaxSeries,-3} | {Math.Round(serial.WatchPercent, 2)}%");
        // }
        
        Console.WriteLine();
    }
}