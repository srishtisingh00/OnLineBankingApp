using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnLineBankingApp.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }


       
        [Required]
        [Display(Name = "Account Id")]
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]

        
        [Required]
        [Display(Name = "Beneficiary Id")]
        public int BeneficiaryId { get; set; }
        [ForeignKey("BeneficiaryId")]

        [Display(Name = "Opening Balance")]
        public decimal OpeningBalance{ get; set; }
       
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be non-negative")]
        public decimal Amount { get; set; }

        [Display(Name = "Closing Balance")]
        public decimal ClosingBalance { get; set; }
        
        public int TraId { get; set; }
        [ForeignKey("TraId")]

       
        public int StatusID { get; set; }
        [ForeignKey("StatusID")]

        [MaxLength(255)]

        [Display(Name = "Description")]
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime Date { get; set; }

        //[Display(Name = "Status")]
        //public string Status { get ; set; }

        
        public virtual Beneficiary Beneficiary{ get; set; }
       
        public virtual Account Accounts { get; set; }
 
        public virtual Status Status{ get; set; }
        public virtual TransactionType TransactionType { get; set; }
    }

}
