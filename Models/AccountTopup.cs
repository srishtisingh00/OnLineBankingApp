using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class AccountTopup
    {
       
        [Display(Name = "AccountId")]
        public int AccountId { get; set; }

        [Required]
        public int CustomerID { get; set; }


        [Required]
        public int Id { get; set; }
       

        [Required]
        [Display(Name = "Current Balance")]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Add to Amount")]
        public decimal AddAmount { get; set; }

        [Required]
        [Display(Name = "Date of Opening")]
        public DateTime DateOfOpening { get; set; }




    }
}
