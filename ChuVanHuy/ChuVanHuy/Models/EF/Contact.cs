using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ChuVanHuy.Models.EF
{
    [Table("tb_Contact")]
    public class Contact : CommonAbstract
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(ErrorMessage ="Không được để trống thông tin")]
        [StringLength(150,ErrorMessage ="Không được vượt quá 150 ký tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(150, ErrorMessage = "Không được vượt quá 150 ký tự")]
        public string Email { get; set; }
        public string Website { get; set; }
        public string Message { get; set; }
        public int IsRead { get; set; }
    }
}