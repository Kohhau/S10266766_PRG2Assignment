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
        // Accumulate fees before discounts
        double fee = Flights.Values.Sum(f => f.CalculateFees());

        // 3% discount for more than 5 flights arriving/departing
        if (Flights.Count > 5) { fee *= 0.97; }

        foreach (var flight in Flights.Values)
        {
            // $110 discount for each flight arriving/departing before 11am or after 9pm
            if (flight.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0) || flight.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0))
            {
                fee -= 110;
            }

            // $25 discount for each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
            if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
            {
                fee -= 25.00;
            }

            // $50 discount for each flight not indicating any Special Request Codes
            if (flight is NORMFlight) { fee -= 50.00; }
        }

        // $350 discount for every 3 flights arriving/departing
        fee -= (Flights.Count / 3) * 350.00;
        return fee;

    }

    public override string ToString()
    {
        return $"Name: {Name}\nCode: {Code}";
    }

}