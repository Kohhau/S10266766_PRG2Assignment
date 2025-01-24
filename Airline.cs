using System;

namespace Assignment
{
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber)){
                return false;
            }
            else
            {
                Flights.Add(flight.FlightNumber,flight);
                return true;
            }
        }
        public bool RemoveFlight(Flight flight) 
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                Flights.Remove(flight.FlightNumber);
                return true;
            }
            else
            {
                return false;
            }
        }

        public double CalculateFees()
        {
            double fee = 0;
            foreach (Flight flight in Flights.Values)
            {
                fee += flight.CalculateFees();
            }
            if (Flights.Count >= 5)
            {
                fee *= 0.97;
            }
            foreach (Flight flight in Flights.Values)
            {
                if (flight.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0) || flight.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0))
                {
                    fee -= 110;
                }
                if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                {
                    fee -= 25.00;
                }
                if (flight is NORMFlight) { fee -= 50.00; }
            }
            fee -= (Flights.Count / 3) * 350.00;
            return fee;

        }
        public override string ToString()
        {
            return "Name: " + Name + "\nCode: " + Code;
        }

    }
}