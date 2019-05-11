using System;

namespace FlatRent.Constants
{
    public static class MessageConstants
    {
        public const string NewAgreementSubject = "Sukurta nauja nuomos užklausa";
        public const string NewInvoiceSubject = "Sugeneruota nauja sąskaita";
        public const string NewIncidentSubject = "Sukurtas naujas incidentas";

        public const string Hostname = "http://192.168.1.204:3000";
        public static Func<string, string> SiteUrl = (string path) => $"{Hostname}{path}";
    }
}