using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int Id { get; set; }

        public TransactionStatus Status { get; set; }

        public int StatusId { get; set; }

        public TransactionType Type { get; set; }

        public int TypeId { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string ClientName { get; set; } = default!;

        [Column(TypeName = "decimal(8,2)")]
        public decimal Amount { get; set; }

        public User User { get; set; }

        public string UserId { get; set; }
    }
}
