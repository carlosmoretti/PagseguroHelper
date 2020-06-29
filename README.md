# PagseguroHelper
Helper para transações com pagseguro usando .NET CORE!

```csharp
using MercadoPago.Common;
using MercadoPago.DataStructures.Customer.Card;
using MercadoPago.DataStructures.Preference;
using MercadoPago.Resources;
using PagSeguroPayment.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks.Dataflow;
using System.Xml.Linq;

namespace MercadoPago
{
    class Program
    {
        static void Main(string[] args)
        {
            PagSeguroPayment.Domain.Credentials cre = new PagSeguroPayment.Domain.Credentials("SEU_EMAIL@gmail.com", "SEU_TOKEN");
            var token = PaymentService.GetToken(cre);
            var bandeira = PaymentService.ObterBandeiraCartao(token, "411111");
            var cardToken = PaymentService.GetCardToken(token, 1, "4111111111111111", bandeira, "123", 12, 2030);
            var compra = PaymentService.CreatePayment(cardToken, 10.00M, cre, "Testando compra!");

            Console.WriteLine();
        }
    }
}

```
