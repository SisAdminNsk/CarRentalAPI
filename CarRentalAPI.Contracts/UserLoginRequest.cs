using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPI.Contracts
{
    public class UserLoginRequest
    {
        public string Login {  get; set; }
        public string Password { get; set; }

        public UserLoginRequest(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}
