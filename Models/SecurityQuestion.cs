using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class SecurityQuestion
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SqId { get; set; }

        [DisplayName("Select security question")]

        [Required(ErrorMessage = "Security Question should not be blank")]
        [Column("Question", TypeName = "nvarchar(100)")]
        public string Question { get; set; }
        public virtual ICollection<Login> Login{ get; set; }
    }
}
