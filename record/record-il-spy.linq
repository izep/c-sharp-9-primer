<Query Kind="Statements" />

//Top Level Statements must appear before class and namespace declarations
Util.OpenILSpy(typeof(WeatherClass));

Util.OpenILSpy(typeof(WeatherRecord));

public class WeatherClass
{
	public DateTime date { get; set; }
	public double temperature { get; set; }
	public double barometric_pressue { get; set; }
}


//Essentially implements IEquatable<t>
public record WeatherRecord
{
	public DateTime date { get; set; }	
	public double temperature { get; set; }
	public double barometric_pressue { get; set; }
}

