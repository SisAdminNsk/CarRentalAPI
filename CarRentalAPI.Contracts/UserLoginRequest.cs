using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.Contracts
{
    public class UserLoginRequest
    {
        public string Email {  get; set; }
        public string Password { get; set; }

        public UserLoginRequest(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
