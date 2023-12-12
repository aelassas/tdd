using Moq;

namespace Dictionary.UnitTests;

public class DictionaryParserTest
{
    [Fact]
    public void TestEmptyDictionaryName()
    {
        var mockDictionaryLoader = new Mock<IDictionaryLoader>();
        mockDictionaryLoader
            .Setup(dl => dl.GetLines())
            .Returns(Array.Empty<string>());

        var dictionaryParser = new DictionaryParser(mockDictionaryLoader.Object);
        Assert.Equal(string.Empty, dictionaryParser.GetName());
    }

    [Fact]
    public void TestNoTranslations()
    {
        var mockDictionaryLoader = new Mock<IDictionaryLoader>();
        mockDictionaryLoader
            .Setup(dl => dl.GetLines())
            .Returns(Array.Empty<string>());

        var dictionaryParser = new DictionaryParser(mockDictionaryLoader.Object);
        Assert.Equal(new Dictionary<string, Dictionary<string, string>>(), dictionaryParser.GetTranslations());
    }

    [Fact]
    public void TestDictionaryName()
    {
        var mockDictionaryLoader = new Mock<IDictionaryLoader>();
        mockDictionaryLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr" });

        var dictionaryParser = new DictionaryParser(mockDictionaryLoader.Object);
        Assert.Equal("en-fr", dictionaryParser.GetName());
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var mockDictionaryLoader = new Mock<IDictionaryLoader>();
        mockDictionaryLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr", "against = contre", "against = versus" });

        var dictionaryParser = new DictionaryParser(mockDictionaryLoader.Object);
        var expected = new Dictionary<string, Dictionary<string, string>>
        {
            {"against", new Dictionary<string, string> {
                  {"contre", "against"},
                  {"versus", "against"}
                }
            }
        };
        Assert.Equal(expected, dictionaryParser.GetTranslations());
    }

    [Fact]
    public void TestErroneousFile()
    {
        var mockDictionaryLoader = new Mock<IDictionaryLoader>();
        mockDictionaryLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr", "against = ", "against = " });

        var dictionaryParser = new DictionaryParser(mockDictionaryLoader.Object);
        Assert.Throws<DictionaryException>(() => dictionaryParser.GetTranslations());
    }
}