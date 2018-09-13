namespace FluentApi.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;

    [Table("ContactInfos")]
    public partial class ContactInfo
    {
        private string phone;
        private string email;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(25)]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("The value must be set.");
                }
                if (!value.All(Char.IsNumber))
                {
                    throw new FormatException("The value must be a number");
                }
                if (value.Length <= 8)
                {
                    throw new ArgumentOutOfRangeException("The value must be atleast 8 numbers long");
                }
            }
        }

        public virtual Employee Employee { get; set; }
    }
}
