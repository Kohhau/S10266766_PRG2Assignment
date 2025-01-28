//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================

using Assignment;

Terminal terminal5 = new("Changi Airport Terminal 5");
LoadAirlines(terminal5);
LoadBoardingGates(terminal5);
LoadFlights(terminal5);

var firstRun = true;
while (true)
{
    if (!firstRun) Console.WriteLine();
    firstRun = false;

    DisplayMenu();
    Console.Write("Enter choice: ");
    switch (Console.ReadLine())
    {
        case "1":
            Console.WriteLine();
            DisplayBasicInformation(terminal5);
            break;
        case "2":
            Console.WriteLine();
            DisplayBoardingGates(terminal5);
            break;
        case "3":
            AssignBoardingGateToFlight(terminal5);
            break;
        case "4":
            Console.WriteLine();
            // CreateNewFlight(terminal5);
            break;
        case "5":
            Console.WriteLine();
            // DisplayFullFlightDetails(terminal5);
            break;
        case "6":
            Console.WriteLine();
            // ModifyFlightDetails(terminal5);
            break;
        case "7":
            Console.WriteLine();
            // DisplayScheduledFlights(terminal5);
            break;
        case "8":
            return;
        default:
            Console.WriteLine("Invalid choice; please try again.");
            break;
    }
}

//=========================================
// Loading data from CSVs (feature 1 & 2)
//=========================================

void LoadAirlines(Terminal terminal)
{
    using var reader = new StreamReader("airlines.csv");
    var line = reader.ReadLine();  // Skip header

    while ((line = reader.ReadLine()) != null)
    {
        var values = line.Split(",");
        terminal.AddAirline(new(values[0], values[1]));
    }
}

void LoadBoardingGates(Terminal terminal)
{
    using var reader = new StreamReader("boardinggates.csv");
    var line = reader.ReadLine();  // Skip header

    while ((line = reader.ReadLine()) != null)
    {
        var values = line.Split(",");
        terminal.AddBoardingGate(new(values[0], Convert.ToBoolean(values[1]), Convert.ToBoolean(values[2]), Convert.ToBoolean(values[3])));
    }
}

void LoadFlights(Terminal terminal)
{
    using var reader = new StreamReader("flights.csv");
    var line = reader.ReadLine();  // Skip header

    while ((line = reader.ReadLine()) != null)
    {
        var values = line.Split(",");
        Flight f;

        if (values[4] == "") { f = new NORMFlight(values[0], values[1], values[2], DateTime.Parse(values[3])); }
        else if (values[4] == "LWTT") { f = new LWTTFlight(values[0], values[1], values[2], DateTime.Parse(values[3])); }
        else if (values[4] == "DDJB") { f = new DDJBFlight(values[0], values[1], values[2], DateTime.Parse(values[3])); }
        else { f = new CFFTFlight(values[0], values[1], values[2], DateTime.Parse(values[3])); }

        terminal.GetAirlineFromFlight(f).AddFlight(f);
    }
}

//===============================================
// Displaying information (feature 3, 4, 7 & 9)
//===============================================
void DisplayBasicInformation(Terminal terminal)
{
    Console.WriteLine("Flight  Airline name        Origin              Destination         Expected departure/arrival time");

    foreach (var a in terminal.Airlines.Values)
        foreach (var f in a.Flights.Values)
            Console.WriteLine($"{f.FlightNumber,-7} {a.Name,-19} {f.Origin,-19} {f.Destination,-19} {f.ExpectedTime:HH:mm}");
}

void DisplayBoardingGates(Terminal terminal)
{

}

//=======================================
// Data manipulation (feature 5, 6 & 8)
//=======================================

void AssignBoardingGateToFlight(Terminal terminal)
{
    var flight = InputFlight(terminal);
    Console.WriteLine();
    DisplayFlightInfoWithSRC(terminal, flight);
    Console.WriteLine();

    var gate = InputAvailableBoardingGate(terminal);
    gate.Flight = flight;

    Console.WriteLine();
    DisplayFlightInfoWithSRC(terminal, flight);
    Console.WriteLine($"Boarding gate entered..: {gate.GateName}");

    Console.Write("\nUpdate flight status [Y/N]? ");
    if (Console.ReadLine()?.ToUpper() == "Y")
    {
        flight.Status = InputFlightStatus();
        Console.WriteLine();
    }

    Console.WriteLine($"Gate {gate.GateName} assigned to {flight.FlightNumber} successfully");
}

//==================
// Input functions
//==================

Flight InputFlight(Terminal terminal)
{
    while (true)
    {
        Console.Write("Enter flight number: ");
        var flightNo = Console.ReadLine() ?? "";

        if (terminal.Flights.TryGetValue(flightNo, out var f)) return f;
        Console.WriteLine("Flight not found; please try again.");
    }
}

BoardingGate InputAvailableBoardingGate(Terminal terminal)
{
    while (true)
    {
        Console.Write("Enter boarding gate: ");
        var gateName = Console.ReadLine() ?? "";

        if (terminal.BoardingGates.TryGetValue(gateName, out var g))
        {
            if (g.Flight == null) return g;
            Console.WriteLine("Gate already occupied; please try again.");
            continue;
        }

        Console.WriteLine("Gate not found; please try again.");
    }
}

string InputFlightStatus()
{
    Console.WriteLine("[D]elayed");
    Console.WriteLine("[B]oarding");
    Console.WriteLine("[O]n time");

    while (true)
    {
        Console.Write("Enter status: ");
        switch (Console.ReadLine()?.ToUpper() ?? "")
        {
            case "D": return "Delayed";
            case "B": return "Boarding";
            case "O": return "On time";
            default:
                Console.WriteLine("Invalid status; please try again.");
                break;
        }
    }
}

//===================
// Helper functions
//===================

string? GetSpecialRequestCode(Flight flight)
{
    return flight switch
    {
        LWTTFlight => "LWTT",
        DDJBFlight => "DDJB",
        CFFTFlight => "CFFT",
        _ => null
    };
}

void DisplayFlightInfoWithSRC(Terminal terminal, Flight flight)
{
    Console.WriteLine($"Flight number..........: {flight.FlightNumber}");
    Console.WriteLine($"Airline name...........: {terminal.GetAirlineFromFlight(flight).Name}");
    Console.WriteLine($"Origin.................: {flight.Origin}");
    Console.WriteLine($"Destination............: {flight.Destination}");

    if (flight.Origin == "Singapore (SIN)") Console.WriteLine($"Expected departure time: {flight.ExpectedTime:HH:mm}");
    else Console.WriteLine($"Expected arrival time..: {flight.ExpectedTime:HH:mm}");

    Console.WriteLine($"Special request code...: {GetSpecialRequestCode(flight) ?? "-"}");
}

void DisplayMenu()
{
    Console.WriteLine("------------------------- MENU -----------------------");
    Console.WriteLine("1) List all flights with their basic information");
    Console.WriteLine("2) List all boarding gates");
    Console.WriteLine("3) Assign a boarding gate to a flight");
    Console.WriteLine("4) Create a new flight");
    Console.WriteLine("5) Display full flight details from an airline");
    Console.WriteLine("6) Modify flight details");
    Console.WriteLine("7) Display scheduled flights in chronological order,");
    Console.WriteLine("   with boarding gates assignments where applicable");
    Console.WriteLine("8) Exit");
    Console.WriteLine("------------------------------------------------------");
}