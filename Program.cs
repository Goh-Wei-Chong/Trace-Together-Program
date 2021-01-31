//============================================================
// Student Number : S10204647, S10205678
// Student Name : Goh Wei Chong, Swan Htat Aung
// Module Group : T02
//============================================================

using System;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Add Facilities to List
            List<SHNFacility> facilityList = new List<SHNFacility>();
            List<Person> personList = new List<Person>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://covidmonitoringapiprg2.azurewebsites.net");
                Task<HttpResponseMessage> responseTask = client.GetAsync("/facility");
                responseTask.Wait();
                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<string> readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    string data = readTask.Result;

                    facilityList = JsonConvert.DeserializeObject<List<SHNFacility>>(data);
                }
            }
            foreach (SHNFacility shn in facilityList)
            {
                shn.FacilityVacancy = shn.FacilityCapacity;
            }

            //Add BusinessLocation to List
            List<BusinessLocation> locationlist = new List<BusinessLocation>();
            InitLocationlist(locationlist);

            //Add Person to List
            List<Resident> residentList = new List<Resident>();
            List<Visitor> visitorList = new List<Visitor>();
            InitPerson(residentList, visitorList, facilityList);

            foreach (Resident r in residentList)
            {
                personList.Add(r);
            }
            foreach (Visitor v in visitorList)
            {
                personList.Add(v);
            }

            //ViewPersonDetail(personList);
            //TokenMenu(residentList);
            //EditLocationCapacity(locationlist);
            //CheckIn(locationlist, personList);
            //CheckOut(locationlist, personList);
            //ViewVisitors(personList);
            //ViewFacilities(facilityList);
            //CreateVisitor(visitorList);
            //CreateTravelEntry(personList, facilityList);
            //CalculateSHNCharges(personList);
            //ContactTracing(personList, locationlist);

            //Main Menu
            string op = null;
            while (op == null)
            {
                Console.WriteLine("\nCovid-19 Monitoring System\n" +
                        "==========================\n" +
                        "[1] SafeEntry / TraceTogether\n" +
                        "[2] TravelEntry\n" +
                        "[3] General\n" +
                        "[4] Advanced Features\n" +
                        "[0] Exit");
                Console.WriteLine("Enter option: ");
                op = Console.ReadLine();

                if (op == "0")
                {
                    Environment.Exit(1);
                }
                else if (op == "1")
                {
                    SafeEntryMenu(locationlist, personList);
                }
                else if (op == "2")
                {
                    TravelEntryMenu(personList, facilityList);
                }
                else if (op == "3")
                {
                    GeneralMenu(locationlist, personList, facilityList);
                }
                else if (op == "4")
                {
                    AdvancedMenu(locationlist, personList);
                }else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                op = null;
                continue;
            }
        }
        
        //Advanced Menu
        static void AdvancedMenu(List<BusinessLocation> bList, List<Person> pList)
        {
            string op = null;
            while (op == null)
            {
                Console.WriteLine("\nAdvanced Features\n" +
                    "=================\n" +
                    "[1] Contact Tracing Report\n" +
                    "[2] SHN Status Report\n" +
                    "[3] Check Out Of All Locations\n" +
                    "[0] Back");
                Console.WriteLine("Enter option: ");
                op = Console.ReadLine();

                if (op == "0")
                {
                    return;
                }else if (op == "1")
                {
                    ContactTracing(pList, bList);
                }else if (op == "2")
                {
                    StatusReporting(pList);
                }else if (op == "3")
                {
                    CheckOutAll(bList, pList);
                }else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                op = null;
                continue;
            }
        }
        
        //SafeEntry / TraceTogether Menu
        static void SafeEntryMenu(List<BusinessLocation> bList, List<Person> pList)
        {
            string op = null;
            while (op == null)
            {
                Console.WriteLine("\nSafeEntry / TraceTogether\n" +
                        "=========================\n" +
                        "[1] Assign/Replace TraceTogether Token\n" +
                        "[2] Edit Business Location Capacity\n" +
                        "[3] SafeEntry Check-In\n" +
                        "[4] SafeEntry Check-Out\n" +
                        "[0] Back");
                Console.WriteLine("Enter option: ");
                op = Console.ReadLine();

                if (op == "0")
                {
                    return;
                }
                else if (op == "1")
                {
                    TokenMenu(pList);
                }
                else if (op == "2")
                {
                    EditLocationCapacity(bList);
                }
                else if (op == "3")
                {
                    CheckIn(bList, pList);
                }
                else if (op == "4")
                {
                    CheckOut(bList, pList);
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                op = null;
                continue;
            }
        }

        //TravelEntry Menu
        static void TravelEntryMenu(List<Person> pList, List<SHNFacility> fList)
        {
            string op = null;
            while (op == null)
            {
                Console.WriteLine("\nTravelEntry\n" +
                        "===========\n" +
                        "[1] Create Visitor\n" +
                        "[2] Create TravelEntry Record\n" +
                        "[3] Calculate SHN Charges\n" +
                        "[0] Back");
                Console.WriteLine("Enter option: ");
                op = Console.ReadLine();

                if (op == "0")
                {
                    return;
                }
                else if (op == "1")
                {
                    CreateVisitor(pList);
                }
                else if (op == "2")
                {
                    CreateTravelEntry(pList, fList);
                }
                else if (op == "3")
                {
                    CalculateSHNCharges(pList);
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
                op = null;
                continue;
            }
        }

        //General Menu
        static void GeneralMenu(List<BusinessLocation>bList, List<Person> pList, List<SHNFacility> fList)
        {
            int? op = null;
            while (op == null)
            {
                try
                {
                    Console.WriteLine("\nGeneral\n" +
                        "=======\n" +
                        "[1] List Person Details\n" +
                        "[2] List All Visitors\n" +
                        "[3] List All Business Locations\n" +
                        "[4] List All SHN Facilities\n" +
                        "[0] Back");
                    Console.WriteLine("Enter option: ");
                    op = Convert.ToInt32(Console.ReadLine());

                    if (4 < op || op < 0)
                    {
                        Console.WriteLine("**Invalid input. Try again.**");
                    }
                    else if (op == 0)
                    {
                        return;
                    }
                    else if (op == 1)
                    {
                        ViewPersonDetail(pList);
                    }
                    else if (op == 2)
                    {
                        ViewVisitors(pList);
                    }
                    else if (op == 3)
                    {
                        DisplayBusinessLocation(bList);
                    }
                    else if (op == 4)
                    {
                        ViewFacilities(fList);
                    }
                    op = null;
                    continue;
                }
                catch
                {
                    op = null;
                    Console.WriteLine("Invalid input. Try again.");
                    continue;
                }
            }
        }

        //Function to add Person to List
        static void InitPerson(List<Resident> rList, List<Visitor> vList, List<SHNFacility> facilityList)
        {
            using (StreamReader i = new StreamReader("Person.csv"))
            {
                string header = i.ReadLine();

                while ((header = i.ReadLine()) != null)
                {
                    string[] data = header.Split(',');

                    //Check if Person is Resident or Visitor
                    if (data[0] == "resident")
                    {
                        Resident r = new Resident(data[1], data[2], Convert.ToDateTime(data[3]));

                        //Check if resident has Token
                        if (data[6] != "")
                        {
                            //Format data tokenExpiryDate to work with Convert.ToDateTime
                            string[] dtr = data[8].Split("-");
                            string dtrr = dtr[0] + "/" + dtr[1] + "/" + dtr[2].Insert(0, "20");
                            //Add token to resident
                            r.Token = new TraceTogetherToken(data[6], data[7], Convert.ToDateTime(dtrr));
                        }

                        //Check for travelEntryLastCountry
                        if (data[9] != "")
                        {
                            r.AddTravelEntry(new TravelEntry(data[9], data[10], Convert.ToDateTime(data[11])));
                            r.TravelEntryList[0].ShnEndDate = Convert.ToDateTime(data[12]);
                            foreach (SHNFacility g in facilityList)
                            {
                                if (g.FacilityName == data[14])
                                {
                                    r.TravelEntryList[r.TravelEntryList.Count - 1].ShnStay = g;
                                    g.FacilityVacancy -= 1;
                                }
                            }
                        }
                        rList.Add(r);
                    }
                    else
                    {
                        Visitor v = new Visitor(data[1], data[4], data[5]);
                        if (data[9] != "")
                        {
                            v.AddTravelEntry(new TravelEntry(data[9], data[10], Convert.ToDateTime(data[11])));
                            v.TravelEntryList[0].ShnEndDate = Convert.ToDateTime(data[12]);
                            foreach (SHNFacility g in facilityList)
                            {
                                if (g.FacilityName == data[14])
                                {
                                    v.TravelEntryList[v.TravelEntryList.Count - 1].ShnStay = g;
                                    g.FacilityVacancy -= 1;
                                }
                            }
                        }
                        vList.Add(v);
                    }
                }
            }
        }

        //Function to add BusinessLocation to List
        static void InitLocationlist(List<BusinessLocation> locationlist)
        {
            string t;
            string[] y;
            using (StreamReader i = new StreamReader("BusinessLocation.csv"))
            {
                string header = i.ReadLine();

                while((t = i.ReadLine()) != null)
                {
                    y = t.Split(',');
                    BusinessLocation n = new BusinessLocation(y[0], y[1], Convert.ToInt32(y[2]));
                    locationlist.Add(n);
                }
            }
        }

        //Function to view person details
        static void ViewPersonDetail(List<Person> pList)
        {
            string u;
            bool samename = false;

            Console.WriteLine("\nList Person Details\n" +
                "===================");
            while (true)
            {
                //Prompt user for name
                Console.WriteLine("Enter name: ");
                string name = Console.ReadLine();

                //Find name in resident/visitor list
                foreach (Person p in pList)
                {
                    if (p.Name.ToLower() == name.ToLower())
                    {
                        samename = true;
                        if (p is Resident)
                        {
                            Resident r = (Resident)p;
                            //List Resident details
                            Console.WriteLine("\n===Resident===");
                            Console.WriteLine("{0,-25} {1,-25} {2}", "Address", "Last Left Country", "Token SerialNo");
                            Console.WriteLine("{0,-25} {1,-25} {2}", r.Address, r.LastLeftCountry, r.Token.SerialNo);
                        }
                        else if (p is Visitor)
                        {
                            Visitor v = (Visitor)p;
                            //List Visitor details
                            Console.WriteLine("\n===Visitor===");
                            Console.WriteLine("{0,-15} {1}", "PassportNo", "Nationality");
                            Console.WriteLine("{0,-15} {1}", v.PassportNo, v.Nationality);
                        }

                        //List travel entry details
                        Console.WriteLine("\n===Travel Entry===");
                        if (p.TravelEntryList.Count == 0)
                        {
                            Console.WriteLine("No travel entry");
                        }
                        else
                        {
                            Console.WriteLine("{0,-30} {1,-15} {2,-25} {3,-25} {4}", "Last Country Of Embarkation", "Entry Mode", "Entry Date", "SHN End Date", "SHN Facility");
                            p.TravelEntryList[p.TravelEntryList.Count - 1].CalculateSHNDuration();
                            if (p.TravelEntryList[p.TravelEntryList.Count - 1].ShnStay == null)
                            {
                                u = "NIL";
                            }
                            else
                            {
                                u = p.TravelEntryList[p.TravelEntryList.Count - 1].ShnStay.FacilityName;
                            }
                            Console.WriteLine("{0,-30} {1,-15} {2,-25} {3,-25} {4}", p.TravelEntryList[p.TravelEntryList.Count - 1].LastCountryOfEmbarkation, p.TravelEntryList[p.TravelEntryList.Count - 1].EntryMode, p.TravelEntryList[p.TravelEntryList.Count - 1].EntryDate, p.TravelEntryList[p.TravelEntryList.Count - 1].ShnEndDate, u);
                        }

                        //List safe entry details
                        Console.WriteLine("\n===Safe Entry===");
                        if (p.SafeEntryList.Count == 0)
                        {
                            Console.WriteLine("No safe entry");
                        }
                        else
                        {
                            Console.WriteLine("{0,-25} {1,-25} {2}", "Check In", "Check Out", "Business Location");

                            if (p.SafeEntryList[p.SafeEntryList.Count - 1].Location == null)
                            {
                                u = "NIL";
                            }
                            else
                            {
                                u = p.SafeEntryList[p.SafeEntryList.Count - 1].Location.BusinessName;
                            }
                            Console.WriteLine("{0,-25} {1,-25} {2}", p.SafeEntryList[p.SafeEntryList.Count - 1].CheckIn, p.SafeEntryList[p.SafeEntryList.Count - 1].CheckOut, u);
                        }
                        if (p is Visitor)
                        {
                            Console.WriteLine();
                            break;
                        }

                        if (p is Resident)
                        {
                            Resident r = (Resident)p;
                            //List Trace Together Token details
                            Console.WriteLine("\n===Trace Together Token===");
                            if (r.Token == null)
                            {
                                Console.WriteLine("No Trace Together Token");
                                Console.WriteLine();
                                break;
                            }
                            else
                            {
                                Console.WriteLine("{0,-10} {1,-22} {2}", "SerialNo", "Collection Location", "Expiry Date");
                                Console.WriteLine("{0,-10} {1,-22} {2}", r.Token.SerialNo, r.Token.CollectionLocation, r.Token.ExpiryDate);
                                Console.WriteLine();
                                break;
                            }
                        }
                    }
                }
                if (samename == false)
                {
                    Console.WriteLine("Name not found, enter the name again\n");
                }
                else
                {
                    break;
                }
            }
        }

        //Function to view visitors
        static void ViewVisitors(List<Person> pList)
        {
            Console.WriteLine("\nVisitors: ");
            Console.WriteLine("{0,-15} {1,-20} {2}", "Name", "PassportNo", "Nationality");
            foreach (Person p in pList)
            {
                if (p is Visitor)
                {
                    Visitor v = (Visitor)p;
                    Console.WriteLine("{0,-15} {1,-20} {2}", v.Name, v.PassportNo, v.Nationality);
                }
            }
            Console.WriteLine();
        }

        //Function edit and view Trace Together Token
        static void TokenMenu(List<Person> pList)
        {
            //Prompt user for name
            Console.WriteLine("\nCreate/Replace Token\n" +
                "===================");

            Resident rt = null;
            while (rt == null)
            {
                Console.WriteLine("Enter name: ");
                string nm = Console.ReadLine();

                //FInd name in resident list
                foreach (Person r in pList)
                {
                    if (r is Resident && r.Name.ToLower() == nm.ToLower())
                    {
                        rt = (Resident)r;
                    }
                }

                if (rt == null)
                {
                    Console.WriteLine("\nResident not found, try again.");
                }
            }

            //Check if resident has token
            //Redirect to option
            if (rt.Token == null)
            {
                //Request resident for new token serial number and collection location
                Console.WriteLine("\nCreate New Token\n" +
                    "================");

                bool unique = false;
                string sn = null;

                while (unique == false)
                {
                    Console.WriteLine("Enter new token serial number: ");
                    string sen = Console.ReadLine();
                    unique = true;
                    foreach (Person r in pList)
                    {
                        if (r is Resident)
                        {
                            Resident rr = (Resident)r;
                            if (rr.Token.SerialNo != null && rr.Token.SerialNo.ToLower() == sen.ToLower())
                            {
                                Console.WriteLine("\nSerial number already exists. Try again.");
                                unique = false;
                                continue;
                            }
                        }
                    }
                    sn = sen;
                }

                Console.WriteLine("Enter collection location: ");
                string ln = Console.ReadLine();

                //Calculate token expiry date
                DateTime ed = DateTime.Now.AddMonths(6);

                //Put input data in resident object
                rt.Token = new TraceTogetherToken(sn, ln, ed);

                //Display success
                Console.WriteLine("\nNew token successfully created.\n" +
                    "=============================\n" +
                    $"Serial No: {sn}\n" +
                    $"Collection Location: {ln}\n" +
                    $"Expiry Date: {ed.ToString("MM/dd/yyyy")}");
            }
            else
            {
                //Check if resident is eligible
                if (rt.Token.IsEligibleForReplacement())
                {
                    //Request new info
                    Console.WriteLine("\nReplace Token\n" +
                    "=============");

                    bool unique = false;
                    string sn = null;

                    while (unique == false)
                    {
                        Console.WriteLine("Enter new token serial number: ");
                        string sen = Console.ReadLine();
                        unique = true;
                        foreach (Person r in pList)
                        {
                            if (r is Resident)
                            {
                                Resident rr = (Resident)r;
                                if (rr.Token.SerialNo != null && rr.Token.SerialNo.ToLower() == sen.ToLower())
                                {
                                    Console.WriteLine("\nSerial number already exists. Try again.");
                                    unique = false;
                                    continue;
                                }
                            }
                        }
                        sn = sen;
                    }

                    Console.WriteLine("Enter collection location: ");
                    string ln = Console.ReadLine();

                    rt.Token.ReplaceToken(sn, ln);

                    //Display success
                    Console.WriteLine("Token successfully replaced.");
                }
                else
                {
                    Console.WriteLine("\nYou are not eligible for replacement for a new token as it has not expired yet.");
                }

            }
        }

        //Function display business location
        static void DisplayBusinessLocation(List<BusinessLocation> bList)
        {
            Console.WriteLine("\nBusiness Locations:");
            
            foreach (BusinessLocation b in bList)
            {
                //Display
                Console.WriteLine(b.ToString());
            }
        }

        //Function edit business location capacity
        static void EditLocationCapacity(List<BusinessLocation> bList)
        {
            Console.WriteLine("\nEdit Location Capacity\n" +
                "======================");

            BusinessLocation bl = null;
            while (bl == null)
            {
                DisplayBusinessLocation(bList);
                Console.WriteLine("\nEnter branch code: ");
                string ce = Console.ReadLine();

                foreach (BusinessLocation b in bList)
                {
                    if (b.BranchCode == ce)
                    {
                        bl = b;
                    }
                }

                if (bl == null)
                {
                    Console.WriteLine("Branch code not found. Try again.");
                }
            }

            Console.WriteLine($"Current capacity is {bl.MaximumCapacity}.\n");

            int cap = -1;
            while (cap == -1)
            {
                try
                {
                    Console.WriteLine("Enter new capacity: ");
                    cap = Convert.ToInt32(Console.ReadLine());
                    if (cap < 0)
                    {
                        Console.WriteLine("Invalid input. Enter a positive number.");
                    }
                }catch
                {
                    Console.WriteLine("Invalid input. Enter a valid whole number (example: 4).");
                }
            }

            bl.MaximumCapacity = cap;
            Console.WriteLine($"\nLocation capacity is now set to {cap}.");
        }

        //Function safe entry check in
        static void CheckIn(List<BusinessLocation> bList, List<Person> pList)
        {
            Console.WriteLine("\nSafe Entry Check-In\n" +
                "===================");

            Person p = null;
            while (p == null)
            {
                Console.WriteLine("Enter name: ");
                string nm = Console.ReadLine().ToLower();
                
                //Search for person
                foreach (Person pe in pList)
                {
                    if (pe.Name.ToLower() == nm.ToLower())
                    {
                        p = pe;
                    }
                }

                if (p == null)
                {
                    Console.WriteLine("Name not found. Try again.\n");
                }
            }

            //Dispay Business Locations
            DisplayBusinessLocation(bList);

            //Search business location
            BusinessLocation bl = null;
            
            while (bl == null)
            {
                Console.WriteLine("\nEnter branch code: ");
                string bc = Console.ReadLine();

                foreach (BusinessLocation b in bList)
                {
                    if (b.BranchCode == bc)
                    {
                        bl = b;
                    }
                }

                if (bl == null)
                {
                    Console.WriteLine("Branch code not found. Try again.");
                }
            }

            if (bl.IsFull() == false)
            {
                //Increase visitor count & add safe entry to list
                bl.VisitorsNow += 1;
                p.AddSafeEntry(new SafeEntry(DateTime.Now, bl));
                Console.WriteLine("Check-In successful.");

            }else
            {
                Console.WriteLine("Could not check in as location is at max capacity.");
            }
            
        }

        //Function safe entry check out
        static void CheckOut(List<BusinessLocation> bList, List<Person> pList)
        {
            Console.WriteLine("\nSafe Entry Check-Out\n" +
                "====================");

            Person p = null;
            while (p == null)
            {
                Console.WriteLine("Enter name: ");
                string nm = Console.ReadLine().ToLower();

                //Search for person
                foreach (Person pe in pList)
                {
                    if (pe.Name.ToLower() == nm.ToLower())
                    {
                        p = pe;
                    }
                }

                if (p == null)
                {
                    Console.WriteLine("Name not found. Try again.\n");
                }
            }

            //List non checked out safe entry
            List<SafeEntry> sList = new List<SafeEntry>();
            int n = 1;
            foreach (SafeEntry se in p.SafeEntryList)
            {
                if (se.CheckOut == new DateTime(0001,1,1))
                {
                    Console.WriteLine($"[{n}] " + 
                        $"Check-In time: {se.CheckIn}"+
                        $"\tLocation: {se.Location.BusinessName}" +
                        $"\tBranch Code: {se.Location.BranchCode}");
                    sList.Add(se);
                    n += 1;
                }
            }
            Console.WriteLine("[0] Back");

            int num = -1;
            while (num < 0)
            {
                try
                {
                    Console.WriteLine("\nEnter option: ");
                    if (Console.ReadLine() == "0")
                    {
                        return;
                    }
                    num = Convert.ToInt32(Console.ReadLine()) - 1;
                    sList[num].CheckOut = DateTime.Now;
                    foreach (BusinessLocation b in bList)
                    {
                        if (b == sList[num].Location)
                        {
                            b.VisitorsNow -= 1;
                        }
                    }
                    Console.WriteLine("Check-out successful.");
                }catch
                {
                    Console.WriteLine("Enter a valid option. Try again.");
                    num = -1;
                    continue;
                }
            }
        }

        //Function to view Facilities
        static void ViewFacilities(List<SHNFacility> facilityList)
        {
            Console.WriteLine("\nSHN Facilities:");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-30} {4,-30} {5}", "Facility Name", "Capacity", "Vacancy", "DistFromAirCheckpoint", "DistFromSeaCheckpoint", "DistFromLandCheckpoint");
            foreach (SHNFacility f in facilityList)
            {
                Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-30} {4,-30} {5}", f.FacilityName, f.FacilityCapacity, f.FacilityVacancy, f.DistFromAirCheckpoint, f.DistFromSeaCheckpoint, f.DistFromLandCheckpoint);
            }
            Console.WriteLine();
        }

        //Function to create visitor
        static void CreateVisitor(List<Person> pList)
        {
            string name = "";
            string passportno = "";
            Console.WriteLine("\nCreate Visitor\n" +
                "==============");
            while (true)
            {
                Console.WriteLine("Enter Name:");
                name = Console.ReadLine();
                foreach (Person p in pList)
                {
                    if (p.Name.ToLower() == name.ToLower())
                    {
                        Console.WriteLine("Name has already been used, enter another unique name");
                        name = "";
                    }
                }
                if (name != "")
                {
                    break;
                }
            }

            while (true)
            {
                Console.WriteLine("Enter Passport Number:");
                passportno = Console.ReadLine();
                foreach (Person p in pList)
                {
                    if (p is Visitor)
                    {
                        Visitor v = (Visitor)p;
                        if (v.PassportNo.ToLower() == passportno.ToLower())
                        {
                            Console.WriteLine("Passport number has already been used, enter another unique passport number");
                            passportno = "";
                        }
                    }
                }
                if (passportno != "")
                {
                    break;
                }
            }
            Console.WriteLine("Enter Nationality:");
            string nationality = Console.ReadLine();

            pList.Add(new Visitor(name, passportno, nationality));

            Console.WriteLine("Visitor Created\n");
        }

        //Function to create travel entry
        static void CreateTravelEntry(List<Person> pList, List<SHNFacility> facilityList)
        {
            Console.WriteLine("\nCreate TravelEntry Record\n" +
                "=========================");
            string mode, fname;
            Console.WriteLine("Enter Name:");
            string name = Console.ReadLine();

            foreach (Person p in pList)
            {
                if (p.Name.ToLower() == name.ToLower())
                {
                    Console.WriteLine("Enter last country of embarkation:");
                    string lce = Console.ReadLine();

                    while (true)
                    {
                        Console.WriteLine("Enter entry mode:");
                        mode = Console.ReadLine();

                        if (mode.ToLower() == "air" || mode.ToLower() == "land" || mode.ToLower() == "sea")
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Wrong type of mode, try again");
                        }
                    }

                    TravelEntry t = new TravelEntry(lce, mode, DateTime.Now);
                    t.IsPaid = false;

                    if (p is Visitor)
                    {
                        Visitor v = (Visitor)p;
                        if (lce.ToLower() != "new zealand" || lce.ToLower() != "macao sar" || lce.ToLower() != "vietnam")
                        {
                            while (true)
                            {
                                ViewFacilities(facilityList);
                                Console.WriteLine("Enter facility assigned to visitor:");
                                fname = Console.ReadLine();
                                foreach (SHNFacility f in facilityList)
                                {
                                    if (f.FacilityName.ToLower() == fname.ToLower())
                                    {
                                        t.ShnStay = f;
                                        f.FacilityVacancy -= 1;
                                    }
                                }
                                if (t.ShnStay == null)
                                {
                                    Console.WriteLine("Facility name not found,try again\n");
                                }
                                else
                                {
                                    Console.WriteLine();
                                    break;
                                }
                            }
                        }
                    }

                    t.CalculateSHNDuration();
                    p.AddTravelEntry(t);

                    Console.WriteLine("New Travel Entry created for {0}", p.Name);
                    Console.WriteLine();
                    break;
                }
            }
        }

        //Function to calculate SHN charges
        static void CalculateSHNCharges(List<Person> pList)
        {
            Console.WriteLine("\nCalculate SHN Charges\n" +
                "=====================");
            double price = 0;
            string answer;
            Console.WriteLine("Enter name: ");
            string name = Console.ReadLine();

            foreach (Person p in pList)
            {
                if (p.Name.ToLower() == name.ToLower())
                {
                    if (p.TravelEntryList.Count == 0)
                    {
                        Console.WriteLine("No Travel Entry");
                        Console.WriteLine();
                        break;
                    }

                    else if (p.TravelEntryList.Count >= 1)
                    {
                        foreach (TravelEntry t in p.TravelEntryList)
                        {
                            if (t.ShnEndDate < DateTime.Now && t.IsPaid == false)
                            {
                                if (p is Resident)
                                {
                                    Resident r = (Resident)p;
                                    price = r.CalculateSHNCharges();
                                }
                                if (p is Visitor)
                                {
                                    Visitor v = (Visitor)p;
                                    price = v.CalculateSHNCharges();
                                }
                                while (true)
                                {
                                    Console.WriteLine("The price is ${0:0.00}, would you like to pay?(y/n)", price);
                                    answer = Console.ReadLine();
                                    if (answer.ToLower() == "y" || answer.ToLower() == "yes")
                                    {
                                        Console.WriteLine("Payment has been settled");
                                        t.IsPaid = true;
                                        break;
                                    }
                                    else if (answer.ToLower() == "n" || answer.ToLower() == "no")
                                    {
                                        Console.WriteLine("Payment not paid");
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid input, try again");
                                    }
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("There are no unpaid payments");
                        Console.WriteLine();
                        break;
                    }
                }
            }
        }

        //advanced function contact tracing
        static void ContactTracing(List<Person> pList, List<BusinessLocation> bList)
        {
            File.WriteAllText("ContactTracing.csv", String.Empty);
            Console.WriteLine("\nContact Tracing Reporting\n" +
                "=========================");

            DisplayBusinessLocation(bList);
            //Search business location
            string bl = null;

            while (bl == null)
            {
                Console.WriteLine("\nEnter branch code: ");
                string bc = Console.ReadLine();

                foreach (BusinessLocation b in bList)
                {
                    if (b.BranchCode == bc)
                    {
                        bl = bc;
                    }
                }

                if (bl == null)
                {
                    Console.WriteLine("Branch code not found. Try again.");
                }
            }

            Console.WriteLine("\nThe progeam will search safe entry check-ins from a specified start and end time.\n" +
                "Enter your date time in this format dd/mm/yyyy hour:min ");

            DateTime? startdt = null;
            DateTime? enddt = null;

            while (enddt == null && startdt == null)
            {
                try
                {
                    Console.WriteLine("\nEnter start date time (e.g 21/10/2000 08:00):");

                    startdt = Convert.ToDateTime(Console.ReadLine());

                    Console.WriteLine("\nEnter end date time (e.g 21/10/2000 09:00): ");
                    enddt = Convert.ToDateTime(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Invalid input. Try again.");
                    startdt = null;
                    enddt = null;
                }
            }

            List<string> intime = new List<string>();

            foreach (Person p in pList)
            {
                foreach (SafeEntry se in p.SafeEntryList)
                {
                    if (se.Location.BranchCode == bl && se.CheckIn > startdt && se.CheckOut < enddt)
                    {
                        if (se.CheckOut == DateTime.MinValue)
                        {
                            intime.Add($"{p.Name},{se.CheckIn},Not checked out");
                        }else
                        {
                            intime.Add($"{p.Name},{se.CheckIn},{se.CheckOut}");
                        }
                    }
                }
            }

            string filename = "ContactTracing.csv";
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write("Name,CheckIn TIme, CheckOut Time\n");
                foreach (string s in intime)
                {
                    writer.WriteLine(s);
                }
            }
            Console.WriteLine("File created successfully.");

        }

        //Advance function to view and write in csv file all Persons in SHN facility given the date
        static void StatusReporting(List<Person> pList)
        {
            File.WriteAllText("ShnRecord.csv", String.Empty);
            Console.WriteLine("\nStatus Reporting\n" +
                "=====================");

            DateTime? d = null;
            while (d == null)
            {
                try
                {
                    Console.WriteLine("Enter a date (dd/mm/yyyy):");
                    d = Convert.ToDateTime(Console.ReadLine());
                }catch
                {
                    Console.WriteLine("Invalid inpput. Try again.");
                    d = null;
                }
            }

            Console.WriteLine("{0,-10} {1,-20} {2}", "Name", "SHN End Date", "Location");
            foreach (Person p in pList)
            {
                if (p.TravelEntryList.Count >= 1)
                {
                    foreach (TravelEntry t in p.TravelEntryList)
                    {
                        if (t.EntryDate <= d && t.ShnEndDate >= d)
                        {
                            Console.WriteLine("{0,-10} {1,-20} {2}", p.Name, t.ShnEndDate, t.LastCountryOfEmbarkation);
                            var newLine = string.Format("{0},{1},{2}", p.Name, t.ShnEndDate, t.LastCountryOfEmbarkation);
                            using (StreamWriter s = new StreamWriter("ShnRecord.csv", true))
                            {
                                s.WriteLine(newLine);
                            }
                        }
                    }
                }
            }
        }

        //Additional Feature 
        static void CheckOutAll(List<BusinessLocation> bList, List<Person> pList)
        {
            Console.WriteLine("\nSafe Entry Check-Out Of All Locations\n" +
                "=====================================");

            Person p = null;
            while (p == null)
            {
                Console.WriteLine("Enter name: ");
                string nm = Console.ReadLine().ToLower();

                //Search for person
                foreach (Person pe in pList)
                {
                    if (pe.Name.ToLower() == nm.ToLower())
                    {
                        p = pe;
                    }
                }

                if (p == null)
                {
                    Console.WriteLine("Name not found. Try again.\n");
                }
            }

            foreach (SafeEntry se in p.SafeEntryList)
            {
                if (se.CheckOut == DateTime.MinValue)
                {
                    se.CheckOut = DateTime.Now;
                    foreach (BusinessLocation b in bList)
                    {
                        if (b == se.Location)
                        {
                            b.VisitorsNow -= 1;
                        }
                    }
                }
            }

            Console.WriteLine("Checked out of all locations.");
        }
    }
}
