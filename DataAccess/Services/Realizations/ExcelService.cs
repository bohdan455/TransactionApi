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
        private readonly ApplicationContext _context;

        public ExcelService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task LoadFile(Stream file, string userId)
        {
            var clientTransactions = await LoadDataFromExcel(file);

            using var dbTransaction = _context.Database.BeginTransaction();

            foreach(var clientTransaction in clientTransactions)
            {
                var newCLientTransaction = new Transaction
                {
                    Amount = clientTransaction.Amount,
                    ClientName = clientTransaction.ClientName,
                    StatusId = await GetOrCreateTransactionStatus(clientTransaction.Status),
                    TypeId = await GetOrCreateTransactionType(clientTransaction.Type),
                    UserId = userId,
                    Id = clientTransaction.Id,
                };

                if(_context.Transactions.Any(t => t.Id == newCLientTransaction.Id))
                {
                    _context.Transactions.Update(newCLientTransaction);
                }
                else
                {
                    _context.Transactions.Add(newCLientTransaction);
                }

                await _context.SaveChangesAsync();
            }
            await dbTransaction.CommitAsync();

        }

        private async Task<int> GetOrCreateTransactionStatus(string transactionStatus)
        {
            var status = _context.TransactionStatuses.FirstOrDefault(ts => ts.Status == transactionStatus);
            if (status == null)
            {
                status = new TransactionStatus
                {
                    Status = transactionStatus,
                };
                _context.TransactionStatuses.Add(status);

                await _context.SaveChangesAsync();
            }

            return status.Id;
        }

        private async Task<int> GetOrCreateTransactionType(string transactionType)
        {
            var type = _context.TransactionTypes.FirstOrDefault(ts => ts.Type == transactionType);
            if (type == null)
            {
                type = new TransactionType
                {
                    Type = transactionType,
                };
                _context.TransactionTypes.Add(type);

                await _context.SaveChangesAsync();
            }

            return type.Id;
        }

        private async Task<List<TransactionDto>> LoadDataFromExcel(Stream file)
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
                    Type = status,
                });
            }

            return transactions;
        }
    }
}
