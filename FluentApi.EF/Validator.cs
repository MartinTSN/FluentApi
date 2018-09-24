using System;
using System.Linq;

namespace FluentApi.EF
{
    /// <summary>
    /// A static class called Validator that validates.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Checks if the provided email is valid or not.
        /// </summary>
        /// <param name="email">A string email.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsEmailValid(string email)
        {
            if (String.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            else if (email.Length < 10 || email.Length > 100)
            {
                return false;
            }
            else if (!email.Contains("@"))
            {
                return false;
            }
            else if (!email.EndsWith(".com") && !email.EndsWith(".net") && !email.EndsWith(".dk"))
            {
                return false;
            }
            else if (email.EndsWith("@.com") || email.EndsWith("@.net") || email.EndsWith("@.dk"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided phoneNumber is valid.
        /// </summary>
        /// <param name="phone">A string phoneNumber.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsPhoneValid(string phone)
        {
            if (!phone.All(Char.IsNumber))
            {
                return false;
            }
            else if (phone.Length < 8)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided address is valid.
        /// </summary>
        /// <param name="address">A string address.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsAddressValid(string address)
        {
            if (String.IsNullOrWhiteSpace(address))
            {
                return false;
            }
            else if (address.Length < 7)
            {
                return false;
            }
            else if (!address.Substring(address.Length -4).All(Char.IsNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided name is valid.
        /// </summary>
        /// <param name="name">A string name.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsNameValid(string name)
        {
            if (String.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            else if (name.Length < 2 || name.Length > 100)
            {
                return false;
            }
            else if (name.All(Char.IsNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided birthDay is valid.
        /// </summary>
        /// <param name="birthDay">A datetime birthDay.</param>
        /// <returns>Returns false if the validation isn't correct. True otherwise.</returns>
        public static bool IsBirthDayValid(DateTime birthDay)
        {
            if (birthDay == null)
            {
                return false;
            }
            else if (birthDay.Year >= (DateTime.Now.Year - 18))
            {
                return false;
            }
            else if (birthDay.Year <= (DateTime.Now.Year - 70))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided employmentDate is valid.
        /// </summary>
        /// <param name="employmentDate">A datetime employmentDate.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsEmploymentDateValid(DateTime employmentDate)
        {
            if (employmentDate == null)
            {
                return false;
            }
            else if (employmentDate > DateTime.Now.Date.AddDays(1))
            {
                return false;
            }
            else if (employmentDate < new DateTime(1950, 07, 30))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Takes the provided employmentDate and birthDay and validates if the employmentDate is after birthday.
        /// </summary>
        /// <param name="birthDay">A datetime birthDay.</param>
        /// <param name="employmentDate">A datetime employmentDate</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsBirthDayEmploymentDateValid(DateTime birthDay, DateTime employmentDate)
        {
            if (employmentDate < birthDay)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided cprNumber is valid.
        /// </summary>
        /// <param name="cprNumber">A string cprNumber.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsCPRNumberValid(string cprNumber)
        {
            if (String.IsNullOrWhiteSpace(cprNumber))
            {
                return false;
            }
            else if (!cprNumber.Substring(cprNumber.Length - 4).All(Char.IsNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided money value is valid.
        /// </summary>
        /// <param name="money">A decimal money value.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsMoneyValid(decimal money)
        {
            if (money < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided description is valid.
        /// </summary>
        /// <param name="description">A string description.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsDescriptionValid(string description)
        {
            if (String.IsNullOrWhiteSpace(description))
            {
                return false;
            }
            else if (description.All(c => Char.IsLetter(c) && Char.IsSeparator(c)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided startDate is valid.
        /// </summary>
        /// <param name="startDate">A datetime startDate.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsStartDateValid(DateTime startDate)
        {
            if (startDate == null)
            {
                return false;
            }
            else if (startDate.Date > DateTime.Now.Date.AddDays(1))
            {
                return false;
            }
            else if (startDate < new DateTime(1950, 07, 10))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Checks if the provided endDate is valid.
        /// </summary>
        /// <param name="endDate">A datetime endDate.</param>
        /// <returns>Returns false if the validation isn't correct, true otherwise.</returns>
        public static bool IsEndDateValid(DateTime endDate)
        {
            if (endDate == null)
            {
                return false;
            }
            else if (endDate.Date < DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
