using System.Text.RegularExpressions;

namespace Dictionary;

public class DictionaryParser(IDictionaryLoader loader) : IDictionaryParser
{
    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;

    public Dictionary<string, Dictionary<string, string>> GetTranslations()
    {
        var dictionary = new Dictionary<string, Dictionary<string, string>>();

        if (_lines.Length > 1)
        {
            var regex = new Regex(@"^(?<key>\w+) \= (?<value>\w+)$");

            for (var i = 1; i < _lines.Length; i++)
            {
                var line = _lines[i];
                var match = regex.Match(line);

                if (!match.Success)
                {
                    throw new DictionaryException("The file is erroneous.");
                }

                var key = match.Groups["key"].Value;
                var value = match.Groups["value"].Value;

                if (dictionary.TryGetValue(key, out var translations))
                {
                    translations.Add(value, key);
                }
                else
                {
                    dictionary.Add(key, new Dictionary<string, string> { { value, key } });
                }
            }
        }

        return dictionary;
    }
}