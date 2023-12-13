namespace Translator;

public class TranslatorLoader(string path) : ITranslatorLoader
{
    public string[] GetLines() => File.ReadAllLines(path);
}