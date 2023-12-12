using Moq;

namespace Dictionary.UnitTests;

public class DictionaryDataSourceTest
{
    [Fact]
    public void TestEmptyDictionaryName()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetName())
            .Returns(string.Empty);

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal(string.Empty, dictionary.Name);
    }

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
    public void TestIsEmpty()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>());

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.True(dictionary.IsEmpty());

        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>> {
                    {"against", new Dictionary<string, string> {{"contre", "against"}} }
                }
            );
        dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.False(dictionary.IsEmpty());
    }

    [Fact]
    public void TestAddTranslation()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>());

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        dictionary.AddTranslation("against", "contre");
        Assert.Equal(new[] { "contre" }, dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestAddTranslations()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>());

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        dictionary.AddTranslation("against", "contre");
        dictionary.AddTranslation("against", "versus");
        Assert.Equal(new[] { "contre", "versus" }, dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestNoTranslation()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>>());

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal(Array.Empty<string>(), dictionary.GetTranslation("against"));
    }

    [Fact]
    public void TestOneTranslation()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>> {
                    {"against", new Dictionary<string, string> {{"contre", "against"}} }
                }
            );

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal("contre", dictionary.GetTranslation("against")[0]);
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

    [Fact]
    public void TestReverseTranslation()
    {
        var mockDictionaryParser = new Mock<IDictionaryParser>();
        mockDictionaryParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, Dictionary<string, string>> {
                    {"against", new Dictionary<string, string> {{"contre", "against"}} }
                }
            );

        var dictionary = new Dictionary(mockDictionaryParser.Object);
        Assert.Equal("against", dictionary.GetTranslation("contre")[0]);
    }
}