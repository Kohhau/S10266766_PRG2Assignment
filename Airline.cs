//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================
namespace Assignment;

public class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; } = [];

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public bool AddFlight(Flight flight)
    {
        return Flights.TryAdd(flight.FlightNumber, flight);
    }

    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }

    public double CalculateFees()
    {
        return Flights.Values.Sum(f => f.CalculateFees());
    }

    public override string ToString()
    {
        return $"Name: {Name}\nCode: {Code}";
    }

}