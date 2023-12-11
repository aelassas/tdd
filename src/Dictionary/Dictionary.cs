namespace Dictionary;

public class Dictionary(IDictionaryParser parser)
{
    private readonly Dictionary<string, Dictionary<string, string>> _translations = parser.GetTranslations();
    public string Name { get; } = parser.GetName();

    public void AddTranslation(string word, string translation)
    {
        if (!_translations.ContainsKey(word))
        {
            _translations.Add(word, new Dictionary<string, string> { { translation, word } });
        }
        else
        {
            _translations[word].Add(translation, word);
        }
    }

    public string[] GetTranslation(string word)
    {
        if (_translations.TryGetValue(word, out var translation))
        {
            return translation.Keys.ToArray();
        }

        return (from t in _translations
                from v in t.Value.Values
                where t.Value.ContainsKey(word)
                select v).Distinct().ToArray();
    }

    public bool IsEmpty() => _translations.Count == 0;
}