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
    }
}