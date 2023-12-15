using Moq;

namespace Translator.UnitTests;

public class TranslatorDataSourceTest
{
    [Fact]
    public void TestEmptyTranslatorName()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetName())
            .Returns(string.Empty);

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal(string.Empty, translator.Name);
    }

    [Fact]
    public void TestEmptyFile()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns([]);

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal([], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestTranslatorName()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetName())
            .Returns("en-fr");

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal("en-fr", translator.Name);
    }

    [Fact]
    public void TestIsEmpty()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns([]);

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.True(translator.IsEmpty());

        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, List<string>>
            {
                    {"against", ["contre"] }
            });
        translator = new Translator(mockTranslatorParser.Object);
        Assert.False(translator.IsEmpty());
    }

    [Fact]
    public void TestAddTranslation()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns([]);

        var translator = new Translator(mockTranslatorParser.Object);
        translator.AddTranslation("against", "contre");
        Assert.Equal<string[]>(["contre"], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestAddTranslations()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns([]);

        var translator = new Translator(mockTranslatorParser.Object);
        translator.AddTranslation("against", "contre");
        translator.AddTranslation("against", "versus");
        Assert.Equal<string[]>(["contre", "versus"], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestNoTranslation()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns([]);

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal([], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestOneTranslation()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, List<string>>
            {
                    {"against", ["contre"] }
            });

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal("contre", translator.GetTranslation("against")[0]);
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, List<string>>
            {
                { "against", ["contre", "versus"]}
            });

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal<string[]>(["contre", "versus"], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestErroneousFile()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Throws(new TranslatorException("The file is erroneous."));

        Assert.Throws<TranslatorException>(() => new Translator(mockTranslatorParser.Object));
    }

    [Fact]
    public void TestReverseTranslation()
    {
        var mockTranslatorParser = new Mock<ITranslatorParser>();
        mockTranslatorParser
            .Setup(dp => dp.GetTranslations())
            .Returns(new Dictionary<string, List<string>>
            {
                    {"against", ["contre"] }
            });

        var translator = new Translator(mockTranslatorParser.Object);
        Assert.Equal("against", translator.GetTranslation("contre")[0]);
    }
}