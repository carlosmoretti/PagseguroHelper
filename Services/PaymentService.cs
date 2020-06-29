using Newtonsoft.Json;
using PagSeguroPayment.Domain;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Transactions;
using System.Xml;

namespace PagSeguroPayment.Services
{
    public static class PaymentService
    {
        public static string GetToken(Credentials credentials)
        {
            WebRequest request = WebRequest.Create($"https://ws.sandbox.pagseguro.uol.com.br/v2/sessions?email={credentials.email}&token={credentials.token}");
            request.Method = "POST";
            var res = request.GetResponse();

            using(Stream stream = res.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                var streamRes = reader.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(streamRes);

                var xml = doc.ChildNodes[1].InnerText;
                return xml;
            }
        }

        public static string ObterBandeiraCartao(string token, string sixFirstCardNumber)
        {
            WebRequest request = WebRequest.Create($"https://df.uol.com.br/df-fe/mvc/creditcard/v1/getBin?tk={token}&creditCard={sixFirstCardNumber}");
            request.Method = "GET";
            var res = request.GetResponse();

            using (Stream stream = res.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                var streamRes = reader.ReadToEnd();

                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<CreditCardInfo>(streamRes);

                return obj.Bin.Brand.Name;
            }
        }

        public static string GetCardToken(string session, double amount, string cardNumber, string cardBrand, string cardCvv, int cardExpirationMonth, int cardExpirationYear)
        {
            WebRequest request = WebRequest.Create("https://df.uol.com.br/v2/cards");
            request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            request.Method = "POST";

            string postData = $"sessionId={session}&amount={amount}&cardNumber={cardNumber}&cardBrand={cardBrand}&cardCvv={cardCvv}&cardExpirationMonth={cardExpirationMonth}&cardExpirationYear={cardExpirationYear}";
            var pam = Encoding.UTF8.GetBytes(postData);

            request.ContentLength = pam.Length;

            Stream dataStream = request.GetRequestStream();
            dataStream.Write(pam, 0, pam.Length);
            dataStream.Close();

            var res = request.GetResponse();

            using (Stream stream = res.GetResponseStream())
            {
                var reader = new StreamReader(stream);
                var streamRes = reader.ReadToEnd();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(streamRes);

                var xml = doc.ChildNodes[1].InnerText;
                return xml;
            }
        }
        public static PagSeguroPayment.Domain.Transaction CreatePayment(string cardtoken, decimal valor, Credentials credentials, string tituloCompra)
        {
            try
            {
                WebRequest request = WebRequest.Create($"https://ws.sandbox.pagseguro.uol.com.br/v2/transactions?email={credentials.email}&token={credentials.token}");
                request.UseDefaultCredentials = true;
                request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                request.Method = "POST";
                CultureInfo.CurrentCulture = new CultureInfo("en-US");

                var meuip = GetIpAddress();

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string postData = $@"paymentMode=default&paymentMethod=creditCard&receiverEmail=carlosmoretti2019@gmail.com&currency=BRL&itemId1=0001&itemDescription1={tituloCompra}&itemAmount1={valor}&itemQuantity1=1&reference=REF1234&senderName=Carlos A S Moretti&senderCPF=22111944785&senderAreaCode=11&senderPhone=56273440&senderEmail=c44172005802607089434@sandbox.pagseguro.com.br&senderIp={meuip}&shippingAddressStreet=Av.Brig.FariaLima&shippingAddressNumber=1384&shippingAddressComplement=5oandar&shippingAddressDistrict=JardimPaulistano&shippingAddressPostalCode=01452002&shippingAddressCity=SaoPaulo&shippingAddressState=SP&shippingAddressCountry=BRA&creditCardToken={cardtoken}&installmentQuantity=1&installmentValue={valor}&noInterestInstallmentQuantity=2&creditCardHolderName=Carlos A S Moretti&creditCardHolderCPF=22111944785&creditCardHolderBirthDate=27/10/1987&creditCardHolderAreaCode=11&creditCardHolderPhone=56273440&billingAddressStreet=Av.Brig.FariaLima&billingAddressNumber=1384&billingAddressComplement=5oandar&billingAddressDistrict=JardimPaulistano&billingAddressPostalCode=01452002&billingAddressCity=SaoPaulo&billingAddressState=SP&billingAddressCountry=BRA";

                postData = postData.Replace("\n", "");
                postData = postData.Replace("\r", "");

                var pam = Encoding.UTF8.GetBytes(postData);

                request.ContentLength = pam.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(pam, 0, pam.Length);
                dataStream.Close();

                var res = request.GetResponse();

                using (Stream stream = res.GetResponseStream())
                {
                    var reader = new StreamReader(stream);
                    var streamRes = reader.ReadToEnd();

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(streamRes);

                    var xml = doc.ChildNodes[1].InnerText;

                    var json = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.None, true);
                    var jsonbypass = json.Substring(70);
                    return JsonConvert.DeserializeObject<PagSeguroPayment.Domain.Transaction>(jsonbypass);
                }
            } catch(Exception e)
            {
                var transaction = new Domain.Transaction();
                transaction.ErrorList.Add("Não foi possível realizar essa compra!");
                return transaction;
            }
        }
        private static string GetIpAddress()
        {
            string externalip = new WebClient().DownloadString("http://icanhazip.com");
            return externalip.Replace("\n", "");
        }
    }
}
