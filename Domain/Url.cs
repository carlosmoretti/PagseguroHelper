using System;
using System.Collections.Generic;
using System.Text;

namespace PagSeguroPayment.Domain
{
    public static class Url
    {
        public static string CREATE_SESSION => "https://ws.pagseguro.uol.com.br/v2/sessions?email={EMAIL}@hotmail.com&token={TOKEN}";
    }
}
