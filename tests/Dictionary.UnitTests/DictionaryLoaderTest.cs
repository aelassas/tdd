namespace Dictionary.UnitTests;

public class DictionaryLoaderTest
{
    [Fact]
    public void TestEmptyFile()
    {
        var dictionaryLoader = new DictionaryLoader(@"..\..\..\..\..\data\dict-empty.txt");
        Assert.Equal(Array.Empty<string>(), dictionaryLoader.GetLines());
    }

    [Fact]
    public void TestDictionaryName()
    {
        var dictionaryLoader = new DictionaryLoader(@"..\..\..\..\..\data\dict-name.txt");
        Assert.Equal(new[] { "en-fr" }, dictionaryLoader.GetLines());
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var dictionaryLoader = new DictionaryLoader(@"..\..\..\..\..\data\dict.txt");
        Assert.Equal(new[] { "en-fr", "against = contre", "against = versus" }, dictionaryLoader.GetLines());
    }

    [Fact]
    public void TestErroneousFile()
    {
        var dictionaryLoader = new DictionaryLoader(@"..\..\..\..\..\data\dict-erroneous.txt");
        Assert.Equal(new[] { "en-fr", "against = ", "against = " }, dictionaryLoader.GetLines());
    }
}