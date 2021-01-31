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
    abstract class Person
    {
        public string Name { get; set; }
        public List<SafeEntry> SafeEntryList { get; set; }
        public List<TravelEntry> TravelEntryList { get; set; }

        public Person()
        {
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public abstract double CalculateSHNCharges();

        public void AddTravelEntry(TravelEntry i)
        {
            TravelEntryList.Add(i);
        }

        public void AddSafeEntry(SafeEntry i)
        {
            SafeEntryList.Add(i);
        }

        public Person(string n)
        {
            Name = n;
            SafeEntryList = new List<SafeEntry>();
            TravelEntryList = new List<TravelEntry>();
        }

        public override string ToString()
        {
            return "Name: " + Name;
        }
    }

    class Resident : Person
    {
        public string Address { get; set; }
        public DateTime LastLeftCountry { get; set; }
        public TraceTogetherToken Token { get; set; }

        public Resident(string n, string a, DateTime l) : base(n)
        {
            Address = a;
            LastLeftCountry = l;
            Token = new TraceTogetherToken();
        }

        public override double CalculateSHNCharges()
        {
            if(TravelEntryList.Count == 0)
            {
                Console.WriteLine("This person has not travelled anywhere");
                return 0;
            }
            else
            {
                TravelEntry i = TravelEntryList[TravelEntryList.Count - 1];
                if (i.LastCountryOfEmbarkation.ToLower() == "new zealand" || i.LastCountryOfEmbarkation.ToLower() == "vietnam")
                {
                    return 200 * 1.07;
                }
                else if (i.LastCountryOfEmbarkation.ToLower() == "macao sar")
                {
                    return 220 * 1.07;
                }
                else
                {
                    return 1220 * 1.07;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\t" + "Address: " + Address + "\t" + "Last Left Country: " + LastLeftCountry;
        }
    }

    class Visitor : Person
    {
        public string PassportNo { get; set; }
        public string Nationality { get; set; }

        public Visitor(string n, string p, string na) : base(n)
        {
            PassportNo = p;
            Nationality = na;
        }

        public override double CalculateSHNCharges()
        {
            if (TravelEntryList.Count == 0)
            {
                Console.WriteLine("This person has not travelled anywhere");
                return 0;
            }
            else
            {
                TravelEntry i = TravelEntryList[TravelEntryList.Count - 1];
                if (i.LastCountryOfEmbarkation.ToLower() == "new zealand" || i.LastCountryOfEmbarkation.ToLower() == "vietnam")
                {
                    return 200 * 1.07;
                }
                else if (i.LastCountryOfEmbarkation.ToLower() == "macao sar")
                {
                    return 280 * 1.07;
                }
                else
                {
                    double cost = i.ShnStay.CalculateTravelCost(i.EntryMode, i.EntryDate);
                    return (cost + 2200) * 1.07;
                }
            }
        }

        public override string ToString()
        {
            return base.ToString() + "\t" + "PassportNo: " + PassportNo + "\t" + "Nationality: " + Nationality;
        }
    }
}

