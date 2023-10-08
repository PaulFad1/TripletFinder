using System.Text.RegularExpressions;

public class TripletFinder
{
    public string Path { get; set; }
    public static Dictionary<string, int> Triplets { get; set; }
    private static Regex regularEx;
    private static Object obj;
    public TripletFinder(string path)
    {
        Path = path;
        Triplets = new Dictionary<string, int>();
        obj = new();
        regularEx = new("[a-zа-я]");
    }
    public static void GetTripletsWord(string word)
    {
        for (int i = 0; i < word.Length; i++)
        {
            try
            {
                var triplet = word.Substring(i, 3);
                triplet = triplet.ToLower();
                MatchCollection matchCollection = regularEx.Matches(triplet);
                if (matchCollection.Count == 3)
                {
                    lock (obj)
                    {
                        if (Triplets.ContainsKey(triplet))
                        {
                            Triplets[triplet]++;
                        }
                        else
                        {
                            Triplets.Add(triplet, 1);
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {

            }

        }
    }

    public async Task<Dictionary<string, int>> GetTopTriplets(int top)
    {
        string text = "";
        try
        {
            using (StreamReader streamReader = new(Path))
            {
                text = await streamReader.ReadToEndAsync();
            }
        }
        catch(FileNotFoundException ex)
        {
            Console.WriteLine("Файл не найден");
        }

        string[] words = text.Split(' ');
        Parallel.ForEach(words, word => GetTripletsWord(word));
        return Triplets.OrderByDescending(x => x.Value).Take(top).ToDictionary(x => x.Key, x => x.Value);
    }
}