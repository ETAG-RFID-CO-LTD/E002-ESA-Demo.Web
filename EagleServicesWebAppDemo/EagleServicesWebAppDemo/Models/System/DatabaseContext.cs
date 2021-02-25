using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;

namespace EagleServicesWebApp.Models
{
    public class DatabaseContext : DbContext
    {
        int ErrorNo { get; set; }
        string ErrorMessage { get; set; }

        public DatabaseContext() : base("name=DefaultConnection")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                //foreach (var eve in e.EntityValidationErrors)
                //{
                //    Debug.WriteLine(@"Entity of type ""{0}"" in state ""{1}"" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                //    foreach (var ve in eve.ValidationErrors)
                //    {
                //        Debug.WriteLine(@"- Property: ""{0}"", Error: ""{1}""", ve.PropertyName, ve.ErrorMessage);
                //    }
                //}
                //throw;
                ErrorMessage = e.Message;

                return 0;
            }
            catch (DbUpdateException e)
            {
                //Add your code to inspect the inner exception and/or
                //e.Entries here.
                //Or just use the debugger.
                //Added this catch (after the comments below) to make it more obvious 
                //how this code might help this specific problem
                ErrorMessage = e.Message;

                return 0;
            }
            catch (Exception e)
            {
                //Debug.WriteLine(e.Message);
                //throw;
                ErrorMessage = e.Message;

                return 0;
            }
        }
    }
}