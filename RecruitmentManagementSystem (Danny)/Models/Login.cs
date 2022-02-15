using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecruitmentManagementSystem__Danny_.Models
{
    public partial class Login
    {
        public int LoginId { get; set; }
        public string LoginUsername { get; set; }
        public string LoginPassword { get; set; }
        public Nullable<int> LoginEmpId { get; set; }
        public string FirstTimeLogin { get; set; }
        public string TempPassword { get; set; }
        public string LoginResetPasswordCode { get; set; }

        [NotMapped]
        [Compare("LoginPassword")]
        public string ConfirmLoginPassword { get; set; }
}
}