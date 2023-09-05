using BLL.Dto;
using BLL.Services.Interfaces;
using ClosedXML.Excel;
using DataAccess;
using DataAccess.Entities;
using DocumentFormat.OpenXml.InkML;

namespace BLL.Services.Realizations
{
    public class ExcelService : IExcelService
    {
        public async Task<List<TransactionDto>> LoadDataFromExcel(Stream file)
        {
            using var workbook = new XLWorkbook(file);
            var worksheet = workbook.Worksheets.Worksheet(1);

            var column = worksheet.Columns().CellsUsed().ToList();

            var transactions = new List<TransactionDto>();

            for (int i = 5; i < column.Count; i += 5)
            {
                var id = int.Parse(column[i].Value.ToString());
                var status = column[i + 1].Value.ToString();
                var type = column[i + 2].Value.ToString();
                var clientName = column[i + 3].Value.ToString();
                var amount = decimal.Parse(column[i + 4].Value.ToString()[1..]);
                transactions.Add(new TransactionDto
                {
                    Amount = amount,
                    Id = id,
                    ClientName = clientName,
                    Status = status,
                    Type = type,
                });
            }

            return transactions;
        }
    }
}
