using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GorinGrain.Models;

namespace GorinGrain.ViewModels
{
    public class UserVM
    {
        public UserVM()
        {
            User = new UserPO();
            UserList = new List<UserPO>();
        }

        public UserPO User { get; set; }

        public List<UserPO> UserList { get; set; }

        public string ErrorMessage { get; set; }

        public bool RememberMe { get; set; }
    }
}