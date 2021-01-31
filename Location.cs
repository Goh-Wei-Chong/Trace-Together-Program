//============================================================
// Student Number : S10204647, S10205678
// Student Name : Goh Wei Chong, Swan Htet Aung
// Module Group : T02
//============================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace test
{
    class BusinessLocation
    {
        public string BusinessName { get; set; }
        public string BranchCode { get; set; }
        public int MaximumCapacity { get; set; }
        public int VisitorsNow { get; set; }

        public BusinessLocation(string bn, string bc, int mc)
        {
            BusinessName = bn;
            BranchCode = bc;
            MaximumCapacity = mc;
        }

        public bool IsFull()
        {
            if (VisitorsNow >= MaximumCapacity)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"BusinessName: {BusinessName}   \t" +
                $"BranchCode: {BranchCode}  \t" +
                $"MaximumCapacity: {MaximumCapacity}   \t" +
                $"VisitorsNow: {VisitorsNow}  ";
        }
    }

    class SHNFacility
    {
        public string FacilityName { get; set; }
        public int FacilityCapacity { get; set; }
        public int FacilityVacancy { get; set; }
        public double DistFromAirCheckpoint { get; set; }
        public double DistFromSeaCheckpoint { get; set; }
        public double DistFromLandCheckpoint { get; set; }

        public SHNFacility() { }

        public SHNFacility(string n, int c, double a, double s, double l)
        {
            FacilityName = n;
            FacilityCapacity = c;
            DistFromAirCheckpoint = a;
            DistFromSeaCheckpoint = s;
            DistFromLandCheckpoint = l;
        }

        public double CalculateTravelCost(string s, DateTime date)
        {
            double base_fare = 0;
            if (s.ToLower() == "land")
            {
                base_fare = 50 + DistFromLandCheckpoint * 0.22;
            }
            else if (s.ToLower() == "air")
            {
                base_fare = 50 + DistFromAirCheckpoint * 0.22;
            }
            else if (s.ToLower() == "sea")
            {
                base_fare = 50 + DistFromSeaCheckpoint * 0.22;
            }

            TimeSpan time = date.TimeOfDay;
            
            if (time >= new TimeSpan(6, 0, 0) && time <= new TimeSpan(8, 59, 59))
            {
                return base_fare * 1.25;
            }
            else if (time >= new TimeSpan(18, 0, 0) && time <= new TimeSpan(23, 59, 59))
            {
                return base_fare * 1.25;
            }
            else if (time >= new TimeSpan(0, 0, 0) && time <= new TimeSpan(5, 59, 59))
            {
                return base_fare * 1.5;
            }
            else
            {
                return base_fare;
            }
        }

        public bool IsAvailable()
        {
            if (FacilityVacancy == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public override string ToString()
        {
            return "Facility Name: " + FacilityName +
                "\t" + "Facility Capacity: " +
                FacilityCapacity + "\t" + "DistFromAirCheckpoint: "
                + DistFromAirCheckpoint + "\t" + "DistFromSeaCheckpoint: "
                + DistFromSeaCheckpoint + "\t" + "DistFromLandCheckpoint: "
                + DistFromLandCheckpoint;
        }
    }
}
