using Library;
using Library.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;



namespace SalesReasonBL
{
    public class SalesRsnBL
    {
        List<Exception> exeptionList = new List<Exception>();

        TransactionOptions transOP = new TransactionOptions() { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = new TimeSpan(0, 30, 0) };

        public List<SalesReason> RetrieveAllRecord()
        {
            List<SalesReason> salesReasonList = new List<SalesReason>();
            try
            {
                using (var trans = new TransactionScope(TransactionScopeOption.Required, transOP))
                {
                    using (var context = new AdventureWorks2008Entities())
                    {
                        salesReasonList = (from slsRsn in context.SalesReasons
                                           orderby slsRsn.Name
                                           select slsRsn).ToList();
                    }
                };

            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return salesReasonList;
        }


        //public List<T> RetrieveAllRecordGenerics<T>(object objectClass)
        //{
        //    List<T> tableList = new List<T>();
        //    try
        //    {
        //        using (var trans = new TransactionScope(TransactionScopeOption.Required, transOP))
        //        {
        //            Type type = objectClass.GetType();
        //            string tableName = type.Name.Substring(0, type.Name.Length - 2);

        //            PropertyInfo tableProp = typeof(AdventureWorks2008Entities).GetProperty(tableName);
        //            Type tableType = tableProp.PropertyType;
        //            Type pocoType = tableType.GetGenericArguments()[0];

        //            int id = (int)type.GetProperty("ID").GetValue(objectClass);

        //            using (var context = new AdventureWorks2008Entities())
        //            {
        //                object entity = context.Set(objectClass.GetType());

        //                object table = tableProp.GetValue(context);
        //            }
        //        };

        //    }
        //    catch (Exception e)
        //    {
        //        exeptionList.Add(e);
        //    }
        //    return tableList;
        //}

        public SalesReason RetrieveSpecificRecord(int ID)
        {
            SalesReason salesReason = new SalesReason();
            try
            {
                using (var trans = new TransactionScope(TransactionScopeOption.Required, transOP))
                {
                    using (var context = new AdventureWorks2008Entities())
                    {
                        salesReason = context.SalesReasons.Where(x => x.SalesReasonID == ID).FirstOrDefault();
                    }
                };

            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return salesReason;
        }

        public int CreateSalesReasonRecord(SalesReason salesReason)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    // context.SalesReasons.Add(salesReason);

                    context.SalesReasons.Add(new SalesReason()
                    {
                        Name = salesReason.Name,
                        ReasonType = salesReason.ReasonType,
                        ModifiedDate = DateTime.Now
                    });
                    returnValue = context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return returnValue;
        }

        public int UpdateSalesReasonRecord(SalesReason salesReason)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    SalesReason lastEntry = context.SalesReasons.Where(x => x.SalesReasonID == salesReason.SalesReasonID).SingleOrDefault();

                    if (lastEntry != null)
                    {
                        lastEntry.Name = salesReason.Name;
                        lastEntry.ReasonType = salesReason.ReasonType;
                        lastEntry.ModifiedDate = DateTime.Now;
                        //context.Entry(salesReason).State = System.Data.Entity.EntityState.Modified;
                        returnValue = context.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return returnValue;
        }

        public int DeleteSalesReasonRecord(int ID)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    returnValue = context.Database.ExecuteSqlCommand("DELETE FROM Sales.SalesReason WHERE SalesReasonID = @ID", new SqlParameter("@ID", ID));
                }
            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return returnValue;
        }

        public int CountNumberOFSalesReason()
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    returnValue = context.Database.SqlQuery<int>("SELECT COUNT(SalesReasonID) FROM Sales.SalesReason").FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return returnValue;
        }

        public List<EmployeeManager> CheckManager(int ID)
        {
            List<EmployeeManager> managerList = new List<EmployeeManager>();
            try
            {
                using (var trans = new TransactionScope(TransactionScopeOption.Required, transOP))
                {
                    using (var context = new AdventureWorks2008Entities())
                    {
                        var result = context.Database.SqlQuery<uspGetEmployeeManagers_Result>("dbo.uspGetEmployeeManagers @BusinessEntityID", new SqlParameter("@BusinessEntityID", ID)).ToList();
                        // managerList = context.Database.SqlQuery<uspGetEmployeeManagers_Result>("dbo.uspGetEmployeeManagers @BusinessEntityID", new SqlParameter("@BusinessEntityID", ID)).ToList();
                        foreach (var item in result)
                        {
                            managerList.Add(new EmployeeManager()
                            {
                                Name = item.ManagerFirstName + " " + item.ManagerLastName,
                                EmployeeName = item.FirstName + " " + item.LastName,
                                Node = item.OrganizationNode
                            });
                        }
                    }
                };
            }
            catch (Exception e)
            {
                exeptionList.Add(e);
            }
            return managerList;
        }

    }
}
