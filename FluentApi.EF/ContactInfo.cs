namespace FluentApi.EF
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// The ContactInfo class.
    /// </summary>
    [Table("ContactInfos")]
    public partial class ContactInfo
    {
        /// <summary>
        /// The phoneNumber value is stored here.
        /// </summary>
        private string phone;
        /// <summary>
        /// The e-Mail value is stored here.
        /// </summary>
        private string email;
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the e-Mail value. Validates it if set. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided.</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Throws when the value is under 10 characters.</exception>
        ///// <exception cref="FormatException">Throws when the mail doesnt end with: .com .net or .dk.</exception>
        ///// <exception cref="FormatException">Throws when teh mail doesnt have anything between the @ and the domain ending.</exception>
        //// <exception cref="FormatException">Throws when the mail doesnt contain a @.</exception>
        [StringLength(100)]
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                //if (value.Length < 10)
                //{
                //    throw new ArgumentOutOfRangeException("The Mail must be over 10 characters.");
                //}
                //if (!value.EndsWith(".com") && !value.EndsWith(".net") && !value.EndsWith(".dk"))
                //{
                //    throw new FormatException("The mail must end with .com .net or .dk");
                //}
                //if (value.EndsWith("@.com") || value.EndsWith("@.net") || value.EndsWith("@.dk"))
                //{
                //    throw new FormatException("You must write something between the @ and the domain ending");
                //}
                //if (!value.Contains("@"))
                //{
                //    throw new FormatException("There must be an at (@) symbol in the mail.");
                //}
                if (!Validator.IsEmailValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                email = value;
            }
        }

        /// <summary>
        /// Gets or sets the phone value. Validates it if set. Returns an exception if something is wrong.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown when an invalid value is provided.</exception>
        ///// <exception cref="FormatException">Throws when any character in the value is not a number.</exception>
        ///// <exception cref="ArgumentOutOfRangeException">Thrown when the value is under 8 characters long.</exception>
        [StringLength(25)]
        public string Phone
        {
            get
            {
                return phone;
            }
            set
            {
                //if (!value.All(Char.IsNumber))
                //{
                //    throw new FormatException("The value must be a number");
                //}
                //if (value.Length < 8)
                //{
                //    throw new ArgumentOutOfRangeException("The value must be atleast 8 numbers long");
                //}
                if (!Validator.IsPhoneValid(value))
                {
                    throw new ArgumentException("Invalid value provided");
                }
                phone = value;
            }
        }

        public virtual Employee Employee { get; set; }
    }
}
