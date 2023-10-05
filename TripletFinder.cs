using Microsoft.VisualBasic;

public class TripletFinder 
{
    public string Path  {get;set;}
    public static Dictionary<string, int> Triplets {get;set;}
    private static Object obj;
    public TripletFinder(string path)
    {
        Path = path;
        Triplets = new Dictionary<string, int>();
        obj = new();
    }
    public static void GetTripletsWord(string word)
    {
        lock(obj)
        {
            char litera = '.';
            int count = 0;
            for(int i = 0; i < word.Length; i++)
            {
                if(count == 3)
                {
                    string str = "" + litera;
                    if(Triplets.ContainsKey(str))
                    {
                        Triplets[str]++;
                    }
                    else
                    {
                        Triplets.Add(str, 1);
                    }
                }
                if(word[i] != litera)
                {
                    litera = word[i];
                    count = 1;
                }
                else 
                {
                    count++;
                }

            }
        }
    }
    public static void GetTripletsSentence(string sentence)
    {
        var words = sentence.Split(' ');
        Task[] tasks = new Task[words.Length];
        for(int i = 0; i < words.Length; i++)
        {
            int j = i;
            tasks[j] = Task.Run(() => GetTripletsWord(words[j]));
        }
        Task.WaitAll(tasks);
    }
    public async Task<Dictionary<string, int>> GetTopTriplets(int top)
    {
        string text;
        using(StreamReader streamReader = new(Path))
        {
            text = await streamReader.ReadToEndAsync();
        }
        string[] sentences = text.Split('.');
        Task[] tasks = new Task[sentences.Length];
        for(int i = 0; i < sentences.Length; i++)
        {
            int j = i;
            tasks[j] = Task.Run(() => GetTripletsSentence(sentences[j]));
        }
        Task.WaitAll(tasks);
        return Triplets.OrderByDescending(x => x.Value).Take(top).ToDictionary(x => x.Key, x => x.Value);
    }
}