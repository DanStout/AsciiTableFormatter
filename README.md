# ![Logo](Docs/Logo.png) AsciiTableFormatter
A  C# library to generate ASCII Tables

## Examples:

```
var list = new[]
{
    new TestSix()
    {
        One = 3,
        Two = "Hi",
        Three = 123.34M
    },
    new TestSix()
    {
        One = 385745,
        Two = "???",
        Three = 84745.3498489754M
    }
};
string output = Formatter.Format(list);
Console.WriteLine(output);
```

Output:
```
o--------o-----o------------------o
|    One | Two |            Three |
o--------o-----o------------------o
|      3 | Hi  |           123.34 |
| 385745 | ??? | 84745.3498489754 |
o--------o-----o------------------o
```

## Build package

* Ensure Visual Studio 2017's MSBuild is on your path environment variable variable (for example, `C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin`)
* Run `msbuild /t:pack /p:Configuration=Release` in the directory containing `AsciiTableFormatter.sln`
* It will generate a `.nupkg` file in the `/bin/Release` directory
