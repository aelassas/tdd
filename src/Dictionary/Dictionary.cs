namespace Dictionary;

public class Dictionary
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Dictionary(string name)
    {
        _translations = new Dictionary<string, List<string>>();
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
            translations.Add(translation);
        }
        else
        {
            _translations.Add(word, [translation]);
        }
    }

    public string[] GetTranslation(string word)
    {
        if (_translations.TryGetValue(word, out var translations))
        {
            return translations.ToArray();
        }

        // Try reverse translation
        return (from t in _translations
                from v in t.Value
                where t.Value.Contains(word)
                select t.Key).ToArray();
    }

    public bool IsEmpty() => _translations.Count == 0;
}