using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsidResellers.Helpers
{
    public class ValidationError
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _code;
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }


        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public ValidationError(string description)
        {
            _description = description;
        }

        public ValidationError(string description, string code)
        {
            this._description = description;
            this._code = code;
        }
    }
    public class ValidationProperty
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _validationParams;
        public string ValidationParams
        {
            get { return _validationParams; }
            set { _validationParams = value; }
        }

        public ValidationProperty(string name, string validationParams)
        {
            this._name = name;
            this._validationParams = validationParams;
        }
    }

    public class ValidationHelper
    {
        private bool _succeeded = false;
        public bool Succeeded
        {
            get { return _succeeded; }
            set { _succeeded = value; }
        }

        private List<ValidationError> _errors = new List<ValidationError>();
        public List<ValidationError> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }

        public List<string> GetErrors()
        {
            List<string> errorList = new List<string>();
            foreach (var err in _errors)
            {
                errorList.Add(err.Name + " - " + err.Description);
            }
            return errorList;
        }

        public bool Object(object obj, ValidationProperty[] properties)
        {
            bool hasError = false;
            foreach (ValidationProperty property in properties)
            {
                if (obj.GetType().GetProperty(property.Name) != null)
                {
                    var propertyValue = obj.GetType().GetProperty(property.Name).GetValue(obj);
                    string[] validationParams = property.ValidationParams.Split('|');

                    foreach (string validationParam in validationParams)
                    {
                        string key = null;
                        string value = null;

                        if (validationParam.Contains(":"))
                        {
                            string[] keyValue = validationParam.Split(':');
                            key = keyValue[0];
                            value = keyValue[1];
                        }
                        else
                        {
                            key = validationParam;
                        }

                        ValidationError valError = this.validate(property.Name, key, propertyValue, value);
                        if (valError != null)
                        {
                            hasError = true;
                            _errors.Add(valError);
                        }
                    }
                }
            }

            _succeeded = !hasError;

            return !hasError;
        }

        private ValidationError validate(string propertyName, string key, object obj, string value = null)
        {
            ValidationError validationError = null;
            switch (key)
            {
                case "Required":
                    validationError = this.validateRequired(obj);
                    break;
                case "Max":
                    validationError = this.validateMax(obj, value);
                    break;
                case "NotAllowed":
                    validationError = this.validateNotAllowed(obj);
                    break;
                case "IsGuid":
                    validationError = this.validateIsGuid(obj);
                    break;
                case "IsInteger":
                    validationError = this.validateIsInteger(obj);
                    break;
                case "IsString":
                    validationError = this.validateIsString(obj);
                    break;
                case "IsLongitude":
                    validationError = this.validateLongitude(obj);
                    break;
                case "IsLatitude":
                    validationError = this.validateLatitude(obj);
                    break;
                default:
                    throw new NotSupportedException();
            }

            if (validationError != null)
                validationError.Name = propertyName;

            return validationError;
        }

        private ValidationError validateIsString(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                if (obj is string == false)
                    validationError = new ValidationError("Inte en sträng") { Code = "NotAString" };
            }

            return validationError;
        }

        private ValidationError validateIsGuid(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                try
                {
                    string str = obj.ToString();
                    Guid guid = new Guid(str);

                    if (guid == Guid.Empty)
                        validationError = new ValidationError("Inte ett Guid.") { Code = "NotAnGuid" };
                }
                catch
                {
                    validationError = new ValidationError("Inte ett Guid.") { Code = "NotAnGuid" };
                }
            }

            return validationError;
        }

        private ValidationError validateIsInteger(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                try
                {
                    int number = Convert.ToInt32(obj);
                    if (number < 1)
                        validationError = new ValidationError("Inte en giltig integer.") { Code = "NotAValidIntegr" };
                }
                catch
                {
                    validationError = new ValidationError("Inte en integer.") { Code = "NotAnInteger" };
                }
            }


            return validationError;
        }

        private ValidationError validateRequired(object obj)
        {
            ValidationError validationError = null;

            if (obj == null || obj.ToString() == "")
                validationError = new ValidationError("Fältet måste fyllas i.") { Code = "FieldIsRequired" };

            return validationError;
        }

        private ValidationError validateNotAllowed(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                try
                {
                    string str = obj.ToString();
                    Guid guid = new Guid(str);

                    if (guid != Guid.Empty)
                        validationError = new ValidationError("Fältet kan inte ändras.") { Code = "FieldCannotBeChanged" };
                }
                catch
                {
                    validationError = new ValidationError("Inte ett Guid.") { Code = "NotAnGuid" };
                }
            }

            return validationError;
        }

        private ValidationError validateMax(object obj, string value)
        {
            ValidationError validationError = null;
            if (value == null)
                throw new AggregateException("Värdet får inte vara null!");

            try
            {
                int expectedLenght = Convert.ToInt16(value);
                string objAsString = (string)obj;

                if (objAsString != null && expectedLenght < objAsString.Length)
                {
                    validationError = new ValidationError("Får inte vara längre än " + value + " tecken.") { Code = "IncorrectNumberOfCharacters" };
                }
            }
            catch (InvalidCastException e)
            {
                throw e;
            }

            return validationError;
        }   

        private ValidationError validateLongitude(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                try
                {
                    Regex regex = new Regex(@"^(\+|-)?(?:180(?:(?:\.0{1,7})?)|(?:[0-9]|[1-9][0-9]|1[0-7][0-9])(?:(?:\.[0-9]{1,7})?))$");
                    Match match = regex.Match(obj.ToString());
                    if (!match.Success)
                        validationError = new ValidationError("Longitud måste vara av datatypen double, mellan -180 och 180 och max ha sju decimaler.");
                }
                catch
                {
                    validationError = new ValidationError("Longitud måste vara av datatypen double");
                }
            }

            return validationError;
        }

        private ValidationError validateLatitude(object obj)
        {
            ValidationError validationError = null;

            if (obj != null)
            {
                try
                {
                    Regex regex = new Regex(@"^(\+|-)?(?:90(?:(?:\.0{1,7})?)|(?:[0-9]|[1-8][0-9])(?:(?:\.[0-9]{1,7})?))$");
                    Match match = regex.Match(obj.ToString());
                    if (!match.Success)
                        validationError = new ValidationError("Latitude måste vara av datatypen double, mellan -90 och 90 och max ha sju decimaler.");
                }
                catch
                {
                    validationError = new ValidationError("Latitud måste vara av datatypen double");
                }
            }

            return validationError;
        }

        public bool validateLatLong(string lat, string lon)
        {          
            if (lat == null && lon != null || lat != null && lon == null)
            {
                ValidationError valError = new ValidationError("Om en longitud angetts måste en latitud anges och vice versa");
                _errors.Add(valError);
                return false;
            }

            return true;
        } 
    }
}
