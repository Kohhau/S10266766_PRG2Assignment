namespace Assignment;

public abstract class Flight
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime ExpectedTime { get; set; }
    public string Status { get; set; }
    protected readonly double baseFee = 300.00;

    protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
        Status = "On Time";
    }

    protected Flight(
        string flightNumber,
        string origin,
        string destination,
        DateTime expectedTime,
        string status
    ) : this(flightNumber, origin, destination, expectedTime)
    {
        Status = status;
    }

    public virtual double CalculateFees()
    {
        // $500 fee for arriving flights, $800 fee for departing flights
        return baseFee + Origin == "Singapore (SIN)" ? 800.00 : 500.00;
    }

    public override string ToString()
    {
        return $"Flight No. : {FlightNumber}\nOrigin: {Origin}\nDestination: {Destination}\nExpected time of arrival: {ExpectedTime}\nStatus: {Status}";
    }
}

public class NORMFlight : Flight
{
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }

    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status) { }
}

public class LWTTFlight : Flight
{
    private readonly double requestFee = 500.00;

    public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }

    public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status) { }

    public override double CalculateFees()
    {
        return base.CalculateFees() + requestFee;
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nRequest Fee: {requestFee}";
    }
}

public class CFFTFlight : Flight
{
    private readonly double requestFee = 150.00;

    public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }

    public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status) { }

    public override double CalculateFees()
    {
        return base.CalculateFees() + requestFee;
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nRequest Fee: {requestFee}";
    }
}

public class DDJBFlight : Flight
{
    private readonly double requestFee = 300.00;

    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime) { }

    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        : base(flightNumber, origin, destination, expectedTime, status) { }

    public override double CalculateFees()
    {
        return base.CalculateFees() + requestFee;
    }

    public override string ToString()
    {
        return $"{base.ToString()}\nRequest Fee: {requestFee}";
    }
}