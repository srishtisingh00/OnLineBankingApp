using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Runtime.CompilerServices;

namespace OnLineBankingApp.Models
{
    public class Customer
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name ="Customer Id")] 
        public int CustomerID { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Only letters of the alphabet")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }

        
       
        [Display(Name = "Role Id")]
        public int RoleTypeId { get; set; }
        [ForeignKey("RoleTypeId")]

        [Required]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]

        public DateTime DateOfBirth { get; set; }


        [RegularExpression(@"^[A-Z]{5}\d{4}[A-Z]$", ErrorMessage = "Invalid PAN format")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "PAN should be 10 characters long")]
        [Display(Name = "PAN Number")]
        public string PAN { get; set; }

        [Required(ErrorMessage = "EmailI Id is required")]
        [Display(Name ="Email Id")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")]
        public string Email { get; set; }



        [MaxLength(50), RegularExpression(@"^\b[\dA-Za-z\s\-\\]+\b$", ErrorMessage = "Only letters and numbers")]
        public string Address { get; set; }

        [MaxLength(40), RegularExpression(@"^\b[A-Za-z\s]+$", ErrorMessage = "Please only use names.")]
        public string City { get; set; }

        //Must be 2 or 3 lettered Indian state abbreviation, e.g., KA, MH, TN
        [MaxLength(3)]
        [RegularExpression(@"(?i)\b(ap|ar|as|br|cg|ga|gj|hr|hp|jk|jh|ka|kl|mp|mh|mn|mz|nl|or|pb|rj|sk|tn|tg|tr|up|ut|wb|an|ch|dn|dd|ld|dl|py)\b", ErrorMessage = "Abbreviated names only")]
        public string State { get; set; }

        // Must be a 6 digit number
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Please enter a 6-digit PIN code")]
        public string PinCode { get; set; }

        private string _phone;

        //[RegularExpression(@"^\(91\)-\d{10}$", ErrorMessage = "Must be an Indian mobile number")]
        public string Phone
        {
            get
            {
                return _phone;
            }
            set
            {
                _phone = Regex.Replace(value, @"(\(91\)|\-|\s)", "");
            }
        }

        [NotMapped]
        public string DisplayPhoneNumber => _phone;


       
        //[ForeignKey("StatusID")]
        public int StatusID { get; set; }


        [Display(Name ="Password")]
        //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")]
        [Required(ErrorMessage = "Password can`t be blank")]
        public string Password { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Beneficiary> Beneficiaries { get; set; }
        public virtual RoleType RoleType { get; set; }
        public virtual Status Status { get; set; }
        public virtual Gender Gender { get; set; }

    }
}
