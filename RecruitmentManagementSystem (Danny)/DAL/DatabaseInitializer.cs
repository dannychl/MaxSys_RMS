using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RecruitmentManagementSystem__Danny_.Models;

namespace RecruitmentManagementSystem__Danny_.DAL
{
    public class DatabaseInitializer
    {
        public class SchoolInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DatabaseContext>
        {
            protected override void Seed(DatabaseContext context)
            {
                var students = new List<Home>
                    {
                        new Home{Name="Carson", Age = 18},
                        new Home{Name="Abc", Age = 18},
                        new Home{Name="DSFF", Age = 18},
                        new Home{Name="EWn", Age = 18}

                    };

                context.SaveChanges();
            }
        }
    }
}