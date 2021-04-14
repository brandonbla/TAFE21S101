using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StartFinance.Models
{
    class Person
    {
        [PrimaryKey, AutoIncrement]
        public int PersonalID { get; set; }
        [NotNull]
        public string PersonalFirstName { get; set; }
        [NotNull]
        public string PersonalLastName { get; set; }
        [NotNull]
        public string DOB { get; set; }
        [NotNull]
        public string Gender { get; set; }
        [NotNull]
        public string PersonalEmail { get; set; }

        [NotNull, Phone]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="Please provide a phone number")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string PersonalPhone { get; set; }
    }
}
