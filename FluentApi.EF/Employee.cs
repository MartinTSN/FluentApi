namespace FluentApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Employee
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string Name
        {
            get
            {
                return Name;
            }
            set
            {
                if (value == null || value == "")
                {
                    throw new ArgumentNullException("The name must be set.");
                }
                if (value.Length > 100 || value.Length < 2)
                {
                    throw new ArgumentOutOfRangeException("The name must be above 2 chars and under 100.");
                }
                Name = value;
            }
        }

        public DateTime EmploymentDate
        {
            get
            {
                return EmploymentDate;
            }
            set
            {
                if (value.Date == null)
                {
                    throw new ArgumentNullException("The date must be set");
                }
                EmploymentDate = value;
            }
        }

        public int? TeamId { get; set; }

        public virtual ContactInfo ContactInfo { get; set; }

        public virtual Team Team { get; set; }
    }
}
