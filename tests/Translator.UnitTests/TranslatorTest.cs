namespace Translator.UnitTests;

public class TranslatorTest
{
    [Fact]
    public void TestTranslatorName()
    {
        var translator = new Translator("en-fr");
        Assert.Equal("en-fr", translator.Name);
    }

    [Fact]
    public void TestIsEmpty()
    {
        var translator = new Translator("en-fr");
        Assert.True(translator.IsEmpty());
        translator.AddTranslation("against", "contre");
        Assert.False(translator.IsEmpty());
    }

    [Fact]
    public void TestOneTranslation()
    {
        var translator = new Translator("en-fr");
        translator.AddTranslation("against", "contre");
        Assert.Equal("contre", translator.GetTranslation("against")[0]);
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var translator = new Translator("en-fr");
        translator.AddTranslation("against", "contre");
        translator.AddTranslation("against", "versus");
        Assert.Equal(new[] { "contre", "versus" }, translator.GetTranslation("against"));
    }
}