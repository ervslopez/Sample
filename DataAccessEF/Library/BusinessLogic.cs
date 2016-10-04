using Library.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class BusinessLogic
    {

        public void RetrieveCategoryRecord()
        {
            using (var context = new AdventureWorks2008Entities())
            {
                var results = (from prdtCat in context.ProductCategories
                               select prdtCat).ToList();
                if (results != null && results.Count > 0)
                {
                    foreach (var item in results)
                    {
                        Console.WriteLine(item.ProductCategoryID + " " + item.Name + " " + item.ModifiedDate);
                        foreach (var item2 in item.ProductSubcategories)
                        {
                            Console.WriteLine("\t" + item2.Name);
                        }
                    }
                    Console.ReadKey();
                }
            }
        }

        public void RetrieveJoinedList()
        {
            using (var context = new AdventureWorks2008Entities())
            {
                var results = (from person in context.People
                               join emp in context.Employees
                               on person.BusinessEntityID equals emp.BusinessEntityID
                               join empdept in context.EmployeeDepartmentHistories
                               on emp.BusinessEntityID equals empdept.BusinessEntityID
                               join dept in context.Departments
                               on empdept.DepartmentID equals dept.DepartmentID
                               where person.BusinessEntityID == 1
                               select new
                               {
                                   FullName = person.LastName + ", " + person.LastName,
                                   Department = dept.Name
                               }).OrderBy(x => x.FullName);
                foreach (var item in results)
                {
                    Console.WriteLine("Name: {0} \t Department Name: {1}", item.FullName, item.Department);
                }
                Console.ReadLine();
            }
        }

        public void RetrieveProdCategoryList()
        {
            using (var context = new AdventureWorks2008Entities())
            {
                productCategoryList = (from pc in context.ProductCategories
                                       orderby pc.ProductCategoryID
                                       select new ProductCategoryClass()
                                       {
                                           ID = pc.ProductCategoryID,
                                           Name = pc.Name
                                       }).ToList();

                foreach (var item in productCategoryList)
                {
                    Console.WriteLine("ID: {0} \t Name: {1}", item.ID, item.Name);
                }
                Console.ReadLine();
            }
        }

        public void RetrieveProdSubCatCountPerCat()
        {
            using (var context = new AdventureWorks2008Entities())
            {
                var newCount = (from cat in context.ProductCategories
                                select cat).ToList();

                foreach (var item in newCount)
                {
                    Console.WriteLine("ID: " + item.ProductCategoryID + " Name: " + item.Name + " Count: " + item.ProductSubcategories.Count());
                }
                Console.ReadKey();
            }
        }

        public int CreateNewDeptRecord(string name, string groupName)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    Department dept = new Department();

                    dept.Name = name;
                    dept.GroupName = groupName;
                    dept.ModifiedDate = DateTime.Now;

                    context.Departments.Add(dept);

                    returnValue = context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            return returnValue;
        }

        public int DeleteDeptRecord(string name)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    context.Departments.Remove(context.Departments.Where(x => x.Name == name).LastOrDefault());
                    returnValue = context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            return returnValue;
        }

        public int UpdateDepartmentRecord(string name, string groupName)
        {
            int returnValue = 0;
            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    Department lastEntry = (from dept in context.Departments
                                            where dept.DepartmentID == 1
                                            select dept).SingleOrDefault();

                    if (lastEntry != null)
                    {
                        lastEntry.GroupName = groupName;
                        lastEntry.Name = name;
                        returnValue = context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
            return returnValue;
        }

        public void CallingThroughFunctionImportStoredProcedure()
        {
            using (var context = new AdventureWorks2008Entities())
            {
                int empID = 17;
                var results = context.uspGetManagerEmployees(empID);

                if (results != null)
                {
                    foreach (var item in results)
                    {
                        Console.WriteLine("Employee: {0}\tImmediate Supervisor: {1}", item.FirstName + " " + item.LastName, item.ManagerFirstName + " " + item.ManagerLastName);
                    }
                }
            }
        }

        public void SubCategoryGroupByCategory()
        {
            //using (var context = new AdventureWorks2008Entities())
            //{
            //    var newCount = (from cat in context.ProductCategories
            //                    select cat).ToList();

            //    foreach (var item in newCount)
            //    {
            //        Console.WriteLine("ID: " + item.ProductCategoryID + " Name: " + item.Name + " Count: " + item.ProductSubcategories.Count());
            //    }
            //    Console.ReadKey();
            //}
        }

        public void RetrieveNativeSqlQuery()
        {
            //int deptID = 123;
            //string name = "Business Development";

            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    var lastEntry = context.Departments.SqlQuery("SELECT * FROM HumanResources.Department").FirstOrDefault();

                    Console.WriteLine(lastEntry.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        public void RetrieveNativeSqlQueryWithParams()
        {
            //int deptID = 123;
            //string name = "Business Development";

            try
            {
                using (var context = new AdventureWorks2008Entities())
                {
                    var lastEntry = context.Departments.SqlQuery("SELECT * FROM HumanResources.Department WHERE DepartmentID = @ID", new SqlParameter("@ID", 1)).FirstOrDefault();

                    Console.WriteLine(lastEntry.Name);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        public void GetNativeSqlResults()
        {
            short deptID = 1;
            string name = "HealthCare";
            using (var context = new AdventureWorks2008Entities())
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@name", name),
                    new SqlParameter("@id", deptID)};

                var lastEntry = context.Database.ExecuteSqlCommand("UPDATE HumanResources.Department SET Name = @name WHERE DepartmentID = @id ", parameters);

                Console.WriteLine(lastEntry);
            }
        }

        public void GetNativeSqlResults2()
        {
            short deptID = 1;
            string name = "HealthCare";
            using (var context = new AdventureWorks2008Entities())
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@name", name),
                    new SqlParameter("@id", deptID)};

                var Entity = context.Database.SqlQuery<string>("SELECT Name FROM HumanResources.Department");
                var lastEntry = context.Database.ExecuteSqlCommand("UPDATE HumanResources.Department SET Name = @name WHERE DepartmentID = @id ", parameters);

                foreach (var item in Entity)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine(lastEntry);
            }
        }

        public void GetNativeSqlResults3()
        {
            using (var context = new AdventureWorks2008Entities())
            {

                var Entity = context.Database.SqlQuery<int>("SELECT Count(*) FROM HumanResources.Department").SingleOrDefault();
                //var lastEntry = context.Database.ExecuteSqlCommand("UPDATE HumanResources.Department SET Name = @name WHERE DepartmentID = @id ", parameters);
                
                Console.WriteLine(Entity);
            }
        }
    }
}
