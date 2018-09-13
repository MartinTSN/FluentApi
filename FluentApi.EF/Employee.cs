namespace FluentApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        private string firstName;
        private string lastName;
        private DateTime birthDay;
        private DateTime employementDate;
        private string cprNumber;
        private decimal salary;
        public int Id { get; set; }

        [StringLength(100)]
        public string FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The Firstname must be set");
                }
                if (value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("The Firstname must be over 2 characters");
                }
                else
                {
                    firstName = value;
                }

            }
        }

        [StringLength(100)]
        public string LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The Lastname must be set");
                }
                if (value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("The Lastname must be over 2 characters");
                }
                else
                {
                    lastName = value;
                }
            }
        }

        [Column(TypeName = "date")]
        public DateTime BirthDay
        {
            get
            {
                return birthDay;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The birthDay must be set.");
                }
                if (value.Year < (DateTime.Now.Year - 18))
                {
                    throw new ArgumentOutOfRangeException("The date must be atleast 18 years old.");
                }
                if (value.Year > (DateTime.Now.Year - 70))
                {
                    throw new ArgumentOutOfRangeException("The date must be under 70 years ago.");
                }
                birthDay = value;
            }
        }

        [Column(TypeName = "date")]
        public DateTime EmploymentDate
        {
            get
            {
                return employementDate;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The date must be set");
                }
                //if (value.Year)
                //{

                //}
                employementDate = value;
            }
        }

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
                if (value == null)
                {
                    throw new ArgumentNullException("The value must be set.");
                }
                if (value.Substring(value.Length - 4))
                {

                }
            }
        }

        [Column(TypeName = "money")]
        public decimal Salary { get; set; }

        public int? TeamId { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }

        public virtual Team Team { get; set; }
    }
}
