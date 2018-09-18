namespace FluentApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    public partial class Team
    {
        private string name;
        private string description;
        private DateTime startDate;
        private DateTime expectedEndDate;
        private decimal budget;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Team()
        {
            Employees = new HashSet<Employee>();
        }

        public int Id { get; set; }

        /// <summary>
        /// Takes the Name value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided</exception>
        ///// <exception cref="ArgumentNullException">Throws if the value is null.</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Thrown when the value is under 2 or over 50 characters.</exception>
        ///// <exception cref="FormatException">Thrown when any character is a number.</exception>
        [Required]
        [StringLength(100)]
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The Name must be set");
                //}
                //if (value.Length < 2 || value.Length > 100)
                //{
                //    throw new ArgumentOutOfRangeException("The Name must be over 2 and under 100 characters");
                //}
                //if (value.All(Char.IsNumber))
                //{
                //    throw new FormatException("There cant be any numbers in the value");
                //}
                if (!Validator.IsNameValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                name = value;
            }
        }

        /// <summary>
        /// Takes the Description value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided</exception>
        ///// <exception cref="ArgumentNullException">Throws when the value is null.</exception>
        ///// <exception cref="FormatException">Thrown when any character in the value is a number.</exception>
        [Required]
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The value must be set");
                //}
                //if (value.All(Char.IsNumber))
                //{
                //    throw new FormatException("The value must not have any numbers");
                //}
                if (!Validator.IsDescriptionValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                description = value;
            }
        }

        /// <summary>
        /// Takes the StartDate value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided</exception>
        ///// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Thrown when the value is in the future.</exception>
        [Column(TypeName = "date")]
        public DateTime StartDate
        {
            get
            {
                return startDate;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The value must be set");
                //}
                //if (value.Date > DateTime.Now.Date)
                //{
                //    throw new ArgumentOutOfRangeException("The value must not be in the future");
                //}
                if (!Validator.IsStartDateValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                startDate = value;
            }
        }

        /// <summary>
        /// Takes the ExpectedEndDate value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided</exception>
        ///// <exception cref="ArgumentNullException">Thrown when the value is null.</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Thrown when the value is in the past.</exception>
        [Column(TypeName = "date")]
        public DateTime ExpectedEndDate
        {
            get
            {
                return expectedEndDate;
            }
            set
            {
                //if (value == null)
                //{
                //    throw new ArgumentNullException("The value must be set");
                //}
                //if (value.Date < DateTime.Now.Date)
                //{
                //    throw new ArgumentOutOfRangeException("The value must not be in the past");
                //}
                if (!Validator.IsEndDateValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                expectedEndDate = value;
            }
        }

        /// <summary>
        /// Takes the BudgetLimit value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Thrown when the value is negative.</exception>
        [Column(TypeName = "money")]
        public decimal Budget
        {
            get
            {
                return budget;
            }
            set
            {
                if (!Validator.IsMoneyValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                budget = value;
            }
        }

        public int? ProjectId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }

        public virtual Project Project { get; set; }
    }
}

