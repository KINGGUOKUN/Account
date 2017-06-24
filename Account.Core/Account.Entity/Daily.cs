using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Account.Entity
{
    [Table("Daily")]
    public class Daily
    {
        [Key]
        [StringLength(36)]
        public string ID { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime Date { get; set; }

        public decimal Cost { get; set; }
    }
}
