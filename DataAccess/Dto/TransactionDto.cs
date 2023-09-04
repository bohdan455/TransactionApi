using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public string Type { get; set; } = default!;

        public string Status { get; set; } = default!;

        public string ClientName { get; set; } = default!;

        public decimal Amount { get; set; }
    }
}
