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
        /// Takes the Email value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Throws when the value is under 10 characters.</exception>
        /// <exception cref="FormatException">Throws when the mail doesnt end with: .com .net or .dk</exception>
        /// <exception cref="FormatException">Throws when teh mail doesnt have anything between the @ and the domain ending.</exception>
        /// <exception cref="FormatException">Throws when the mail doesnt contain a @.</exception>
        [StringLength(100)]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (value.Length < 10)
                {
                    throw new ArgumentOutOfRangeException("The Mail must be over 10 characters.");
                }
                if (!value.EndsWith(".com") && !value.EndsWith(".net") && !value.EndsWith(".dk"))
                {
                    throw new FormatException("The mail must end with .com .net or .dk");
                }
                if (value.EndsWith("@.com") || value.EndsWith("@.net") || value.EndsWith("@.dk"))
                {
                    throw new FormatException("You must write something between the @ and the domain ending");
                }
                if (!value.Contains("@"))
                {
                    throw new FormatException("There must be an at (@) symbol in the mail.");
                }
                email = value;
            }
        }

        /// <summary>
        /// Takes the Phone value and validates it. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="FormatException">Throws when any character in the value is not a number.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the value is under 8 characters long.</exception>
        [StringLength(25)]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                if (!value.All(Char.IsNumber))
                {
                    throw new FormatException("The value must be a number");
                }
                if (value.Length < 8)
                {
                    throw new ArgumentOutOfRangeException("The value must be atleast 8 numbers long");
                }
                phone = value;
            }
        }

        public virtual Employee Employee { get; set; }
    }
}
