using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountsBackend.BusinessLogic.Services.AuthService
{
    public class UserRegisterRequest
    {
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}