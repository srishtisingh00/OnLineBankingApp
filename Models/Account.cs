using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace OnLineBankingApp.Models
{
  

    
    public class Account
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "AccountId")]
        public int AccountId{ get; set; }

       
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]

        
        public int Id { get; set; }
        [ForeignKey("Id")]

        [Required]
        [Display(Name = "Current Balance")]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Date of Opening")]
        public DateTime DateOfOpening { get; set; }


             
        public virtual Customer Customer { get; set; }
        public virtual AccountType AccountType { get; set; }

        public virtual ICollection<ChequeBook> ChequeBooks { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }



    }
}
