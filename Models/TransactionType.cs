using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnLineBankingApp.Models
{
    public class TransactionType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TraId { get; set; }

        [DisplayName("Transation Name")]
        [Required(ErrorMessage = "Transaction Type should not be blank")]
        
        public string TransactionName { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
