namespace Assignment;

public class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } = [];
    public Dictionary<string, Flight> Flights { get; set; } = [];
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } = [];
    public Dictionary<string, double> GateFees { get; set; } = [];

    public Terminal(string terminalname)
    {
        TerminalName = terminalname;
    }

    public bool AddAirline(Airline airline)
    {
        return Airlines.TryAdd(airline.Name, airline);
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

    public void PrintAirlineFees() { }


    public override string ToString() 
    {
        return TerminalName;
    }
}