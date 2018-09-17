using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FluentApi.EF
{
    public static class Validator
    {
        public static bool IsEmailValid(string s)
        {
            if (s.Length < 10 || s.Length > 100)
            {
                return false;
            }
            else if (!s.EndsWith(".com") && !s.EndsWith(".net") && !s.EndsWith(".dk"))
            {
                return false;
            }
            else if (s.EndsWith("@.com") || s.EndsWith("@.net") || s.EndsWith("@.dk"))
            {
                return false;
            }
            else if (!s.Contains("@"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsPhoneValid(string s)
        {
            if (!s.All(Char.IsNumber))
            {
                return false;
            }
            else if (s.Length < 8)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsNameValid(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return false;
            }
            else if (s.Length < 2 || s.Length > 100)
            {
                return false;
            }
            else if (s.All(Char.IsNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsBirthDayValid(DateTime d)
        {
            if (d == null)
            {
                return false;
            }
            else if (d.Year >= (DateTime.Now.Year - 18))
            {
                return false;
            }
            else if (d.Year <= (DateTime.Now.Year - 70))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsEmploymentDateValid(DateTime d)
        {
            if (d == null)
            {
                return false;
            }
            else if (d.Year > DateTime.Now.Year)
            {
                return false;
            }
            else if (d.Year < new DateTime(1950).Year)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsBirthDayEmploymentDateValid(DateTime birthday, DateTime EmploymentDate)
        {
            if (EmploymentDate.Year < birthday.Year)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsCPRNumberValid(string s)
        {
            if (s == null)
            {
                return false;
            }
            else if (!s.Substring(s.Length - 4).All(Char.IsNumber))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsMoneyValid(decimal d)
        {
            if (d < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsDescriptionValid(string s)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return false;
            }
            else if (s.All(c => Char.IsLetter(c) && Char.IsSeparator(c)))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsStartDateValid(DateTime d)
        {
            if (d == null)
            {
                return false;
            }
            else if (d.Date > DateTime.Now.Date)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsEndDateValid(DateTime d)
        {
            if (d == null)
            {
                return false;
            }
            else if (d.Date < DateTime.Now.Date)
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
