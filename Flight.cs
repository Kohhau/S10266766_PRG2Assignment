using System;

namespace Assignment
{
    public abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
        }

        protected Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }
        public double CalculateFees()
        {
            double fee;
            if (Origin == "Singapore (SIN)")
            {
                fee = 800.00;
            }
            else
            {
                fee = 500.00;
            }
            return fee;
        }
        public override string ToString()
        {
            return "Flight No. : " + FlightNumber + "\nOrigin: " + Origin + "\nDestination: " + Destination + "\nExpected time of arrival: " + ExpectedTime + "\nStatus: " + Status;
        }
    }

    public class NORMFlight : Flight
    {
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
        }

        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        public double CalculateFees()
        {
            double fee;
            if (Origin == "Singapore (SIN)")
            {
                fee = 800.00;
            }
            else
            {
                fee = 500.00;
            }
            return fee;
        }
    }

    public class LWTTFlight : Flight
    {
        private double requestFee;
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin,destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
            requestFee = 500.00;
        }

        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin,destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
            requestFee = 500.00;
        }
        public double CalculateFees()
        {
            double fee;
            if (Origin == "Singapore (SIN)")
            {
                fee = 800.00 + requestFee;
            }
            else
            {
                fee = 500.00 + requestFee;
            }
            return fee;
        }

        public override string ToString()
        {
            return base.ToString() + "\nRequest Fee: " + requestFee;
        }
    }

    public class CFFTFlight : Flight
    {
        private double requestFee;
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin,destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
            requestFee = 150.00;
        }

        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
            requestFee = 150.00;
        }
        public double CalculateFees()
        {
            double fee;
            if (Origin == "Singapore (SIN)")
            {
                fee = 800.00+ requestFee;
            }
            else
            {
                fee = 500.00 + requestFee;
            }
            return fee;
        }

        public override string ToString()
        {
            return base.ToString() + "\nRequest Fee: " + requestFee;
        }
    }
    public class DDJBFlight : Flight
    {
        private double requestFee;
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination,expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = "On Time";
            requestFee = 300.00;
        }

        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination,expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
            requestFee = 300.00;
        }

        public double CalculateFees()
        {
            double fee;
            if (Origin == "Singapore (SIN)")
            {
                fee = 800.00 + requestFee;
            }
            else
            {
                fee = 500.00 + requestFee;
            }
            return fee;
        }

        public override string ToString()
        {
            return base.ToString() + "\nRequest Fee: " + requestFee;
        }
    }
}