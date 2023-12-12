namespace Dictionary.UnitTests;

public class DictionaryTest
{
    [Fact]
    public void TestDictionaryName()
    {
        var dictionary = new Dictionary("en-fr");
        Assert.Equal("en-fr", dictionary.Name);
    }

    [Fact]
    public void TestIsEmpty()
    {
        var dictionary = new Dictionary("en-fr");
        Assert.True(dictionary.IsEmpty());
        dictionary.AddTranslation("against", "contre");
        Assert.False(dictionary.IsEmpty());
    }

    [Fact]
    public void TestOneTranslation()
    {
        var dictionary = new Dictionary("en-fr");
        dictionary.AddTranslation("against", "contre");
        Assert.Equal("contre", dictionary.GetTranslation("against")[0]);
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var dictionary = new Dictionary("en-fr");
        dictionary.AddTranslation("against", "contre");
        dictionary.AddTranslation("against", "versus");
        Assert.Equal(new[] { "contre", "versus" }, dictionary.GetTranslation("against"));
    }
}