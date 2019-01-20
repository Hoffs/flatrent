namespace FlatRent.Constants
{
    public static class Errors
    {
        public const string BadCredentials = "Neteisingi prisijungimo duomenys.";
        public const string BadToken = "Blogas autorizacijos raktas.";
        public const string Required = "Šis laukas yra privalomas.";
        public const string EmailAddress = "Šis laukas turi būti teisingas el. pašto adresas.";
        public const string Phone = "Šis laukas turi būti teisingas telefono numeris.";
        public const string ValidDepartment = "Šio lauko reikšmė turi būti viena iš skyrių: {1}.";
        public const string MaxLength = "Simbolių kiekis turi būti mažesnis už {1}.";
        public const string MinLength = "Simbolių kiekis turi būti didesnis už {1}.";
        public const string Range = "Reikšmė turi būti nuo {1} iki {2}.";
        public const string DateAfter = "Data turi būti velesnė arba lygi {1}.";


        public const string UserIsNotClient = "Naudotojas nėra klientas.";
        
        public const string FlatAlreadyRented = "Butas jau išnomuotas.";
        public const string FlatRentPeriodGreater = "Nuomos periodas turi būti ilgesnis už {0} dienas.";
        public const string FlatRentPeriodLess = "Nuomos periodas turi būti trumpesnis už {0} dienas.";
        
        public const string AgreementNotFound = "Nuomos periodas turi būti trumpesnis už {0} dienas.";
        public const string AgreementCancelNotOwner = "Negalima nutraukti nepriklausančios sutarties.";
        public const string AgreementPdfNotOwner = "Sutartį gali peržiūrėti tik jos sudarytojas.";

        public const string OwnerNotFound = "Savininkas nerastas.";
        public const string FlatNotFound = "Butas nerastas.";
        
        public const string Exception = "Įvyko nežinoma klaida.";
        public const string Alphanumeric = "Leidžiami tik skaičiai ir raidės.";
        public const string EmailAlreadyExists = "Šis el. paštas jau yra naudojamas.";
    }
}