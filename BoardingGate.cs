//=============================
// Student Number: S10266766
// Student Name: Koh Hau
// Partner Name: Haziq Hairil 
//=============================

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

    public double CalculateFees()
    {
        // $300 base fee for all boarding gates
        return 300;
    }

    public override string ToString()
    {
        return $"GateName: {GateName}\nSupports CFFT/DDJB/LWTT: {SuppportsCFFT}/{SupportsDDJB}/{SupportsLWTT}\nFlight: {Flight}";
    }
}