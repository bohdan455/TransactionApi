using System.ComponentModel.DataAnnotations;

namespace TransactionApi.Model
{
    public class CSVFileModel
    {
        [Required]
        public List<int> Columns { get; set; } = default!;

        public List<string>? Types { get; set; }

        public string? Status { get; set; }
    }
}
