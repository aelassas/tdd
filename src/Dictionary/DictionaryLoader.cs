namespace Dictionary;

public class DictionaryLoader(string path) : IDictionaryLoader
{
    public string[] GetLines() => File.ReadAllLines(path);
}