//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================

using Assignment;
using System.Text.RegularExpressions;

Terminal terminal5 = new("Changi Airport Terminal 5");
LoadAirlines(terminal5);
LoadBoardingGates(terminal5);
LoadFlights(terminal5);

var firstRun = true;
while (true)
{
    if (!firstRun) Console.WriteLine();
    firstRun = false;

    PrintMenu();
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
            CreateNewFlight(terminal5);
            break;
        case "5":
            Console.WriteLine();
            DisplayFullFlightDetails(terminal5);
            break;
        case "6":
            Console.WriteLine();
            ModifyFlightDetails(terminal5);
            break;
        case "7":
            Console.WriteLine();
            DisplayScheduledFlights(terminal5);
            break;
        case "8":
            Console.WriteLine();
            ProcessAllUnassignedFlights(terminal5);
            break;
        case "9":
            Console.WriteLine();
            terminal5.PrintAirlineFees();
            break;
        case "0":
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

//==================================================
// Displaying information (feature 3, 4, 7, 9 & b)
//==================================================
void DisplayBasicInformation(Terminal terminal)
{
    Console.WriteLine($"{"Flight",-7} {"Airline",-19} {"Origin",-19} {"Destination",-19} Time");

    foreach (var a in terminal.Airlines.Values)
        foreach (var f in a.Flights.Values)
            Console.WriteLine($"{f.FlightNumber,-7} {a.Name,-19} {f.Origin,-19} {f.Destination,-19} {f.ExpectedTime:HH:mm}");
}

void DisplayBoardingGates(Terminal terminal)
{
    Console.WriteLine("Gate  Supported SRCs    Flight");
    foreach (var g in terminal.BoardingGates.Values)
    {
        var supportedSRCs = new List<string>();
        if (g.SupportsDDJB) supportedSRCs.Add("DDJB");
        if (g.SuppportsCFFT) supportedSRCs.Add("CFFT");
        if (g.SupportsLWTT) supportedSRCs.Add("LWTT");
        Console.WriteLine($"{g.GateName,-6}{string.Join(", ", supportedSRCs),-18}{g.Flight?.FlightNumber ?? ""}");
    }
}

void DisplayFullFlightDetails(Terminal terminal)
{
    var flight = ChooseFlight(terminal);
    Console.WriteLine();
    PrintFullFlightInfo(terminal, flight);
}

void DisplayScheduledFlights(Terminal terminal)
{
    var flights = terminal.Flights.Values.ToList().Order();

    Console.WriteLine($"{"Flight",-7} {"Airline",-19} {"Origin",-19} {"Destination",-19} {"Time",-6} {"Status",-9} {"SR code",-8} Gate");
    foreach (var f in flights)
    {
        Console.Write($"{f.FlightNumber,-7} {terminal.GetAirlineFromFlight(f).Name,-19} {f.Origin,-19} {f.Destination,-19} {f.ExpectedTime,-6:HH:mm} ");
        Console.WriteLine($"{f.Status,-9} {GetSpecialRequestCode(f) ?? "",-8} {GetBoardingGateForFlight(terminal, f)?.GateName ?? ""}");
    }
}

//=======================================
// Data manipulation (feature 5, 6, 8)
//=======================================

void AssignBoardingGateToFlight(Terminal terminal)
{
    // Input a flight and display its specifications
    var flight = InputExistingFlightNumber(terminal);
    Console.WriteLine();
    PrintBasicFlightInfoWithSRC(terminal, flight);
    Console.WriteLine();

    // Update the flight's boarding gate.
    // "For Basic Features, there is no need to validate if the Special Request Codes between Flights and Boarding Gates match".
    var newGate = InputAvailableBoardingGate(terminal);
    var oldGate = GetBoardingGateForFlight(terminal, flight);
    if (oldGate != null) oldGate.Flight = null;
    newGate.Flight = flight;

    Console.WriteLine();
    PrintFullFlightInfo(terminal, flight);

    Console.Write("\nUpdate flight status [Y/N]? ");
    if (Console.ReadLine()?.ToUpper() == "Y")
    {
        flight.Status = InputFlightStatus();
        Console.WriteLine();
    }

    Console.WriteLine($"Gate {newGate.GateName} assigned to {flight.FlightNumber} successfully");
}

void CreateNewFlight(Terminal terminal)
{
    int numFlightsAdded = 0;
    string continueChoice;

    do
    {
        // Input the flight specifications
        var flightNo = InputNewFlightNumber(terminal);
        var (origin, destination) = InputOriginAndDestination();
        var expectedTime = origin == "Singapore (SIN)" ? InputTime("departure") : InputTime("arrival");
        var specialRequestCode = InputSpecialRequestCode();
        // TODO: Ask for status / boarding gate?

        // Create the flight
        Flight f = specialRequestCode switch
        {
            "LWTT" => new LWTTFlight(flightNo, origin, destination, expectedTime),
            "DDJB" => new DDJBFlight(flightNo, origin, destination, expectedTime),
            "CFFT" => new CFFTFlight(flightNo, origin, destination, expectedTime),
            _ => new NORMFlight(flightNo, origin, destination, expectedTime)
        };
        terminal.GetAirlineFromFlight(f).AddFlight(f);
        numFlightsAdded++;

        // Append the new flight information to flights.csv
        using (var writer = new StreamWriter("flights.csv", append: true))
        {
            writer.WriteLine($"{f.FlightNumber},{f.Origin},{f.Destination},{f.ExpectedTime:hh:mm tt},{GetSpecialRequestCode(f) ?? ""}");
        }

        Console.Write("Add another flight [Y/N]? ");
        continueChoice = Console.ReadLine()?.ToUpper() ?? "";
        Console.WriteLine();
    } while (continueChoice == "Y");

    if (numFlightsAdded == 1) Console.WriteLine("Flight addded successfully");
    else Console.WriteLine($"{numFlightsAdded} flights added successfully");
}

void ModifyFlightDetails(Terminal terminal)
{
    var flight = ChooseFlight(terminal);
    Console.WriteLine("\n[1] Modify Existing Flight\n[2] Delete Existing Flights");

    while (true)
    {
        Console.WriteLine();
        Console.Write("Enter choice: ");
        switch (Console.ReadLine())
        {
            case "1":
                Console.WriteLine();
                ModifyExistingFlight(terminal, flight);
                Console.WriteLine();
                PrintFullFlightInfo(terminal, flight);
                return;
            case "2":
                Console.WriteLine();
                if (DeleteFlightWithConfirmation(terminal, flight))
                {
                    Console.WriteLine();
                    DisplayScheduledFlights(terminal);
                }
                return;
            default:
                Console.WriteLine("Invalid choice; please try again.");
                break;
        }
    }
}

void ProcessAllUnassignedFlights(Terminal terminal)
{
    var gatelessFlights = QueueGatelessFlights(terminal);
    Console.WriteLine($"Found {gatelessFlights.Count} flights without a boarding gate.");
    if (gatelessFlights.Count == 0) return;

    var numFlightsAlreadyAssigned = terminal.Flights.Count - gatelessFlights.Count;
    var numFlightlessGates = terminal.BoardingGates.Values.Select(g => g.Flight == null).Count();
    var numAssigned = 0;

    Console.WriteLine($"Found {numFlightlessGates} boarding gates without a flight.");
    Console.WriteLine($"\n{"Flight",-7} {"Airline",-19} {"Origin",-19} {"Destination",-19} {"Time",-6} {"SR code",-8} Gate");

    while (gatelessFlights.Count > 0)
    {
        var flight = gatelessFlights.Dequeue();
        var (assigned, assignedGate) = AutoAssignFlight(terminal, flight);
        if (assigned) numAssigned++;

        Console.Write($"{flight.FlightNumber,-7} {terminal.GetAirlineFromFlight(flight).Name,-19} {flight.Origin,-19} {flight.Destination,-19}");
        Console.WriteLine($" {flight.ExpectedTime,-6:HH:mm} {GetSpecialRequestCode(flight),-8} {assignedGate?.GateName ?? ""}");
    }

    Console.WriteLine($"\nAssigned {numAssigned} flights to boarding gates.");
    if (numFlightsAlreadyAssigned != 0)
        Console.WriteLine($"(# of flights assigned automatically / # of flights previously assigned) = {(numAssigned * 100.0 / numFlightsAlreadyAssigned):f2}%");
    else
        Console.WriteLine($"(# of flights assigned automatically : # of flights previously assigned) = {numAssigned} : 0");
}

//==================
// Input functions
//==================

Flight InputExistingFlightNumber(Terminal terminal)
{
    while (true)
    {
        Console.Write("Enter flight number: ");
        var flightNo = Console.ReadLine()?.ToUpper() ?? "";

        if (terminal.Flights.TryGetValue(flightNo, out var f)) return f;
        Console.WriteLine("Flight not found; please try again.");
    }
}

Flight InputExistingAirlineFlightNumber(Airline airline)
{
    while (true)
    {
        Console.Write("Enter flight number: ");
        var flightNo = Console.ReadLine()?.ToUpper() ?? "";

        if (airline.Flights.TryGetValue(flightNo, out var f)) return f;
        Console.WriteLine($"Flight not found for {airline.Code}; please try again.");
    }
}

Flight ChooseFlight(Terminal terminal)
{
    PrintAirlines(terminal);

    Console.WriteLine();
    var airline = terminal.Airlines[InputAirlineCode(terminal)];
    var flights = airline.Flights.Values;

    Console.WriteLine($"\n{"Flight",-7} {"Airline",-19} {"Origin",-19} Destination");
    foreach (var f in flights)
    {
        Console.WriteLine($"{f.FlightNumber,-7} {airline.Name,-19} {f.Origin,-19} {f.Destination,-19}");
    }

    Console.WriteLine();
    return InputExistingAirlineFlightNumber(airline);
}

BoardingGate InputAvailableBoardingGate(Terminal terminal)
{
    while (true)
    {
        Console.Write("Enter boarding gate: ");
        var gateName = Console.ReadLine()?.ToUpper() ?? "";

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

string InputNewFlightNumber(Terminal terminal)
{
    while (true)
    {
        Console.Write("Enter flight number: ");
        var flightNo = Console.ReadLine()?.ToUpper() ?? "";

        if (!Regex.IsMatch(flightNo, @"^[A-Z]{2}\s\d{3}$"))
        {
            Console.WriteLine("Invalid flight number; please try again. Use format \"AZ 123\".");
            continue;
        }

        if (!terminal.Airlines.ContainsKey(flightNo[..2]))
        {
            Console.WriteLine("Invalid airline code; please try again.");
            continue;
        }

        if (!terminal.Flights.ContainsKey(flightNo)) return flightNo;
        Console.WriteLine("Flight number already exists; please try again.");
    }
}

(string, string) InputOriginAndDestination()
{
    while (true)
    {
        Console.Write("Enter origin: ");
        var origin = Console.ReadLine() ?? "";
        Console.Write("Enter destination: ");
        var destination = Console.ReadLine() ?? "";

        if ((origin == "Singapore (SIN)" || destination == "Singapore (SIN)") && origin != destination)
            return (origin, destination);

        Console.WriteLine("Either origin or destination must be \"Singapore (SIN)\"; please try again.");
    }
}

DateTime InputTime(string typeText)
{
    while (true)
    {
        Console.Write($"Enter {typeText} time: ");
        try { return Convert.ToDateTime(Console.ReadLine()); }
        catch (FormatException) { Console.WriteLine("Invalid time; please try again."); }
    }
}

string? InputSpecialRequestCode()
{
    while (true)
    {
        Console.Write("Enter special request code, or leave blank for none: ");
        var code = Console.ReadLine() ?? "";

        if (code == "LWTT" || code == "DDJB" || code == "CFFT") return code;
        if (code == "") return null;

        Console.WriteLine("Invalid code; please try again.");
    }

}

string InputAirlineCode(Terminal terminal)
{
    string code = "";
    while (true)
    {
        Console.Write("Enter airline code: ");
        code = Console.ReadLine()?.ToUpper() ?? "";
        if (terminal.Airlines.ContainsKey(code)) return code;

        Console.WriteLine("Airline code not found; please try again.");
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

BoardingGate? GetBoardingGateForFlight(Terminal terminal, Flight flight)
{
    return terminal.BoardingGates.Values.FirstOrDefault(g => g.Flight == flight);
}

void PrintBasicFlightInfoWithSRC(Terminal terminal, Flight flight)
{
    Console.WriteLine($"Flight number..........: {flight.FlightNumber}");
    Console.WriteLine($"Airline................: {terminal.GetAirlineFromFlight(flight).Name}");
    Console.WriteLine($"Origin.................: {flight.Origin}");
    Console.WriteLine($"Destination............: {flight.Destination}");

    if (flight.Origin == "Singapore (SIN)") Console.WriteLine($"Expected departure time: {flight.ExpectedTime:HH:mm}");
    else Console.WriteLine($"Expected arrival time..: {flight.ExpectedTime:HH:mm}");

    Console.WriteLine($"Special request code...: {GetSpecialRequestCode(flight) ?? "-"}");
}

void PrintFullFlightInfo(Terminal terminal, Flight flight)
{
    PrintBasicFlightInfoWithSRC(terminal, flight);
    Console.WriteLine($"Status.................: {flight.Status}");
    Console.WriteLine($"Boarding gate..........: {GetBoardingGateForFlight(terminal, flight)?.GateName ?? "-"}");
}

void PrintMenu()
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
    Console.WriteLine("8) Process all unassigned flights to boarding gates");
    Console.WriteLine("9) Display the total fee per airline for the day");
    Console.WriteLine("0) Exit");
    Console.WriteLine("------------------------------------------------------");
}

void PrintAirlines(Terminal terminal)
{
    Console.WriteLine("Code  Airline");
    foreach (var airline in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airline.Code,-6}{airline.Name,-25}");
    }
}

void ModifyExistingFlight(Terminal terminal, Flight flight)
{
    while (true)
    {
        // Prompt the user to select the flight specification to modify
        var option = "";
        while (true)
        {
            Console.WriteLine("Select a flight specification to modify");
            Console.WriteLine("[1] Origin and Destination");
            Console.WriteLine("[2] Expected Departure/Arrival Time");
            Console.WriteLine("[3] Status");
            Console.WriteLine("[4] Special Request Code");
            Console.WriteLine("[5] Boarding Gate");
            Console.Write("\nEnter your choice: ");
            option = Console.ReadLine();

            if (option == "1" || option == "2" || option == "3" || option == "4" || option == "5")
                break;

            Console.WriteLine("Invalid choice; please try again.");
        }

        // Update the selected flight specification
        Console.WriteLine();
        switch (option)
        {
            case "1":
                var (origin, destination) = InputOriginAndDestination();
                flight.Origin = origin;
                flight.Destination = destination;
                break;
            case "2":
                flight.ExpectedTime = flight.Origin == "Singapore (SIN)" ? InputTime("departure") : InputTime("arrival");
                break;
            case "3":
                flight.Status = InputFlightStatus();
                break;
            case "4":
                // "For Basic Features, there is no need to validate if the Special Request Codes between Flights and Boarding Gates match"
                flight = ChangeFlightSRCode(terminal, flight, InputSpecialRequestCode());
                break;
            case "5":
                // "For Basic Features, there is no need to validate if the Special Request Codes between Flights and Boarding Gates match"
                var oldGate = GetBoardingGateForFlight(terminal, flight);
                var gate = InputAvailableBoardingGate(terminal);
                gate.Flight = flight;

                // Update the Flight reference in the previous Gate
                if (oldGate != null) oldGate.Flight = null;
                break;
        }

        Console.Write("\nContinue modifications? [N] to quit: ");
        if (Console.ReadLine()?.ToUpper() == "N") break;
    }
}

bool DeleteFlightWithConfirmation(Terminal terminal, Flight flight)
{
    while (true)
    {
        Console.Write("Are you sure? [Y/N]: ");
        var confirmation = Console.ReadLine()?.ToUpper() ?? "";

        if (confirmation == "Y")
        {
            terminal.GetAirlineFromFlight(flight).RemoveFlight(flight);
            var gate = GetBoardingGateForFlight(terminal, flight);
            if (gate != null) gate.Flight = null;
            return true;
        }
        else if (confirmation == "N")
        {
            Console.WriteLine("Deletion cancelled.");
            return false;
        }

        Console.WriteLine("Invalid choice; please try again.");
    }
}

Flight ChangeFlightSRCode(Terminal terminal, Flight flight, string? newSRCode)
{
    // Construct the new Flight object
    Flight newFlight = newSRCode switch
    {
        "LWTT" => new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status),
        "DDJB" => new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status),
        "CFFT" => new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status),
        _ => new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, flight.ExpectedTime, flight.Status)
    };

    // Update the Flight reference in the corresponding Airline
    var airline = terminal.GetAirlineFromFlight(flight);
    airline.Flights[flight.FlightNumber] = newFlight;

    // Update the Flight reference in the corresponding BoardingGate
    var gate = GetBoardingGateForFlight(terminal, flight);
    if (gate != null) gate.Flight = newFlight;

    return newFlight;
}

Queue<Flight> QueueGatelessFlights(Terminal terminal)
{
    var gatelessFlights = new Queue<Flight>();

    foreach (var flight in terminal.Flights.Values)
    {
        var gate = GetBoardingGateForFlight(terminal, flight);
        if (gate == null) gatelessFlights.Enqueue(flight);
    }

    return gatelessFlights;
}

(bool, BoardingGate?) AutoAssignFlight(Terminal terminal, Flight flight)
{
    // Iterate over all the boarding gates to find an available gate that supports the flight's SR code
    foreach (var gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == null && (
            flight is DDJBFlight && gate.SupportsDDJB
            || flight is CFFTFlight && gate.SuppportsCFFT
            || flight is LWTTFlight && gate.SupportsLWTT
            || flight is NORMFlight && !gate.SupportsDDJB && !gate.SuppportsCFFT && !gate.SupportsLWTT
        ))
        {
            gate.Flight = flight;
            return (true, gate);
        }
    }

    if (flight is not NORMFlight)
    {
        Console.WriteLine($"No boarding gates available for flight {flight.FlightNumber}");
        return (false, null);
    }

    // The question states that NORMFlights should only be assigned to gates don't support any SR codes.
    // The following is an additional step because we deemed it illogical for empty gates to exist
    // while having NORMFlights be unassigned.
    foreach (var gate in terminal.BoardingGates.Values)
    {
        if (gate.Flight == null)
        {
            gate.Flight = flight;
            return (true, gate);
        }
    }

    Console.WriteLine($"No boarding gates available for flight {flight.FlightNumber}");
    return (false, null);

}