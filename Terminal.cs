//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================

namespace Assignment;

public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } = [];
    public Dictionary<string, Flight> Flights
    {
        get => Airlines.Values.SelectMany(a => a.Flights).ToDictionary(flight => flight.Key, flight => flight.Value);
    }
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } = [];
    public Dictionary<string, double> GateFees { get; set; } = [];

    public Terminal(string terminalname)
    {
        TerminalName = terminalname;
    }

    public bool AddAirline(Airline airline)
    {
        return Airlines.TryAdd(airline.Code, airline);
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        return BoardingGates.TryAdd(boardingGate.GateName, boardingGate);
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        var airlineCode = flight.FlightNumber[..2];
        return Airlines[airlineCode];  // TODO: Maybe use TryGetValue instead, but then the return type should be Airline? instead
    }

    public void PrintAirlineFees()
    {
        // Map flight numbers to boarding gates
        var flightGates = BoardingGates
            .Where(kv => kv.Value.Flight != null)
            .ToDictionary(kv => kv.Value.Flight.FlightNumber, kv => kv.Value);

        // Check that all flights have been assigned boarding gates
        if (Flights.Values.Any(f => !flightGates.ContainsKey(f.FlightNumber)))
        {
            Console.WriteLine("Error: All flights must have assigned boarding gates before running this feature");
            return;
        }

        Console.WriteLine("Airline");

        foreach (var a in Airlines.Values)
        {
            var fee = a.CalculateFees();  // Accumulate fees for arrival, departure, and special request codes
            var discount = 0.0;

            foreach (var f in a.Flights.Values)
            {
                // Accumulate fees for boarding gates
                fee += flightGates[f.FlightNumber].CalculateFees();

                // $110 discount for each flight arriving/departing before 11am or after 9pm
                if (f.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0) || f.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0))
                    discount += 110;

                // $25 discount for each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                    discount += 25.00;

                // $50 discount for each flight not indicating any Special Request Codes
                if (f is NORMFlight) discount += 50.00;
            }

            // 3% discount for more than 5 flights arriving/departing.
            // Note that this has to be applied after `fee` is fully calculated.
            if (a.Flights.Count > 5) discount += fee * 0.03;

            // $350 discount for every 3 flights arriving/departing
            discount += (Flights.Count / 3) * 350.00;

            Console.WriteLine($"{a.Name,-19} Fee subtotal: ${fee:0.00}");
            Console.WriteLine($"               Discount subtotal: ${discount:0.00}");
        }
    }


    public override string ToString()
    {
        return TerminalName;
    }
}