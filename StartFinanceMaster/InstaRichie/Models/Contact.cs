using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    class Contact
    {
        [PrimaryKey, AutoIncrement]
        public int ContactID { get; set; }
        [NotNull]
        public string ContactFirstName { get; set; }
        [NotNull]
        public string ContactLastName { get; set; }
        [NotNull]
        public string ContactCompany { get; set; }
        [NotNull, Phone]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Please provide a phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string ContactPhone { get; set; }
    }
}
