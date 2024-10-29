[![Build](https://github.com/aelassas/tdd/actions/workflows/build.yml/badge.svg)](https://github.com/aelassas/tdd/actions/workflows/build.yml) [![Test](https://github.com/aelassas/tdd/actions/workflows/test.yml/badge.svg)](https://github.com/aelassas/tdd/actions/workflows/test.yml) [![codecov](https://codecov.io/gh/aelassas/tdd/graph/badge.svg?token=1ZOJ3K0U8B)](https://codecov.io/gh/aelassas/tdd)

Test-Driven Development (TDD) is a powerful approach that transforms how developers write code. Whether you're new to programming or looking to level up your skills, this guide will walk you through the essentials of TDD. You'll discover how writing tests before code can lead to more robust, maintainable software and boost your confidence as a developer. Let's explore the fundamentals together and set you on the path to becoming a test-driven practitioner.

## Contents

1.  [Introduction](#intro)
2.  [Development Environment](#dev-env)
3.  [Prerequisites](#prerequisites)
4.  [Fake it!](#fake-it)
5.  [Triangulation](#triangulation)
6.  [Multiple Translations](#multiple-translations)
7.  [Reverse Translation](#reverse-translation)
8.  [File Loading](#file-loading)
    1.  [TranslatorDataSourceTest](#TranslatorDataSourceTest)
    2.  [TranslatorParserTest](#TranslatorParserTest)
    3.  [TranslatorLoaderTest](#TranslatorLoaderTest)
9.  [Class Diagram](#class-diagram)
10.  [Test Results](#test-results)
11.  [Code Coverage](#code-coverage)
12.  [Running the Source Code](#run-src)
13.  [Is TDD a Time Waster?](#tdd-time-waster)
14.  [Conclusion](#conclusion)

## <a id="intro" name="intro">Introduction</a>

The traditional approach to writing unit tests involves writing tests to check the validity of your code. First, you begin by writing the code and then you write the tests. This is the opposite of test-driven development.

![](https://github.com/aelassas/tdd/blob/main/img/intro-2.png?raw=true)

Test-driven development (TDD) involves writing tests before writing code, as shown in the workflow above.

First, the test is written and must fail at the beginning. Then, we write the code so that the test passes. Then, the test must be executed and must succeed. Then, the code is refactored. Then, the test must be performed again to ensure that the code is correct.

To summarize, this is done in five steps:

1.  Write a test.
2.  The test must fail at the beginning.
3.  Write the code so that the test passes.
4.  Execute the test and make sure it passes.
5.  Refactor the code.

We can notice that in the workflow explained above, the tests are executed after the code has been refactored. This ensures that the code remains correct after refactoring.

This article will discuss TDD through a very simple example. The purpose of the example is to describe each step of TDD. The example will be developed in C# and the testing framework used is xUnit. We will be using Moq for mocking and dotCover for code coverage. We will be creating a multilingual translator through TDD. When writing code, we will try to respect SOLID principles and achieve 100% code coverage.

## <a id="dev-env" name="dev-env">Development Environment</a>

*   Visual Studio 2022 >= 17.8.0
*   .NET 8.0

## <a id="prerequisites" name="prerequisites">Prerequisites</a>

*   C#
*   xUnit
*   Moq

## <a name="fake-it">Fake it!</a>

The first task to achieve when using TDD is important: it must be so simple that the loop red-green-refactor can be completed quickly.

We first create a test class called `TranslatorTest`:

```cs
public class TranslatorTest
{
}
```

Then we will create the first unit test in this class, we initialize an object of type `Translator` with the name `"en-fr"` and we will check if the name is correct:

```cs
public class TranslatorTest
{
    [Fact]
    public void TestTranslatorName()
    {
        var translator = new Translator("en-fr");
        Assert.Equal("en-fr", translator.Name);
    }
}
```

The test will fail, which is what we want.

Now that we have reached the red bar, we will write some code so that the test passes. There are many ways to do this. We will use the "`Fake it!`" method. Specifically, it includes the minimum required to pass the test. In our case, it is enough to write a `Translator` class that returns the property Name of `"en-fr"`:

```cs
public class Translator
{
    public string Name => "en-fr";
}
```

We will create the code step by step by using simple and quick methods in each step. Now, if we run our unit test again, it will pass. But the code is not refactored. There is a redundancy. Indeed, `"en-fr"` is repeated twice. We will refactor the code:

```cs
public class Translator(string name)
{
    public string Name => name;
}
```

After refactoring the code, we have to run the tests again to make sure the code is correct.

Code refactoring is a form of modifying code that preserves the execution of existing tests and obtains a software architecture with minimal defects. Some examples:

*   Remove duplicate code / move code.
*   Adjust `private` / `public` properties / methods.

We noticed that we have completed the cycle of TDD workflow. Now we can start the cycle over and over again with new tests.

## <a id="triangulation" name="triangulation">Triangulation</a>

In TDD, we write tests first, generating functional requirements before coding requirements. To refine the test, we will apply triangulation method.

Let's write a test that checks if a translation has been added to the translator (`AddTranslation`).

Testing will be done through `GetTranslation` method.

```cs
[Fact]
public void TestOneTranslation()
{
    var translator = new Translator("en-fr");
    translator.AddTranslation("against", "contre");
    Assert.Equal("contre", translator.GetTranslation("against"));
}
```

If we run the test, we'll notice that it fails. Okay, so that's what we're looking for at this step.

First, we'll use the "Fake it!" method to pass the test:

```cs
public class Translator(string name)
{
    public string Name => name;

    public void AddTranslation(string word, string translation)
    {
    }

    public string GetTranslation(string word) => "contre";
}
```

After running the test `TestOneTranslation`, we will notice that it passes.

But wait, there's code duplication. The keyword `"contre"` is repeated twice in the code. We will change the code to remove this duplication:

```cs
public class Translator(string name)
{
    private readonly Dictionary<string, string> _translations = new();
    public string Name => name;

    public void AddTranslation(string word, string translation)
    {
        _translations.Add(word, translation);
    }

    public string GetTranslation(string word) => _translations[word];
}
```

After refactoring the code, we have to run the tests again to make sure the code is correct.

Let's add a test to check if the translator is empty:

```cs
[Fact]
public void TestIsEmpty()
{
    var translator = new Translator("en-fr");
    Assert.True(translator.IsEmpty());
}
```

If we run the test, we'll notice that it fails. Okay, so that's what we're looking for at this step. Let's use the "Fake it!" method and write some code to pass the test:

```cs
public class Translator(string name)
{
    [...]

    public bool IsEmpty() => true;
}
```

If we run the test, we'll notice that it passes.

Now, let's use triangulation technique by using two assertions to drive the generalization of the code:

```cs
[Fact]
public void TestIsEmpty()
{
    var translator = new Translator("en-fr");
    Assert.True(translator.IsEmpty());
    translator.AddTranslation("against", "contre");
    Assert.False(translator.IsEmpty());
}
```

Now if we run the test again, It will fail because of the second assertion. This is called triangulation.

So let's fix this:

```cs
public class Translator(string name)
{
    [...]

    public bool IsEmpty() => _translations.Count == 0;
}
```

If we run the test again, we'll notice that it passes.

## <a id="multiple-translations" name="multiple-translations">Multiple Translations</a>

One feature of the translator is the ability to manipulate multiple translations. This use case was not initially planned in our architecture. Let’s write the test first:

```cs
[Fact]
public void TestMultipleTranslations()
{
    var translator = new Translator("en-fr");
    translator.AddTranslation("against", "contre");
    translator.AddTranslation("against", "versus");
    Assert.Equal<string[]>(["contre", "versus"], 
                 translator.GetMultipleTranslations("against"));
}
```

If we run the test, we'll notice that it fails. Okay, so that's what we're looking for at this step. First, we will use the "Fake it!" method to pass the test by modifying the method `AddTranslation` and adding the `GetMultipleTranslations` method:

```cs
public class Translator(string name)
{
    private readonly Dictionary<string, List<string>> _translations = new();
    public string Name => name;

    public string[] GetMultipleTranslation(string word) => ["contre", "versus"];

    [...]
}
```

After running the test `TestMultipleTranslations`, we will notice that it passes. But wait, there's code duplication. The string array `["contre", "versus"]` is repeated twice in the code. We will change the code to remove this duplication:

```cs
public class Translator(string name)
{
    private readonly Dictionary<string, List<string>> _translations = new();
    public string Name => name;

    public void AddTranslation(string word, string translation)
    {
        if (_translations.TryGetValue(word, out var translations))
        {
            translations.Add(translation);
        }
        else
        {
            _translations.Add(word, [translation]);
        }
    }

    public string[] GetMultipleTranslation(string word) => [.. _translations[word]];

    public string GetTranslation(string word) => _translations[word][0];

    public bool IsEmpty() => _translations.Count == 0;
}
```

If we run the test again, we'll notice that it passes. Let's do some refactoring and rename `GetMultipleTranslations` to `GetTranslation`:

```cs
public class Translator(string name)
{
    private readonly Dictionary<string, List<string>> _translations = new();
    public string Name => name;

    public void AddTranslation(string word, string translation)
    {
        if (_translations.TryGetValue(word, out var translations))
        {
            translations.Add(translation);
        }
        else
        {
            _translations.Add(word, [translation]);
        }
    }

    public string[] GetTranslation(string word) => [.. _translations[word]];

    public bool IsEmpty() => _translations.Count == 0;
}
```

We also have to change our tests:

```cs
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
        Assert.Equal<string[]>(["contre"], translator.GetTranslation("against"));
    }

    [Fact]
    public void TestMultipleTranslations()
    {
        var translator = new Translator("en-fr");
        translator.AddTranslation("against", "contre");
        translator.AddTranslation("against", "versus");
        Assert.Equal<string[]>(["contre", "versus"], translator.GetTranslation("against"));
    }
}
```

## <a id="reverse-translation" name="reverse-translation">Reverse Translation</a>

Now suppose we want to consider translation in both directions, for example, a bilingual translator. Let's create the test first:

```cs
[Fact]
public void TestReverseTranslation()
{
    var translator = new Translator("en-fr");
    translator.AddTranslation("against", "contre");
    Assert.Equal<string[]>(["against"], translator.GetTranslation("contre"));
}
```

If we run the test, we'll notice that it fails. Okay, so that's what we're looking for at this step. Now, let's write the code to pass the test by using the "Fake it!" method:

```cs
public string[] GetTranslation(string word)
{
    if (_translations.TryGetValue(word, out var translations))
    {
        return [.. translations];
    }

    // Try reverse translation
    return ["against"];
}
```

The test will pass. But there is a code duplication. Indeed, `"against"` is repeated twice. So let's refactor the code:

```cs
public string[] GetTranslation(string word)
{
    if (_translations.TryGetValue(word, out var translations))
    {
        return [.. translations];
    }

    // Try reverse translation
    return [.. from t in _translations
               where t.Value.Contains(word)
               select t.Key];
}
```

If we run the test again, we'll notice that it passes.

## <a id="file-loading" name="file-loading">File Loading</a>

Now, let's handle loading translations from a data source (such as an external text file). Let's focus on external text files for now. The input format will be a text file where the first line contains the name of the translator and the other lines contain words separated by `" = "` . Here is an example:

```
en-fr
against = contre
against = versus
```

Here is the list of tests we will perform:

1.  Empty file.
2.  File containing only translator name.
3.  File with translations.
4.  Wrong file.

First, we'll use mocks to write tests. Then we'll write code along the way. Then we'll refactor the code. Finally, we'll test the code to make sure we refactored correctly and everything is working properly. We will create three new test classes:

*   `TranslatorDataSourceTest`: We will test a translator loaded from an external data source.
*   `TranslatorParserTest`: We will test the parsing of loaded translator data.
*   `TranslatorLoaderTest`: We will test the loading of translator data loaded from an external data source.

### <a id="TranslatorDataSourceTest" name="TranslatorDataSourceTest">TranslatorDataSourceTest</a>

#### Empty Translator Name

First, let's write the test:

```cs
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
```

The test will fail. Okay, so that's what we're looking for at this step. We will use the interface `ITranslatorParser` to parse translator data loaded from external data source. Following is the interface `ITranslatorParser`:

```cs
public interface ITranslatorParser
{
    string GetName();
}
```

Let's modify the `Translator` class using the "Fake it!" method to pass the test:

```cs
public class Translator
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Translator(string name)
    {
        _translations = [];
        Name = name;
    }

    public Translator(ITranslatorParser parser)
    {
        Name = string.Empty;
    }

    [...]
}
```

If we run the test again, we'll notice that it passes. But wait, there's a duplication in the code. Indeed, `string.Empty` is repeated twice. So, let's do some refactoring:

```cs
public class Translator
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Translator(string name)
    {
        _translations = [];
        Name = name;
    }

    public Translator(ITranslatorParser parser)
    {
        Name = parser.GetName();
    }
}
```

If we run the test again, we'll notice that it passes.

#### No Translation

First, let's start by writing a test:

```cs
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
```

We will notice that the test will fail. Okay, so that's what we're looking for at this step. First, let's modify the interface `ITranslatorParser`:

```cs
public interface ITranslatorParser
{
    string GetName();
    Dictionary<string, List<string>> GetTranslations();
}
```

Then, let's write some code to pass the test using the "Fake it!" method:

```cs
public class Translator
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Translator(string name)
    {
        _translations = [];
        Name = name;
    }

    public Translator(ITranslatorParser parser)
    {
        _translations = [];
        Name = parser.GetName();
    }

    [...]
}
```

If we run the test again, we'll notice that it passes. But wait, there's a duplication in the code. In fact, the translator initialization is repeated twice. So, let's do some refactoring:

```cs
public class Translator
{
    private readonly Dictionary<string, List<string>> _translations;
    public string Name { get; private set; }

    public Translator(string name)
    {
        _translations = [];
        Name = name;
    }

    public Translator(ITranslatorParser parser)
    {
        _translations = parser.GetTranslations();
        Name = parser.GetName();
    }

  [...]
}
```

If we run the test again, we'll notice that it passes.

#### File with only Translator Name

First, let's start by writing a test:

```cs
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
```

We will notice that the test will pass because we have written the interface `ITranslatorParser` and changed the `Translator` class. Currently, this unit does not require refactoring.

#### One Translation

First, let's start by writing a test:

```cs
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
    Assert.Equal<string[]>(["contre"], translator.GetTranslation("against"));
}
```

We will notice that the test will pass because we have written the interface `ITranslatorParser` and changed the `Translator` class. Currently, this unit does not require refactoring.

#### Multiple Translations

First, let's start by writing a test:

```cs
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
```

We will notice that the test will pass because we have written the interface `ITranslatorParser` and changed the `Translator` class. Currently, this unit does not require refactoring.

#### Wrong File

First, let's start by writing a test:

```cs
[Fact]
public void TestErroneousFile()
{
    var mockTranslatorParser = new Mock<ITranslatorParser>();
    mockTranslatorParser
        .Setup(dp => dp.GetTranslations())
        .Throws(new TranslatorException("The file is erroneous."));

    Assert.Throws<TranslatorException>(() => new Translator(mockTranslatorParser.Object));
}
```

We will notice that the test will pass because we have written the interface `ITranslatorParser` and changed the `Translator` class. Currently, this unit does not require refactoring.

### <a id="TranslatorParserTest" name="TranslatorParserTest">TranslatorParserTest</a>

Now let's create a class to parse loaded translator data through `ITranslatorLoader`, which loads translator data from external data source.

#### Empty Translator Name

First, let's start by writing a test:

```cs
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
```

The test will fail. Okay, so that's what we're looking for at this step. We will use an interface `ITranslatorLoader` to load translator data from an external data source. The following is the interface `ITranslatorLoader`:

```cs
public interface ITranslatorLoader
{
    string[] GetLines();
}
```

Let's write some code to pass the test using the "Fake it!" method:

```cs
public class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    public string GetName() => string.Empty;
    public Dictionary<string, List<string>> GetTranslations() => new();
}
```

The test will pass. Let's move on to other units.

#### No Translation

Let's start by writing a test:

```cs
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
```

The test will pass. Let's move on to other units.

#### Translator Name

Let's start by writing a test:

```cs
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
```

The test will fail. Okay, so that's what we're looking for in this step. Let's write some code to pass the test using the "Fake it!" method:

```cs
public class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    public string GetName() => "en-fr";
    public Dictionary<string, List<string>> GetTranslations() => new();
}
```

The test will pass. But wait, there is a duplication in the code and the test `TestEmptyTranslatorName` fails. So let's solve this issue:

```cs
public class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;
    public Dictionary<string, List<string>> GetTranslations() => new();
}
```

Now, the test will pass.

#### One Translation

Let's start by writing a test:

```cs
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
```

The test will fail. Okay, so that's what we're looking for at this step. Let's write some code to pass the test using the "Fake it!" method:

```cs
public class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;
    public Dictionary<string, List<string>> GetTranslations() => new()
    {
        {"against", ["contre"]}
    };
}
```

The test will pass. But wait, there is a duplication in the code and the test `TestNoTranslation` fails. So let's fix this:

```cs
public partial class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    private static readonly Regex TranslatorRegex = new(@"^(?<key>\w+) = (?<value>\w+)$");

    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;

    public Dictionary<string, List<string>> GetTranslations()
    {
        var translator = new Dictionary<string, List<string>>();

        if (_lines.Length <= 1)
        {
            return translator;
        }

        for (var i = 1; i < _lines.Length; i++)
        {
            var line = _lines[i];
            var match = TranslatorRegex.Match(line);

            var key = match.Groups["key"].Value;
            var value = match.Groups["value"].Value;

            if (translator.TryGetValue(key, out var translations))
            {
                translations.Add(value);
            }
            else
            {
                translator.Add(key, [value]);
            }
        }

        return translator;
    }
}
```

Now the test will pass. The method `GetTranslations` just parses the lines loaded by `ITranslatorLoader`.

#### Multiple Translations

Let's start by writing a test:

```cs
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
```

We will notice that the test will pass because we implemented `TranslatorParser`. Currently, this unit does not require refactoring.

#### Wrong File

One of the features we haven't implemented yet is handling the loading of wrong files. This use case was not initially planned in our architecture. Let's write the test first:

```cs
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
```

The test will fail. Okay, so that's what we're looking for at this step. Let's update the code to pass the test:

```cs
public partial class TranslatorParser(ITranslatorLoader loader) : ITranslatorParser
{
    private static readonly Regex TranslatorRegex = new(@"^(?<key>\w+) = (?<value>\w+)$");

    private readonly string[] _lines = loader.GetLines();

    public string GetName() => _lines.Length > 0 ? _lines[0] : string.Empty;

    public Dictionary<string, List<string>> GetTranslations()
    {
        var translator = new Dictionary<string, List<string>>();

        if (_lines.Length <= 1)
        {
            return translator;
        }

        for (var i = 1; i < _lines.Length; i++)
        {
            var line = _lines[i];
            var match = TranslatorRegex.Match(line);

            if (!match.Success)
            {
                throw new TranslatorException("The file is erroneous.");
            }

            var key = match.Groups["key"].Value;
            var value = match.Groups["value"].Value;

            if (translator.TryGetValue(key, out var translations))
            {
                translations.Add(value);
            }
            else
            {
                translator.Add(key, [value]);
            }
        }

        return translator;
    }
}
```

Now, we'll notice that the test will pass because we handled the case of wrong files by throwing `TranslatorException` in case of a wrong line.

### <a id="TranslatorLoaderTest" name="TranslatorLoaderTest">TranslatorLoaderTest</a>

Now, we will create a class that loads translator data from an external file.

#### Empty File

Let's start with the first test that tests an empty file:

```cs
[Fact]
public void TestEmptyFile()
{
    var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-empty.txt");
    Assert.Equal([], translatorLoader.GetLines());
}
```

The test will fail. Okay, so that's what we're looking for at this step. Now, let's write some code to pass the test using the "Fake it!" method:

```cs
public class TranslatorLoader(string path) : ITranslatorLoader
{
    public string[] GetLines() => [];
}
```

Now the test will pass. But wait, there's code duplication. In fact, The empty string array is repeated twice in the code. So, let's do some refactoring:

```cs
public class TranslatorLoader(string path) : ITranslatorLoader
{
    public string[] GetLines() => File.ReadAllLines(path);
}
```

Now, if we run the test again, we'll see that it passes.

#### Files with only Translator Names

Now, let's use the following text file (_translator-name.txt_):

```
en-fr
```

Let's start by writing the test:

```cs
[Fact]
public void TestTranslatorName()
{
    var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-name.txt");
    Assert.Equal<string[]>(["en-fr"], translatorLoader.GetLines());
}
```

The test will pass because we implemented the class `TranslatorLoader` in the previous test. Let's move on to other units.

#### Files with Translations

Now, let's use the following translator file (_translator.txt_):

```
en-fr
against = contre
against = versus
```

Let's start by writing the test:

```cs
[Fact]
public void TestMultipleTranslations()
{
    var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator.txt");
    Assert.Equal<string[]>(["en-fr", "against = contre", "against = versus"], translatorLoader.GetLines());
}
```

Again, the test will pass because we implemented the `TranslatorLoader` class in previous tests.

#### Wrong File

Now, let's use the following translator file (_translator-erroneous.txt_):

```
en-fr
against = 
against = 
```

Let's write the test first:

```cs
[Fact]
public void TestErroneousFile()
{
    var translatorLoader = new TranslatorLoader(@"..\..\..\..\..\data\translator-erroneous.txt");
    Assert.Equal<string[]>(["en-fr", "against = ", "against = "], translatorLoader.GetLines());
}
```

Again, the test will pass because we implemented the `TranslatorLoader` class in previous tests. We completed the testing and created the `TranslatorLoader` class responsible for loading translator data from an external file.

## <a id="class-diagram" name="class-diagram">Class Diagram</a>

We have finished coding our multilingual translator through TDD.

The following is the class diagram:

![](https://github.com/aelassas/tdd/blob/main/img/class-diagram-4.png?raw=true)

## <a id="test-results" name="test-results">Test Results</a>

If we run all the tests, we'll notice that they all pass:

![](https://github.com/aelassas/tdd/blob/main/img/tests-6.png?raw=true)

You can find test results on [GitHub Actions](https://github.com/aelassas/tdd/actions/workflows/test.yml).

## <a id="code-coverage" name="code-coverage">Code Coverage</a>

Here is the code coverage:

![](https://github.com/aelassas/tdd/blob/main/img/coverage-6.png?raw=true)

We'll notice that we've reached 100% code coverage. This is one of the advantages of TDD.

You can find code coverage report on [Codecov](https://app.codecov.io/gh/aelassas/tdd).

## <a id="run-src" name="run-src">Running the Source Code</a>

To run the source code, do the following:

1.  Download the source code.
2.  Open _tdd.sln_ in Visual Studio 2022.
3.  Run all the tests in the solution.
4.  To get code coverage, you can use dotCover.

## <a id="tdd-time-waster" name="tdd-time-waster">Is TDD a Time Waster?</a>

The short answer is no - but the reality deserves a thoughtful discussion.

While TDD requires upfront time investment in writing tests before code, it typically saves significant time in the long run through:

**Reduced Debugging Time**

- Tests catch bugs immediately rather than during later testing phases
- Problems are easier to fix when the related code is fresh in your mind
- Fewer issues make it to production, saving emergency debugging sessions

**Improved Code Quality**

- Writing tests first forces better design decisions
- Code is naturally more modular and maintainable
- Refactoring becomes safer and faster with test coverage

**Documentation Benefits**

- Tests serve as living documentation of how code should behave
- New team members can understand expectations by reading tests
- Less time spent writing and maintaining separate documentation


**Faster Development Cycles**

  - While initial development might feel slower, overall delivery speeds up
  - Less time spent on bug fixes and rework
  - More confident and rapid deployments

Common Time-Related Misconceptions:

- "Writing tests doubles development time" - Actually, TDD often reduces total development time when accounting for debugging and maintenance
- "TDD slows down prototyping" - You can adjust test coverage based on project phase
- "Tests take too long to maintain" - Well-written tests require less maintenance than fixing recurring bugs

The key is viewing TDD as an investment rather than overhead. Like any skill, it takes time to master, but the returns in code quality, developer confidence, and long-term maintenance costs make it worthwhile for most projects.

## <a id="conclusion" name="conclusion">Conclusion</a>

This article demonstrates TDD through a very simple example.

We can notice with TDD:

*   Unit tests were written.
*   We achieved 100% code coverage.
*   We spent less time debugging.
*   The code respects SOLID principles.
*   The code is maintainable, flexible and extensible.
*   The code is more coherent.
*   Behavior is clear.

Test-Driven Development shines brightest in environments where code evolves continuously. By breaking down software into small, testable units, TDD empowers developers to make changes with confidence. This approach not only ensures code quality but fundamentally shifts how we think about software design. Rather than viewing applications as monolithic structures, TDD encourages a modular mindset where components are crafted independently, thoroughly tested, and seamlessly integrated. As you begin your TDD journey, remember that you're not just learning a testing methodology – you're adopting a development philosophy that will help you build more maintainable, reliable, and scalable software.
