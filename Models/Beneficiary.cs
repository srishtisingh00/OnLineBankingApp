using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnLineBankingApp.Models
{
    public class Beneficiary
    {
        private const string PHONE_NUMBER_FORMAT = "{0:(91)- #### ####}";

       
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BeneficiaryID { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$")]
        [Display(Name = "Beneficiary Name")]
        public string BeneficiaryName { get; set; }
            

        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]


        [Required]
        public double BeneficiaryAccountNumber {  get; set; }

        //private string _phone;
        [Required]
        public string Phone { get; set; }

        //[MaxLength(15)]
        //[RegularExpression(@"^\d{10}$")]
        //[Display(Name = "Phone")]
        //public string Phone
        //{
        //    get => _phone;
        //    set => _phone;//= string.Format(PHONE_NUMBER_FORMAT, Convert.ToInt64(value));
        //}

        [Required]
        [Display(Name = "Email Id")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$")]
        public string Email { get; set; }


        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$")]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }


        [Required]
        [MaxLength(50)]
        //* Rectify RegEx
       
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$")]
        [Display(Name = "IFSC Code")]
        public string IFSC { get; set; }

        [Required]
        [MaxLength(50)]
        [RegularExpression(@"^[A-Za-z\s]+$")]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual Customer Customer { get; set; }





    }
}
