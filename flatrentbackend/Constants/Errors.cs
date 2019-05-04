namespace FlatRent.Constants
{
    public static class Errors
    {
        public const string RecipientDoesNotExist = "Toks naudotojas neegzistuoja.";
        public const string MessageFaultDeletedOrRepaired = "Negalima siųsti žinutės jeigu incidentas neaktyvus.";
        public const string InvoiceAlreadyPaid = "Sąskaita jau apmokėta.";
        public const string MessageAgreementDeletedOrRejected = "Negalima siųsti žinutės jeigu sutartis neaktyvi.";
        public const string CantCreateNonActive = "Negalima registruoti naujo gedimo neaktyviai nuomos sutarčiai.";
        public const string CantDeleteRepaired = "Negalima ištrinti sutaisyto gedimo.";
        public const string InvoiceNotValid = "Sąskaita yra negaliojanti.";
        public const string CantDeleteWithActiveAgreements = "Buto negalima panaikinti jeigu yra aktyvi nuomos sutartis.";
        public const string AlreadyRequested = "Jau egzistuoja Jūsų sukurta užklausa.";
        public const string BadRequest = "Neteisinga užklausa.";
        public const string NotFound = "Įrašas nerastas.";
        public const string ImageAlreadyUploaded = "Ši nuotrauka jau buvo įkelta.";
        public const string AttachmentAlreadyUploaded = "Šis failas jau buvo įkeltas.";
        public const string BadCredentials = "Neteisingi prisijungimo duomenys.";
        public const string BadToken = "Blogas autorizacijos raktas.";
        public const string Required = "Šis laukas yra privalomas.";
        public const string EmailAddress = "Šis laukas turi būti teisingas el. pašto adresas.";
        public const string Phone = "Šis laukas turi būti teisingas telefono numeris.";
        public const string ValidDepartment = "Šio lauko reikšmė turi būti viena iš skyrių: {1}.";
        public const string MaxLength = "Simbolių kiekis turi būti mažesnis už {1}.";
        public const string MinLength = "Simbolių kiekis turi būti didesnis arba lygus {1}.";
        public const string Range = "Reikšmė turi būti nuo {1} iki {2}.";
        public const string DateAfter = "Data turi būti velesnė arba lygi {1}.";
        public const string DateBefore = "Data turi būti ankstesnė už {1}.";


        public const string UserIsNotClient = "Naudotojas nėra klientas.";
        public const string TenantCantBeOwner = "Savininkas negali būti nuomininkas.";
        
        public const string FlatNotAvailableForRent = "Butas šiuo metu nėra nuomojamas.";
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
        public const string NotFlatOwner = "Naudotojas nėra buto savininkas.";

        public const string InvalidImage = "Netinkamo formato nuotrauka. Naudokite JPEG/PNG formatą.";
        public const string MaxLengthFiles = "Prisegtų failų skaičius negali būti didesnis už {1}.";
        public const string MinLengthFiles = "Prisegtų failų skaičius turi būti didesnis arba lygus {1}.";
        public const string FileTooBig = "Failas per didelis.";

        public const string NotAuthor = "Naudotojas nėra įrašo autorius.";
        public const string MaxLengthFeatures = "Ypatybių kiekis negali būti didesnis už {0}.";
        public const string MaxLengthFeatureSymbols = "Ypatybės simbolių kiekis negali būti didesnis už {0}.";
        public const string FloorCantBeHigherThanTotalFloors = "Buto aukštas negali būti didesnis už aukštų skaičių.";
    }
}