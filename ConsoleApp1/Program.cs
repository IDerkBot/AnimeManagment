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
        Console.Clear();
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
        Console.Clear();
    }
    else if (key == "2")
    {
        bool exit = false;
        var page = 1;
        bool canNext = false;
        bool canBack = false;
        while (!exit)
        {
            
            Console.Clear();
            foreach (var serial in serials.OrderByDescending(x => x.WatchPercent))
            {
                serial.WatchPercent = calc.GetPercent(serial.ActualSeries, serial.MaxSeries);
            }

            saver.Save(filePath, serials);

            Console.WriteLine();

            var serialsOrdered = serials.Where(x => x.WatchPercent < 100.0).OrderByDescending(x => x.WatchPercent).ToList();

            var maxOnPage = 10;
            var max = page * maxOnPage;
            var see = maxOnPage;
            if (serialsOrdered.Count < max) see = Math.Abs(max - serialsOrdered.Count);
            var maxPage = serialsOrdered.Count / maxOnPage + (serialsOrdered.Count % maxOnPage > 0 ? 1 : 0);
            if (page < maxPage) canNext = true;
            if (page == maxPage) canNext = false;
            if (page == 1) canBack = false;
            if (page > 1) canBack = true;
            
            Console.WriteLine($"{page}/{maxPage}");
            Console.WriteLine($"{"№",-4} {"Title",-75}| {"Series",-7} | {"Percent",-7} | HH:mm");

            
            for (int i = 0; i < see; i++)
            {
                var index = i + see * (page - 1);
                var time = (serialsOrdered[index].MaxSeries - serialsOrdered[index].ActualSeries) * 22;
                Console.WriteLine(
                    $"{$"{i + 1}.",-4} {serialsOrdered[index].Title,-75}| {serialsOrdered[index].ActualSeries,3}/{serialsOrdered[index].MaxSeries,3} | {$"{Math.Round(serialsOrdered[index].WatchPercent, 2)}%",7} | {time / 60:00}:{time % 60:00}");
            }

            // foreach (var serial in serials.OrderByDescending(x => x.WatchPercent))
            // {
            //     Console.WriteLine($"{serial.Title,-75}| {serial.ActualSeries,-3}/{serial.MaxSeries,-3} | {Math.Round(serial.WatchPercent, 2)}%");
            // }

            Console.WriteLine("\"q\" for exit");
            var value = Console.ReadLine();
            if (value == "q")
            {
                exit = true;
            }
            else if (value == "n" && canNext)
            {
                page++;
                
            }
            else if (value == "b" && canBack)
            {
                page--;
            }
            else
            {
                if (int.TryParse(value, out var number))
                {
                    var editIndex = serials.FindIndex(x => x.Title == serialsOrdered[number-1].Title);
                    if (serials[editIndex].ActualSeries + 1 > serials[editIndex].MaxSeries)
                    {
                    }
                    else
                    {
                        serials[editIndex].ActualSeries+=1;
                        saver.Save(filePath, serials);
                    }
                }
            }
        }
    }
}