using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class TransactionType
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(60)]
        [Column(TypeName = "varchar(60)")]
        public string Type { get; set; } = default!;
    }
}
