# C# 9 Primer <!-- omit in toc -->

## Index <!-- omit in toc -->

- [Top Level Statements](#top-level-statements)
- [`record` type](#record-type)
- [`init` set statement](#init-set-statement)
- [Pattern matching enhancements](#pattern-matching-enhancements)
- [Performance and interop](#performance-and-interop)
- [Targeted `new()` expressions](#targeted-new-expressions)
- [`static` anonymous functions](#static-anonymous-functions)
- [Citations](#citations)

## Top Level Statements

Programs can now be as simple as:

```c#
System.Console.WriteLine("HelloWorld!");
```

Using Statements may also be built inline:

```c#
using System;

Console.WriteLine("HelloWorld!");
```

Inline functions can be top level statements:

```c#
void Print(string something) => System.Console.WriteLine($"Something: {something}");

Print("Else");
```

This opens up a lot of possibilities for scripting and functional style programming. However, we probably want to avoid this for "normal" projects.

[Example](top-level-statements/)

## `record` type

> [Records] are defined not by their identity, but by their contents. -Mads Torgersen

Characteristics:
- Immutable
- Extra built in compile time additions
  - `IEquatable<t>`
    - `.Equals()`
    - `.GetHashCode()`
    - `.Clone()`
  - `override .ToString()`

[Example](record/record-il-spy.linq)

Inheritance is available:

```c#
public record Automobile {
    public int wheels { get; init; }
    public int doors { get; init; }
}

public record Motorcycle : automobile {
    public Motorcycle() {
        this.wheels = 2;
        this.doors = 0;
    }
}
```

[Example](record/record-inheritance.linq)

## `init` set statement

"Init only setters provide consistent syntax to initialize members of an object. Property initializers make it clear which value is setting which property. The downside is that those properties must be settable."<sup>[[1](#ref2)]</sup>

```c#
var tomAndJerry = new Cartoon() { Name = "Tom and Jerry", Year = 1940 };

//Compile/linting time error
tomAndJerry.Year = 1930;

public class Cartoon {
    public string Name { get; init; }
    public int Year { get; init; }
}
```

## Pattern matching enhancements

C# 8 introduced pattern matching in the switch statement.  C# 9 is introducing some enhancements with `and` and `or` operators.

```c#
var today = new Weather(DateTime.Now, 30);

var forecast = new Func<Weather, string>(
	(_today) => _today.temperature switch
	{
		< 32 => "cold",
		>= 32 and < 60 => "mild",
		>= 60 and < 85 => "pleasant",
		>= 85 => "hot"
	});

Console.WriteLine($"Today will be {forecast(today)}");

public record Weather(DateTime date, int temperature);
```

Pattern matching can also be applied to `if` statements.

```c#
var today = new Weather(DateTime.Now, 90, 80);

if (today is { temperature: >= 85, barometer: >= 60 })
	Console.WriteLine($"Today will be hot & muggy.");

public record Weather(DateTime date, int temperature, int barometer
```

## [Performance and interop](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#performance-and-interop)
- Native sized integers
    > Native sized integers define properties for MaxValue or MinValue. These values can't be expressed as compile time constants because they depend on the native size of an integer on the target machine.
- Function pointers
    > Invoking the `delegate*` type uses `calli`, in contrast to a delegate that uses `callvirt` on the `Invoke()` method. Syntactically, the invocations are identical.
- Omitting the locals `init` flag
    > ...you can add the `System.Runtime.CompilerServices.SkipLocalsInitAttribute` to instruct the compiler not to emit the `localsinit` flag. This flag instructs the CLR to zero-initialize all local variables...You may add it to a single method or property, or to a class, struct, interface, or even a module.

## Targeted `new()` expressions

```c#
Weather today = new(DateTime.Now, 80);

Console.WriteLine($"Today: {today}");

public record Weather(DateTime date, int temperature);

```

```c$
Something toPrint = new() { print = "else." };
Console.WriteLine($"Something {toPrint}");

public record Something {
	public string print { get; init; }
}
```

## `static` anonymous functions

> Starting in C# 9.0, you can add the `static` modifier to lambda expressions or anonymous methods. Static lambda expressions are analogous to the `static` local functions: a `static` lambda or anonymous method can't capture local variables or instance state. The `static` modifier prevents accidentally capturing other variables.

```c#
var phrase = "How now brown cow?";

Action<string> printLower = static (thing)
	=> Console.WriteLine(thing.ToLower());

printLower(phrase);
```

## Citations
- <a name="ref1">1</a>: LINQPad Samples: What's New in C# 9
- <a name="ref2">2</a>: [What's new in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9)
- <a name="ref3">3</a>: [An Introduction to the New Features in C# 9](https://medium.com/swlh/an-introduction-to-the-new-features-in-c-9-305dc8fb74d2)

