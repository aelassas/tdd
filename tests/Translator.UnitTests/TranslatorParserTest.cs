using Moq;

namespace Translator.UnitTests;

public class TranslatorParserTest
{
    [Fact]
    public void TestEmptyTranslatorName()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(Array.Empty<string>());

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal(string.Empty, translatorParser.GetName());
    }

    [Fact]
    public void TestNoTranslations()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(Array.Empty<string>());

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal(new Dictionary<string, List<string>>(), translatorParser.GetTranslations());
    }

    [Fact]
    public void TestTranslatorName()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr" });

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal("en-fr", translatorParser.GetName());
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr", "against = contre", "against = versus" });

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        var expected = new Dictionary<string, List<string>>
        {
            {"against", ["contre","versus"]}
        };
        Assert.Equal(expected, translatorParser.GetTranslations());
    }

    [Fact]
    public void TestErroneousFile()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(new[] { "en-fr", "against = ", "against = " });

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Throws<TranslatorException>(() => translatorParser.GetTranslations());
    }
}