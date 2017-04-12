using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsidResellers.Helpers
{
    public class GeneralError
    {
        protected string self = "item";
        public string NotFound;
        public string CouldNotGet;
        public string ValidationError;
        public string CouldNotCreate;
        public string CouldNotDelete;
        public string CouldNotUpdate;
        public string NotValid;
        public GeneralError()
        {
            init();
        }
        protected void init()
        {
            NotFound = selfToUpper() + " med angivet ID existerar inte.";
            CouldNotGet = "Kunde inte hitta " + self;
            ValidationError = "Validering lyckades inte.";
            CouldNotCreate = "Kunde inte skapa " + self;
            CouldNotDelete = "Kunde inte ta bort " + self;
            CouldNotUpdate = "Kunde inte uppdatera " + self;
            NotValid = "Felaktig objekttyp";
        }

        private string selfToUpper()
        {
            return self.First().ToString().ToUpper() + self.Substring(1);
        }
    }

    public class CompanyError : GeneralError
    {
        public CompanyError()
        {
            self = "företag";
            init();
        }
    }
    public class StoreError : GeneralError
    {
        public StoreError()
        {
            self = "butik";
            init();
        }
    }

    public class ErrorMessage
    {
        public string DatabaseError = "Kunde inte ansluta till datbas.";
        public string NotAValidObject = "Inte ett giltigt objekt.";
        public string Oops = "Oops, something went wrong...";
        public CompanyError Company { get; set; }
        public StoreError Store { get; set; }

        public ErrorMessage()
        {
            Company = new CompanyError();
            Store = new StoreError();
        }

    }
}
