using System.Text.RegularExpressions;

namespace Translator;

public class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    private static readonly Regex TranslatorRegex = new(@"^(?<key>\w+) = (?<value>\w+)$");

    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;

    public Dictionary<string, List<string>> GetTranslations()
    {
        var translator = new Dictionary<string, List<string>>();

        if (_lines.Length <= 1)
        {
            return translator;
        }

        for (var i = 1; i < _lines.Length; i++)
        {
            var line = _lines[i];
            var match = TranslatorRegex.Match(line);

            if (!match.Success)
            {
                throw new TranslatorException("The file is erroneous.");
            }

            var key = match.Groups["key"].Value;
            var value = match.Groups["value"].Value;

            if (translator.TryGetValue(key, out var translations))
            {
                translations.Add(value);
            }
            else
            {
                translator.Add(key, [value]);
            }
        }

        return translator;
    }
}
