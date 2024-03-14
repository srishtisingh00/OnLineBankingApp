using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnLineBankingApp.Models
{
    public class ChequeBook
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChqId { get; set; }

        
        public int AccountId { get; set; }
        [ForeignKey("AccountId")]

        [DisplayName("ChequeBook Status")]
        [Required(ErrorMessage = "ChequeBook Status  should not be blank")]

       
        public int StatusID { get; set; }
        [ForeignKey("StatusID")]

        public virtual Account Account { get; set; }

        public virtual Status Status { get; set; }



    }
}
