namespace Translator;

public interface ITranslatorParser
{
    string GetName();
    Dictionary<string, List<string>> GetTranslations();
}