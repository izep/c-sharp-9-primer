# C# 9 Primer <!-- omit in toc -->
Dev CoP
Friday, Nov. 13, 2020

## Index <!-- omit in toc -->

- [Top Level Statements](#top-level-statements)
- [`record` type](#record-type)
- [`init` set statement](#init-set-statement)
- [Pattern matching enhancements](#pattern-matching-enhancements)
- [Performance and interop](#performance-and-interop)
- [Targeted `new()` expressions](#targeted-new-expressions)
- [`static` anonymous functions](#static-anonymous-functions)
- [Covariant return types](#covariant-return-types)
- [Target-typed new expressions](#target-typed-new-expressions)
- [Extension `GetEnumerator` support for foreach loops](#extension-getenumerator-support-for-foreach-loops)
- [Attributes on local methods](#attributes-on-local-methods)
- [Module initializers](#module-initializers)
- [Partial method extension](#partial-method-extension)
- [Citations](#citations)
- [Resources](#resources)

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

This opens up a lot of possibilities for scripting and functional style programming.

> However, we probably want to avoid this for "normal" projects.
>
> -Preston

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

C# 8 introduced pattern matching in the switch statement.  C# 9 is introducing some enhancements with `and`, `is` and `or` operators.

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

Underscores can now be used as a discard operator in Lambda functions.

```c#
"Password".Select (_ => "*")
```

The `not` pattern is now available. Prior the `!` operator was needed for negation boolean checking.

```c#
//Before
if(!("Hi There!" is string))
  System.Console.WriteLine("Not a string");

//After
if("Hi There!" is not string)
  System.Console.WriteLine("Not a string");
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
var now = DateTime.Now;

phrase
	.Split()
	.Select(static p => p);

//Won't Compile, because static anonymous methods can't reference a local variable
//phrase
//	.Split()
//	.Select(static p => $"{now.ToShortDateString()}: {p}");
```

## Covariant return types

> Covariant return types provide flexibility for the return types of [override](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/override) methods. An override method can return a type derived from the return type of the overridden base method. This can be useful for records and for other types that support virtual clone or factory methods.

```c#
Sedan HondaCivic = new();

var myCivic = HondaCivic.Clone();

//Output: My Civic is a Sedan
Console.WriteLine($"My Civic is a {myCivic.GetType()}");

//Declarations
public class Automobile {
	public virtual Automobile Clone() => new Automobile();
}

public class Car : Automobile
{
	public override Car Clone() => new Car();
}

public class Sedan : Car
{
	public override Sedan Clone() => new Sedan();
}
```

## Target-typed new expressions

```c#
Weather today;
today = new(DateTime.Now, 100);
Console.WriteLine($"Todays's {today.GetType()} is {today.temperature} Degrees!");
//Output:
//Todays's Weather is 100 Degrees!

//Declartions
public record Weather(DateTime date, int temperature);
```

But this feature is particulary useful when creating object in-line for function calls.

```c#
void Main()
{
	Forecast(new(DateTime.Now, 100));

  //Output:
	//Todays's Weather is 100 Degrees!
}

//Declartions
public record Weather(DateTime date, int temperature);

public void Forecast(Weather today)
{
	Console.WriteLine($"Todays's {today.GetType()} is {today.temperature} Degrees!");
}
```

## Extension `GetEnumerator` support for foreach loops

The `foreach` loop can now be used on with values or objects that have a `GetEnumerator` extension method.

```c#
foreach (char l in "HelloWorld!")
	Console.WriteLine(l);

static class Extensions
{
	public static IEnumerator<char> GetEnumerator(this string x) =>
		x.Split().Cast<char>().GetEnumerator();

}
```

> In practice, this change means you can add `foreach` support to any type. You should limit its use to when enumerating an object makes sense in your design.

## Attributes on local methods

Attributes can now be added to local methods.  This allows for compiler processing changes and optimzations at the method level.  An example of this would be marking up a method as nullable.

```c#
void Main()
{
	Console.WriteLine($"Today's forecast is {TodaysForecast(new(DateTime.Now, 45))}");
}

public record Weather(DateTime date, int temperature);

//Creating the Nullable attribute for later optimizations
class Nullable : Attribute { }

[Nullable]
public string TodaysForecast(Weather today)
  => today.temperature switch
  {
	  < 32 => "freezing",
	  > 32 and <= 50 => "cold",
	  > 50 and <= 90 => "warm",
	  > 90 => null, //Words can just not describe this feeling;
  };

```

## Module initializers

> ...  Module initializers are methods that have the [ModuleInitializerAttribute](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.moduleinitializerattribute) attribute attached to them. These methods will be called by the runtime before any other field access or method invocation within the entire module. A module initializer method:
>
> - Must be static
> - Must be parameterless
> - Must return void
> - Must not be a generic method
> - Must not be contained in a generic class
> - Must be accessible from the containing module [Must at least be `internal` or `public`]

This sets an order of precedence for operations at runtime.  This is similar to the order of events for the lifetime of an object in some frameworks like Windows Forms or React.

## Partial method extension

Partial methods allow declaration of a method in one `partial` class and its implementaion in a later defined `partial` class

```c#
var kitty = new Animal();
Console.WriteLine($"{kitty.Roar()}!");

public partial class Animal {
	public partial string Roar();
}

public partial class Animal {
	public partial string Roar() => "Raaaaar";
}
```

This can be useful when creating the template for expected operations, but leaving the implementation to a code-generator or framework tool later on.

## Citations

- <a name="ref1">1</a>: LINQPad Samples: What's New in C# 9
- <a name="ref2">2</a>: [What's new in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9)
- <a name="ref3">3</a>: [An Introduction to the New Features in C# 9](https://medium.com/swlh/an-introduction-to-the-new-features-in-c-9-305dc8fb74d2)
- <a name="ref4">4</a>: [Reserved attributes contribute to the compiler's null state static analysis](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/attributes/nullable-analysis)

## Resources
- <a name="res1">1</a>: [LINQPad 6 Download - Beta](https://www.linqpad.net/LINQPad6.aspx#beta)
- <a name="res2">2</a>: [.Net Core 5 (SDK 5.0.100) Download](https://dotnet.microsoft.com/download/dotnet/5.0)
