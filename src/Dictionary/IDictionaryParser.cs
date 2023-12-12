namespace Dictionary;

public interface IDictionaryParser
{
    string GetName();
    Dictionary<string, List<string>> GetTranslations();
}