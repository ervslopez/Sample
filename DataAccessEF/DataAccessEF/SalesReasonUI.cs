using Library;
using Library.Entities;
using SalesReasonBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF
{
    class SalesReasonUI
    {
        static SalesRsnBL slsRsnBL = new SalesRsnBL();

        static void Main(string[] args)
        {
            int choice;

            do
            {
                Console.Clear();
                ReadPath();

                choice = Menu();
                switch (choice)
                {
                    case 1:
                        AddPath();
                        break;

                    case 2:
                        ReadSpecificPath();
                        break;

                    case 3:
                        UpdatePath();
                        break;

                    case 4:
                        DeletePath();
                        break;

                    case 5:
                        EmployeeManagersPath();
                        break;

                    case 6:
                        ToExit();
                        break;
                }
            } while (true);
        }

        static void ReadPath()
        {
            List<SalesReason> reasonList = slsRsnBL.RetrieveAllRecord();
            if(reasonList.Count > 0)
            {
                var maxLength = reasonList.OrderByDescending(x => x.ReasonType.Length).FirstOrDefault().ReasonType.Length;
                var maxLength2 = reasonList.OrderByDescending(x => x.Name.Length).FirstOrDefault().Name.Length;

                Console.WriteLine("\n\n{0, 5} ║ {1} ║ {2} {3} ", "ID".PadLeft(5), "Reason Type".PadLeft(maxLength + 4), "Name".PadLeft(maxLength2), "║");
                foreach (var item in reasonList)
                {
                    Console.WriteLine("{0, 5} ║ {1} ║ {2} {3} ", item.SalesReasonID.ToString(), item.ReasonType.PadLeft(maxLength + 4), item.Name.PadLeft(maxLength2), "║");
                }
                Console.WriteLine("\n\t\t\t{0} {1}", "Sales Reason Count: ", slsRsnBL.CountNumberOFSalesReason());
            }else
            {
                Console.WriteLine("\t\t\t\t NO RECORDS YET");
            }
        }   

        static void ReadSpecificPath()
        {
            SalesReason reason = slsRsnBL.RetrieveSpecificRecord(RequestID());
            if(reason != null)
            {
                Console.WriteLine("{0,25} ║ {1,11} ║ {2,5} ║ ", "Name".PadRight(reason.Name.Length), "Reason Type".PadRight(reason.ReasonType.Length), "ID");
                Console.WriteLine("{0,25} ║ {1,11} ║ {2,5} ║ ", reason.Name.PadRight(reason.Name.Length), reason.ReasonType.PadRight(reason.ReasonType.Length), reason.SalesReasonID);
            }else
            {
                Console.WriteLine("Sales Reason Does Not Exist Yet");
            }
            Console.ReadKey();
            
        }

        static void AddPath()
        {
            int response = slsRsnBL.CreateSalesReasonRecord(new SalesReason()
            {
                Name = RequestName(),
                ReasonType = RequestType(),
                ModifiedDate = RequestDate()
            });
            
            PrintTransactionDescription(response>0);
            ReadPath();
        }

        static void UpdatePath()
        {
            int response = slsRsnBL.UpdateSalesReasonRecord(new SalesReason()
            {
                SalesReasonID = RequestID(),
                Name = RequestName(),
                ReasonType = RequestType(),
                ModifiedDate = RequestDate()
            });
            PrintTransactionDescription(response > 0);
            ReadPath();
        }

        static void DeletePath()
        {
            int response = slsRsnBL.DeleteSalesReasonRecord(RequestID());
            PrintTransactionDescription(response > 0);
            ReadPath();
        }

        static void EmployeeManagersPath()
        {
            List<EmployeeManager> manager = slsRsnBL.CheckManager(RequestID());

            if(manager.Count > 0)
            {
                var maxLength = manager.OrderByDescending(x => x.EmployeeName.Length).FirstOrDefault().EmployeeName.Length;
                var maxLength2 = manager.OrderByDescending(x => x.Name.Length).FirstOrDefault().Name.Length;

                foreach (var item in manager)
                {
                    Console.WriteLine("\nEmployee: {0} Manager: {1} Node: {2}",
                        item.EmployeeName.PadRight(maxLength),
                        item.Name.PadRight(maxLength2),
                        item.Node);
                }
            }else
            {
                Console.WriteLine("No Records Found");
            }
            Console.ReadKey();
        }

        static int Menu()
        {
            int choice = 0;
            Console.WriteLine("\n[1] - Create SalesReason");
            Console.WriteLine("[2] - Read Specific SalesReason");
            Console.WriteLine("[3] - Update SalesReason");
            Console.WriteLine("[4] - Delete SalesReason");
            Console.WriteLine("[5] - Check Employee Managers");
            Console.WriteLine("[6] - Exit Application\n");
            Int32.TryParse(Console.ReadLine(), out choice);
            return choice;
        }

        static string RequestName()
        {
            Console.Write("\nPlease Input Name: ");
            return Console.ReadLine();
        }

        static string RequestType()
        {
            Console.Write("\nPlease Input Reason Type: ");
            return Console.ReadLine();
        }

        static DateTime RequestDate()
        {
            return DateTime.Now;
        }

        static int RequestID()
        {
            int ID = 0;
            Console.Write("Input Employee ID: ");
            Int32.TryParse(Console.ReadLine(), out ID);
            return ID;
        }

        static void PrintSuccessful()
        {
            Console.WriteLine("\nTransaction Successful!");
        }

        static void PrintFail()
        {
            Console.WriteLine("\nTransaction Failed!");
        }

        static void PrintTransactionDescription(bool output)
        {
            if (output)
                PrintSuccessful();
            else
                PrintFail();
            Console.ReadKey();
        }

        static void ToExit()
        {
            Console.WriteLine("The Application is about to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
