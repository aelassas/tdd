using Moq;

namespace Dictionary.UnitTests;

public class DictionaryDataSourceTest
{
    [Fact]
    public void TestEmptyFileTranslations()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>());

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal(Array.Empty<string>(), dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestDictionaryName()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetName())
            .Returns("en-fr");

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal("en-fr", dictionary.Name);
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>
            {
                { "against", new Dictionary<string, string> {
                      {"contre", "against"},
                      {"versus", "against"}
                    }
                }
            });

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal(new[] { "contre", "versus" }, dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestErroneousFile()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Throws(new DictionaryException("The file is erroneous."));

        Assert.Throws<DictionaryException>(() => new Dictionary(mockDictionaryParser.Object));
    }
}