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
    public void TestAddTranslation()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt")));
        dictionary.AddTranslation("against", "contre");
        Assert.Equal(new[] { "contre" }, dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestAddTranslations()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt")));
        dictionary.AddTranslation("against", "contre");
        dictionary.AddTranslation("against", "versus");
        Assert.Equal(new[] { "contre", "versus" }, dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestNoTranslation()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt")));
        Assert.Equal(Array.Empty<string>(), dictionary.GetTranslation("against"));
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

    [Fact]
    public void TestReverseTranslation()
    {
        var dictionary = new Dictionary(new DictionaryParser(new DictionaryLoader(@"..\..\..\..\..\data\dict.txt")));
        Assert.Equal("against", dictionary.GetTranslation("contre")[0]);
    }
}