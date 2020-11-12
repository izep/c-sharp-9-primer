# C# 9 Primer <!-- omit in toc -->

## Index <!-- omit in toc -->

- [Top Level Statements](#top-level-statements)
- [`record` type](#record-type)
- [`init` set statement](#init-set-statement)
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

## `record` type

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

"Init only setters provide consistent syntax to initialize members of an object. Property initializers make it clear which value is setting which property. The downside is that those properties must be settable."<sup>[[1](#ref1)]</sup>


## Citations
<a name="ref1">1</a>: [What's new in C# 9.0](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9)