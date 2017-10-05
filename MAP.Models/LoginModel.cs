using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Models
{
    public class LoginModel
    {

        public LoginModel(string emailId, string password, bool rememberMe)
        {
            _EmailId = emailId;
            _Password = password;
            _RememberMe = rememberMe;
        }


        private string _EmailId;

        public string EmailId
        {
            get { return _EmailId; } 
        }

        private string _Password;

        public string Password
        {
            get { return _Password; } 
        }

        private bool _RememberMe;

        public bool RememberMe
        {
            get { return _RememberMe; } 
        }


    }
}
