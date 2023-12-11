namespace Dictionary;

public interface IDictionaryParser
{
    string GetName();
    Dictionary<string, Dictionary<string, string>> GetTranslations();
}