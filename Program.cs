//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================
using Assignment;

Terminal terminal5 = new Terminal("Changi Airport Terminal 5");
LoadAirlines(terminal5);
LoadBoardingGates(terminal5);
LoadFlights(terminal5);

DisplayBasicInformation(terminal5);

/**********************************************/
/*                                            */
/*   Loading data from CSVs (feature 1 & 2)   */
/*                                            */
/**********************************************/

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

/**********************************************/
/*                                            */
/*   Displaying information (feature 3 & 4)   */
/*                                            */
/**********************************************/

void DisplayBasicInformation(Terminal terminal)
{
    //                         Singapore Airlines 
    Console.WriteLine("Flight  Airline name        Origin              Destination         Expected departure/arrival time");

    foreach (var a in terminal.Airlines.Values)
        foreach (var f in a.Flights.Values)
            Console.WriteLine($"{f.FlightNumber,-7} {a.Name,-19} {f.Origin,-19} {f.Destination,-19} {f.ExpectedTime.ToString("HH:mm")}");
}
