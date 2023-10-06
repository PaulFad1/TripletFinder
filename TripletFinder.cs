using Microsoft.VisualBasic;

public class TripletFinder
{
    public string Path { get; set; }
    public static Dictionary<string, int> Triplets { get; set; }
    private static Object obj;
    public TripletFinder(string path)
    {
        Path = path;
        Triplets = new Dictionary<string, int>();
        obj = new();
    }
    public static void GetTripletsSentence(string sentence)
    {

        char litera = '.';
        int count = 0;
        for (int i = 0; i < sentence.Length; i++)
        {
            if (count == 3)
            {
                string str = "" + litera;
                lock (obj)
                {
                    if (Triplets.ContainsKey(str))
                    {
                        Triplets[str]++;
                    }
                    else
                    {
                        Triplets.Add(str, 1);
                    }
                }
            }
            if (sentence[i] != litera)
            {
                litera = sentence[i];
                count = 1;
            }
            else
            {
                count++;
            }
        }
    }

    public async Task<Dictionary<string, int>> GetTopTriplets(int top)
    {
        string text;
        using (StreamReader streamReader = new(Path))
        {
            text = await streamReader.ReadToEndAsync();
        }
        string[] sentences = text.Split('.');
        Parallel.ForEach(sentences, sentence => GetTripletsSentence(sentence));
        return Triplets.OrderByDescending(x => x.Value).Take(top).ToDictionary(x => x.Key, x => x.Value);
    }
}