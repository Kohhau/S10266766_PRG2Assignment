using Assignment;
void CreateAirlines(Terminal terminal)
{
    string[] file =  File.ReadAllLines("airline.csv");
    for (int i = 1; i < file.Length; i++)
    {
        terminal.Airlines.Add(file[i].Split(",")[0], new Airline(file[i].Split(",")[0], file[i].Split(",")[1]));
    }
}

void CreateBoardingGates(Terminal terminal)
{
    string[] file = File.ReadAllLines("boardinggates.csv");
    for (int i = 1; i < file.Length; i++)
    {
        terminal.AddBoardingGate(new BoardingGate(file[i].Split(",")[0], Convert.ToBoolean(file[i].Split(",")[1]), Convert.ToBoolean(file[i].Split(",")[2]), Convert.ToBoolean(file[i].Split(",")[3])));
    }
}
Terminal terminal5 = new Terminal("Changi Airport Terminal 5");

CreateAirlines(terminal5);
CreateBoardingGates(terminal5);
