namespace Translator.UnitTests;

public class TranslatorLoaderTest
{
    [Fact]
    public void TestEmptyFile()
    {
        var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-empty.txt");
        Assert.Equal(Array.Empty<string>(), translatorLoader.GetLines());
    }

    [Fact]
    public void TestTranslatorName()
    {
        var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-name.txt");
        Assert.Equal(new[] { "en-fr" }, translatorLoader.GetLines());
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator.txt");
        Assert.Equal(new[] { "en-fr", "against = contre", "against = versus" }, translatorLoader.GetLines());
    }

    [Fact]
    public void TestErroneousFile()
    {
        var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-erroneous.txt");
        Assert.Equal(new[] { "en-fr", "against = ", "against = " }, translatorLoader.GetLines());
    }
}