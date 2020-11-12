<Query Kind="Statements">
  <Namespace>analyticsLibrary</Namespace>
  <Namespace>analyticsLibrary.library</Namespace>
  <Namespace>static UserQuery</Namespace>
</Query>


var scooter = new Motorcycle();
scooter.Dump();

var myCivic = new Sedan("red");
myCivic.Dump();

var kidsFit = myCivic with { color = "blue" };
kidsFit.Dump();

public record Automobile (int wheels, int doors);

public record Motorcycle : Automobile
{
	public Motorcycle() : base(2, 0)
	{ }
}

public record Sedan(string color) : Automobile(4, 4);