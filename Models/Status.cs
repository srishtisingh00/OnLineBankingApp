using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusID { get; set; }
        [DisplayName("Status Type")]
        [Required(ErrorMessage = "Status Type should not be blank")]
      
        public string StatusType { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<ChequeBook> ChequeBooks { get; set; }
        
    }
}
