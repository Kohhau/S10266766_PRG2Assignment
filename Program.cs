using Assignment;

void CreateAirlines(Terminal terminal)
{
    using var reader = new StreamReader("airlines.csv");
    var line = reader.ReadLine();  // Skip header

    while ((line = reader.ReadLine()) != null)
    {
        var values = line.Split(",");
        terminal.AddAirline(new(values[0], values[1]));
    }
}

void CreateBoardingGates(Terminal terminal)
{
    using var reader = new StreamReader("boardinggates.csv");
    var line = reader.ReadLine();  // Skip header

    while ((line = reader.ReadLine()) != null)
    {
        var values = line.Split(",");
        terminal.AddBoardingGate(new(values[0], Convert.ToBoolean(values[1]), Convert.ToBoolean(values[2]), Convert.ToBoolean(values[3])));
    }
}

void CreateFlights(Terminal terminal)
{
    string[] file = File.ReadAllLines("flights.csv");
    for (int i = 1; i < file.Length; i++)
    {
    }
}
Terminal terminal5 = new Terminal("Changi Airport Terminal 5");

CreateAirlines(terminal5);
CreateBoardingGates(terminal5);
