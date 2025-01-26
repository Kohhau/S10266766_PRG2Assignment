namespace Assignment;

public class BoardingGate
{
    public string GateName { get; set; }
    public bool SuppportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT { get; set; }
    public Flight Flight { get; set; }

    public BoardingGate(string gateName, bool supportsDDJB, bool suppportsCFFT, bool supportsLWTT)
    {
        GateName = gateName;
        SuppportsCFFT = suppportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }

    public BoardingGate(Flight flight)
    {
        Flight = flight;
    }

    public double CalculateFees()
    {
        return 0;  // TODO
    }

    public override string ToString()
    {
        return $"GateName: {GateName}\nSupports CFFT/DDJB/LWTT: {SuppportsCFFT}{SupportsDDJB}{SupportsLWTT}\nFlight: {Flight}";
    }
}