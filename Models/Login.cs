using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class Login
    {

        [DisplayName("Role Type")]
        [Required(ErrorMessage = "Login type should not be blank eg. Manager / User")]
        [StringLength(30, MinimumLength = 5)]
        public string RoleTypeName { get; set; }

        [Required]
        [Key]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "ID must be 8 digits long")]
        [RegularExpression(@"^\w{8}$", ErrorMessage = "Please use only letters and digits without any spaces")]
        [Display(Name = "User Login")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Password can`t be blank")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")]
        [Display(Name = "Password")]
        public string Password { get; set; }

    



    }
}
