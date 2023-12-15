namespace Translator;

public class Translator
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Translator(string name)
    {
        _translations = [];
        Name = name;
    }

    public Translator(ITranslatorParser parser)
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
            return [.. translations];
        }

        // Try reverse translation
        return [.. from t in _translations
                   where t.Value.Contains(word)
                   select t.Key];
    }

    public bool IsEmpty() => _translations.Count == 0;
}