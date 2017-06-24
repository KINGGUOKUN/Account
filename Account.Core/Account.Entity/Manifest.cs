using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account.Entity
{
    [Table("Manifest")]
    public class Manifest
    {
        [Key]
        [StringLength(36)]
        public string ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "请填写消费明细")]
        [MaxLength(100, ErrorMessage = "消费明细最大长度100")]
        public string Remark { get; set; }
    }
}
