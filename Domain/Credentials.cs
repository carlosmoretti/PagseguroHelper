using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace PagSeguroPayment.Domain
{
    public class Credentials
    {
        private string _email { get; set; }
        private string _token { get; set; }
        public Credentials(string email, string token)
        {
            _email = email;
            _token = token;
        }

        public string email => _email;
        public string token => _token;
    }
}
