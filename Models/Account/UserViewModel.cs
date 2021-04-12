using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models.Account
{
    public class UserViewModel
    {
        [Display(Name = "帳號")]
        public string Account { get; set; }
        [Display(Name = "密碼")]
        public string Password { get; set; }
        public string Authcode { get; set; }
    }
}
