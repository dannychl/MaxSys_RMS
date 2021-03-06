using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using RecruitmentManagementSystem__Danny_.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace RecruitmentManagementSystem__Danny_.DAL
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseContext")
        {
        }

        public DbSet<Candidate> Candidate { get; set; }

        public DbSet<Interview> Interviewer { get; set; }

        public DbSet<InterviewDetail> InterviewDetail { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<InterviewerComment> InterviewerComment { get; set; }
        public DbSet<OnBoard> OnBoard { get; set; }

    }
}