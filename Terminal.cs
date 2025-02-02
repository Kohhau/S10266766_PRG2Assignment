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
            .ToDictionary(kv => kv.Value.Flight!.FlightNumber, kv => kv.Value);

        // Check that all flights have been assigned boarding gates
        if (Flights.Values.Any(f => !flightGates.ContainsKey(f.FlightNumber)))
        {
            Console.WriteLine("Error: All flights must have assigned boarding gates before running this feature");
            return;
        }

        var aggFeeSubtotal = 0.0;
        var aggDiscountSubtotal = 0.0;

        // Display the table
        Console.WriteLine("Airline             Fee subtotal  Discount subtotal  Total fee");
        foreach (var a in Airlines.Values)
        {
            var feeSubtotal = a.CalculateFees();  // Accumulate fees for arrival, departure, and special request codes
            var discountSubtotal = 0.0;

            // Accumulate per-flight fees and discounts
            foreach (var f in a.Flights.Values)
            {
                // Accumulate fees for boarding gates
                feeSubtotal += flightGates[f.FlightNumber].CalculateFees();

                // $110 discount for each flight arriving/departing before 11am or after 9pm
                if (f.ExpectedTime.TimeOfDay > new TimeSpan(21, 0, 0) || f.ExpectedTime.TimeOfDay < new TimeSpan(11, 0, 0))
                    discountSubtotal += 110;

                // $25 discount for each flight with the Origin of Dubai (DXB), Bangkok (BKK) or Tokyo (NRT)
                if (f.Origin == "Dubai (DXB)" || f.Origin == "Bangkok (BKK)" || f.Origin == "Tokyo (NRT)")
                    discountSubtotal += 25;

                // $50 discount for each flight not indicating any Special Request Codes
                if (f is NORMFlight) discountSubtotal += 50;
            }

            // 3% discount for more than 5 flights arriving/departing.
            // Note that this has to be applied after `fee` is fully calculated.
            if (a.Flights.Count > 5) discountSubtotal += feeSubtotal * 0.03;

            // $350 discount for every 3 flights arriving/departing
            discountSubtotal += (a.Flights.Count / 3) * 350.00;

            var totalFee = feeSubtotal - discountSubtotal;
            Console.WriteLine($"{a.Name,-19}    ${feeSubtotal,8:0.00}          ${discountSubtotal,8:0.00}  ${totalFee,8:0.00}");

            aggFeeSubtotal += feeSubtotal;
            aggDiscountSubtotal += discountSubtotal;
        }

        var aggTotalFee = aggFeeSubtotal - aggDiscountSubtotal;

        // Dsiplay details of aggregated fees and discounts
        Console.WriteLine();    
        Console.WriteLine($"{"Subtotal of all airline fees:",51} ${aggFeeSubtotal,9:0.00}");
        Console.WriteLine($"{"Subtotal of all airline discounts:",51} ${aggDiscountSubtotal,9:0.00}");
        Console.WriteLine($"{"Final total of all airline fees:",51} ${aggTotalFee,9:0.00}");
        Console.WriteLine($"{"Total discount rate:",51} {aggDiscountSubtotal / aggTotalFee * 100,9:0.0}%");
    }


    public override string ToString()
    {
        return TerminalName;
    }
}