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
            .Returns([]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal(string.Empty, translatorParser.GetName());
    }

    [Fact]
    public void TestNoTranslation()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns([]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal([], translatorParser.GetTranslations());
    }

    [Fact]
    public void TestTranslatorName()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(["en-fr"]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Equal("en-fr", translatorParser.GetName());
    }

    [Fact]
    public void TestOneTranslation()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(["en-fr", "against = contre"]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        var expected = new Dictionary<string, List<string>>
        {
            {"against", ["contre"]}
        };
        Assert.Equal(expected, translatorParser.GetTranslations());
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(["en-fr", "against = contre", "against = versus"]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        var expected = new Dictionary<string, List<string>>
        {
            {"against", ["contre", "versus"]}
        };
        Assert.Equal(expected, translatorParser.GetTranslations());
    }

    [Fact]
    public void TestErroneousFile()
    {
        var mockTranslatorLoader = new Mock<ITranslatorLoader>();
        mockTranslatorLoader
            .Setup(dl => dl.GetLines())
            .Returns(["en-fr", "against = ", "against = "]);

        var translatorParser = new TranslatorParser(mockTranslatorLoader.Object);
        Assert.Throws<TranslatorException>(translatorParser.GetTranslations);
    }
}
