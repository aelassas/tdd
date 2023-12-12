using System.Text.RegularExpressions;

namespace Dictionary;

public class DictionaryParser(IDictionaryLoader loader) : IDictionaryParser
{
    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;

    public Dictionary<string, List<string>> GetTranslations()
    {
        var dictionary = new Dictionary<string, List<string>>();

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
                    translations.Add(value);
                }
                else
                {
                    dictionary.Add(key, [value]);
                }
            }
        }

        return dictionary;
    }
}