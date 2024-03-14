using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class Gender
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GenderId { get; set; }
        [DisplayName("Gender Type")]
        [Required(ErrorMessage = "Gender Type should not be blank")]

        public string GenderType { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
