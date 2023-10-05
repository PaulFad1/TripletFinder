
using System.Diagnostics;

class Program 
{
    public static async Task Main(string[] args)
    {
        Stopwatch stopwatch = new();
        TripletFinder tripletFinder = new(args[0]);
        stopwatch.Start();
        var result = await tripletFinder.GetTopTriplets(10);
        stopwatch.Stop();
        foreach(var el in result)
        {
            Console.WriteLine(el.Key + " " + el.Value);
        }
        Console.WriteLine("Время выполнения:" + stopwatch.ElapsedMilliseconds);
    }
}