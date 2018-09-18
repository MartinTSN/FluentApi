namespace FluentApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class Person
    {
        /// <summary>
        /// The Employee FirstName value is stored here.
        /// </summary>
        private string firstName;
        /// <summary>
        /// The Employee LastName value is stored here.
        /// </summary>
        private string lastName;

        /// <summary>
        /// Person constructor with everything.
        /// </summary>
        /// <param name="firstName">A string firstName</param>
        /// <param name="lastName">A string lastName</param>
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        /// <summary>
        /// A parameterless Person constructor
        /// </summary>
        public Person()
        {
        }

        /// <summary>
        /// Takes the FirstName value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref = "ArgumentNullException" > Thrown when the FirstName is null.</exception>
        /// <exception cref = "ArgumentOutOfRangeException" > Throws when the Firstname is under 2 or over 100 characters.</exception>
        /// <exception cref = "ArgumentException" > Thrown when the Firstname has a number.</exception>
        [StringLength(100)]
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The Firstname must be set");
                //}
                //if (value.Length < 2 || value.Length > 100)
                //{
                //    throw new ArgumentOutOfRangeException("The Firstname must be over 2 and under 100 characters");
                //}
                //if (value.All(Char.IsNumber))
                //{
                //    throw new ArgumentException("There cant be any numbers in the value");
                //}
                if (!Validator.IsNameValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                firstName = value;
            }
        }

        /// <summary>
        /// Takes the LastName value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref = "ArgumentNullException" > Thrown when the Lastname is null.</exception>
        /// <exception cref = "ArgumentOutOfRangeException" > Throws when the Lastname is under 2 or over 100 characters.</exception>
        /// <exception cref = "ArgumentException" > Throws when the Lastname has a number.</exception>
        [StringLength(100)]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The Lastname must be set");
                //}
                //if (value.Length < 2 || value.Length > 100)
                //{
                //    throw new ArgumentOutOfRangeException("The Lastname must be over 2 and under 100 characters");
                //}
                //if (value.All(Char.IsNumber))
                //{
                //    throw new ArgumentException("The name must not have any numbers");
                //}
                if (!Validator.IsNameValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                lastName = value;
            }
        }
    }


    public partial class Employee : Person
    {

        /// <summary>
        /// The Employee BirthDay/date is stored here.
        /// </summary>
        private DateTime birthDay;
        /// <summary>
        /// The Employee Hiredate is stored here.
        /// </summary>
        private DateTime employementDate;
        /// <summary>
        /// The Employee CPRBnumber is stored here.
        /// </summary>
        /// <remarks>Is used with the ContactInfo Birthday.</remarks>
        private string cprNumber;
        /// <summary>
        /// The Employee Salary is stored here.
        /// </summary>
        private decimal salary;

        /// <summary>
        /// Employee constructor with everything
        /// </summary>
        /// <param name="firstName">A string firstName</param>
        /// <param name="lastName">A string lastName</param>
        /// <param name="birthDay">A datetime birthDay</param>
        /// <param name="employmentDate">A datetime employmentDate</param>
        /// <param name="cprNumber">A string cprNumber</param>
        /// <param name="salary">A decimal salary</param>
        /// <remarks>Inherits from the person constructor with everything</remarks>
        public Employee(string firstName, string lastName, DateTime birthDay, DateTime employmentDate, string cprNumber, decimal salary)
            : base(firstName, lastName)
        {
            BirthDay = birthDay;
            EmploymentDate = employmentDate;
            CPRNumber = cprNumber;
            Salary = salary;
        }

        /// <summary>
        /// A Parameterless Employee constructor
        /// </summary>
        /// <remarks>Inherits from the parameterless person constructor</remarks>
        public Employee()
            : base()
        {

        }


        public int Id { get; set; }

        /// <summary>
        /// Takes the BirthDay value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the value is over 70 or under 18.</exception>
        [Column(TypeName = "date")]
        public DateTime BirthDay
        {
            get
            {
                return birthDay;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The birthDay must be set.");
                //}
                //if (value.Year >= (DateTime.Now.Year - 18))
                //{
                //    throw new ArgumentOutOfRangeException("The date must be atleast 18 years old.");
                //}
                //if (value.Year <= (DateTime.Now.Year - 70))
                //{
                //    throw new ArgumentOutOfRangeException("The date must be under 70 years ago.");
                //}
                if (!Validator.IsBirthDayValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                birthDay = value;
            }
        }

        /// <summary>
        /// Takes the EmployementDate and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the value is in the future.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the value is older than their age.</exception>
        [Column(TypeName = "date")]
        public DateTime EmploymentDate
        {
            get
            {
                return employementDate;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The date must be set");
                //}
                //if (value.Year > DateTime.Now.Year)
                //{
                //    throw new ArgumentOutOfRangeException("You cannot hire a person in the future.");
                //}
                //if (value.Year < BirthDay.Year)
                //{
                //    throw new ArgumentOutOfRangeException("You cannot hire a person before they're born.");
                //}
                if (!Validator.IsEmploymentDateValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                if (!Validator.IsBirthDayEmploymentDateValid(BirthDay, value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                employementDate = value;
            }
        }

        /// <summary>
        /// Takes the CPR Number and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        /// <exception cref="FormatException">Thrown when the last 4 characters is not numbers</exception>
        [Required]
        [StringLength(100)]
        public string CPRNumber
        {
            get
            {
                return cprNumber;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The value must be set.");
                //}
                //if (!value.Substring(value.Length - 4).All(Char.IsNumber))
                //{
                //    throw new FormatException("The CPR must be 4 numbers");
                //}
                if (!Validator.IsCPRNumberValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                cprNumber = value;
            }
        }

        /// <summary>
        /// Takes the Salary value and validates it. Returns an exception is something is wrong.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the value is negative.</exception>
        [Column(TypeName = "money")]
        public decimal Salary
        {
            get
            {
                return salary;
            }
            set
            {
                //if (value < 0)
                //{
                //    throw new ArgumentOutOfRangeException("The value must be positive");
                //}
                if (!Validator.IsMoneyValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                salary = value;
            }
        }

        public int? TeamId { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }

        public virtual Team Team { get; set; }
    }
}
