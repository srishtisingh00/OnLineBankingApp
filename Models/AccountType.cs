using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnLineBankingApp.Models
{
    public class AccountType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [DisplayName("Account Type")]
        [Required(ErrorMessage = "Category Name should not be blank")]
       
        public string AccountTypeName { get; set; }

        public virtual ICollection<Account> Accounts{ get; set; }

    }
}
