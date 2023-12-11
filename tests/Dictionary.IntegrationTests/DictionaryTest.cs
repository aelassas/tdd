namespace Dictionary.IntegrationTests;

public class DictionaryTest
{
    [Fact]
    public void TestDictionaryName()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt")));
        Assert.Equal("en-fr", dictionary.Name);
    }

    [Fact]
    public void TestIsEmpty()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt")));
        Assert.True(dictionary.IsEmpty());
    }

    [Fact]
    public void TestIsNotEmpty()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict.txt")));
        Assert.False(dictionary.IsEmpty());
    }

    [Fact]
    public void TestOneTranslation()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict.txt")));
        Assert.Equal("contre", dictionary.GetTranslation("against")[0]);
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict.txt")));
        Assert.Equal(new[] { "contre", "versus" }, dictionary.GetTranslation("against"));
    }
}