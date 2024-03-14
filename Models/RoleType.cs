using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OnLineBankingApp.Models
{
    public class RoleType
    {
       
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int RoleTypeId { get; set; }

            [DisplayName("Role Type")]
            [Required(ErrorMessage = "Login type should not be blank eg. Manager / User")]
            [StringLength(30, MinimumLength = 5)]
            public string RoleTypeName { get; set; }

            public virtual ICollection<Customer> Customer { get; set; }

        
    }
}
