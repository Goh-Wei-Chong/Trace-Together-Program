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
    class TravelEntry
    {
        public string LastCountryOfEmbarkation { get; set; }
        public string EntryMode { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ShnEndDate { get; set; }
        public SHNFacility ShnStay { get; set; }
        public bool IsPaid { get; set; }

        public TravelEntry() { }

        public TravelEntry(string lcoe, string em, DateTime ed)
        {
            LastCountryOfEmbarkation = lcoe;
            EntryMode = em;
            EntryDate = ed;
        }

        public void AssignSHNFacility(SHNFacility i)
        {
            ShnStay = i;
            Console.WriteLine("Updated SDF");
        }

        public void CalculateSHNDuration()
        {
            if (LastCountryOfEmbarkation.ToLower() == "new zealand" || LastCountryOfEmbarkation.ToLower() == "vietnam")
            {
                ShnEndDate = EntryDate;
            }
            else if (LastCountryOfEmbarkation.ToLower() == "macao sar")
            {
                ShnEndDate = EntryDate.AddDays(7);
            }
            else
            {
                ShnEndDate = EntryDate.AddDays(14);
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\t" + "Last Country Of Embarkation: " + LastCountryOfEmbarkation + "\t" + "Entry Mode: " + EntryMode + "\t" + "Entry Date: " + EntryDate + "\t" + "End Date: " + ShnEndDate;
        }
    }

    class SafeEntry
    {
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public BusinessLocation Location { get; set; }

        public SafeEntry() { }

        public SafeEntry(DateTime c, BusinessLocation l)
        {
            CheckIn = c;
            Location = l;
        }

        public void PerformCheckOut()
        {
            CheckOut = DateTime.Now;
            Console.WriteLine("Check Out date updated");
        }

        public override string ToString()
        {
            return "Check In: " + CheckIn + "\t" + "Location: " + Location;
        }
    }

    class TraceTogetherToken
    {
        public string SerialNo { get; set; }
        public string CollectionLocation { get; set; }
        public DateTime ExpiryDate { get; set; }

        public TraceTogetherToken() { }

        public TraceTogetherToken(string s, string c, DateTime e)
        {
            SerialNo = s;
            CollectionLocation = c;
            ExpiryDate = e;
        }

        public bool IsEligibleForReplacement()
        {
            if (ExpiryDate < DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReplaceToken(string s, string c)
        {
            SerialNo = s;
            CollectionLocation = c;
        }

        public override string ToString()
        {
            return $"Serial No: {SerialNo}" +
                $"\tCollection Location: {CollectionLocation}" +
                $"\tExpiry Date: {ExpiryDate}";
        }
    }
}
