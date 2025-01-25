using System;
using System.Reflection.Metadata.Ecma335;

namespace Assignment
{
    public class Terminal
    {
        public string TerminalName { get; set; }
        public Dictionary<string, Airline> Airlines { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }
        public Dictionary<string, BoardingGate> BoardingGates { get; set; }
        public Dictionary<string, double> GateFees { get; set; }

        public Terminal(string terminalname)
        {
            TerminalName = terminalname;
        }

        public bool AddAirline(Airline airline)
        {
            if (Airlines.ContainsKey(airline.Name))
            {
                return false;
            }
            else 
            { 
                Airlines.Add(airline.Name,airline);
                return true;
            }
        }

        public bool AddBoardingGate(BoardingGate boardingGate)
        {
            if (BoardingGates.ContainsKey(boardingGate.GateName))
            {
                return false;
            }
            else 
            { 
                BoardingGates.Add(boardingGate.GateName, boardingGate);
                return true;
            }
        }

        public Airline GetAirlineFromFlight(Flight flight) 
        {
            Dictionary<string, string> AirlineDic = new Dictionary<string, string>();
            string[] file = File.ReadAllLines("airlines.csv");
            foreach (string line in file)
            {
                AirlineDic[line.Split(",")[0]] = line.Split(",")[1];
            }
            string Airline_Prefix = flight.FlightNumber.Substring(0,2);
            foreach (string prefix in AirlineDic.Keys)
            {
                if (Airline_Prefix == AirlineDic[prefix])
                {
                    string AirlineName = prefix;
                    foreach (Airline airline in Airlines.Values)
                    {
                        if (airline.Name == prefix)
                        {
                            return airline;
                        }
                    }
            
                }
            }
            return null;
        }

        public void PrintAirlineFees() { }


        public override string ToString() 
        {
            return TerminalName;
        }
    }
}