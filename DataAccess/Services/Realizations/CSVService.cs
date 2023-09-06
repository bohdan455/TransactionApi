using BLL.Services.Interfaces;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Realizations
{
    public class CSVService : ICSVService
    {
        private readonly ITransactionService _transactionService;

        public CSVService(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        public async Task<Stream> GetCsvFileStream(string userId, List<int> columns, List<string>? types = null, string? transactionStatus = null)
        {
            var transactions = await _transactionService.GetTransactions(userId, types, transactionStatus);

            var csv = new StringBuilder();

            var hasId = columns.Contains(0);
            var hasClientName = columns.Contains(1);
            var hasStatus = columns.Contains(2);
            var hasType = columns.Contains(3);
            var hasAmount = columns.Contains(4);

            csv.AppendLine($"{(hasId ? "Id," : "")}{(hasClientName ? "ClientName," : "")}{(hasStatus ? "Status," : "")}{(hasType ? "Type," : "")}{(hasAmount ? "Amount" : "")}");
            foreach (var transaction in transactions)
            {
                var id = hasId ? $"{transaction.Id}," : "";
                var clientName = hasClientName ? $"{transaction.ClientName}," : "";
                var status = hasStatus ? $"{transaction.Status.Status}," : "";
                var type = hasType ? $"{transaction.Type.Type}," : "";
                var amount = hasAmount ? $"${transaction.Amount}" : "";
                
                csv.AppendLine($"{id}{clientName}{status}{type}{amount}");
            }

            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));
            return stream;
        }

    }
}
