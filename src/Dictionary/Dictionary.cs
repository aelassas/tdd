namespace Dictionary;

public class Dictionary
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations;
    public string Name { get; private set; }

    public Dictionary(string name)
    {
        _translations = new Dictionary<string, Dictionary<string, string>>();
        Name = name;
    }

    public Dictionary(IDictionaryParser parser)
    {
        _translations = parser.GetTranslations();
        Name = parser.GetName();
    }

    public void AddTranslation(string word, string translation)
    {
        if (_translations.TryGetValue(word, out var translations))
        {
            translations.Add(translation, word);
        }
        else
        {
            _translations.Add(word, new Dictionary<string, string> { { translation, word } });
        }
    }

    public string[] GetTranslation(string word)
    {
        if (_translations.TryGetValue(word, out var translations))
        {
            return translations.Keys.ToArray();
        }

        // Try reverse translation
        return (from t in _translations
                from v in t.Value.Values
                where t.Value.ContainsKey(word)
                select v).Distinct().ToArray();
    }

    public bool IsEmpty() => _translations.Count == 0;
}