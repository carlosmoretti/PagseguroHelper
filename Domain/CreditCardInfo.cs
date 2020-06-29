namespace PagSeguroPayment.Domain
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public partial class CreditCardInfo
    {
        [JsonProperty("bin")]
        public Bin Bin { get; set; }
    }

    public partial class Bin
    {
        [JsonProperty("length")]
        public object Length { get; set; }

        [JsonProperty("country")]
        public Country Country { get; set; }

        [JsonProperty("brand")]
        public Brand Brand { get; set; }

        [JsonProperty("bin")]
        public long BinBin { get; set; }

        [JsonProperty("cvvSize")]
        public long CvvSize { get; set; }

        [JsonProperty("expirable")]
        public string Expirable { get; set; }

        [JsonProperty("validationAlgorithm")]
        public string ValidationAlgorithm { get; set; }

        [JsonProperty("bank")]
        public object Bank { get; set; }

        [JsonProperty("cardLevel")]
        public string CardLevel { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        [JsonProperty("reasonMessage")]
        public object ReasonMessage { get; set; }
    }

    public partial class Brand
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public partial class Country
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("isoCode")]
        public string IsoCode { get; set; }

        [JsonProperty("isoCodeThreeDigits")]
        public string IsoCodeThreeDigits { get; set; }
    }
}
