using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace fes.Models
{
    public class FESContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx

        public FESContext() : base("name=FESSiteContext")
        {
            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
        }

        public System.Data.Entity.DbSet<fes.Models.ClaimFile> ClaimFiles { get; set; }
        public System.Data.Entity.DbSet<fes.Models.Field> Fields { get; set; }
        public System.Data.Entity.DbSet<fes.Models.FileFieldValue> FileFieldValues { get; set; }
    }
}
