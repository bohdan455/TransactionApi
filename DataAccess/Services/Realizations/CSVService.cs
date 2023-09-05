using BLL.Services.Interfaces;
using DataAccess;
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
        private readonly ApplicationContext _context;

        public CSVService(ApplicationContext context)
        {
            _context = context;
        }

        public Task<Stream> GetCsvFileStream(string userId)
        {
            var transactions = _context.Transactions.Where(t => t.UserId == userId).Include(t => t.Status).Include(t => t.Type).ToList();
            var csv = new StringBuilder();
            csv.AppendLine("Id,ClientName,Amount,Type,Status");
            foreach (var transaction in transactions)
            {
                csv.AppendLine($"{transaction.Id},{transaction.ClientName},${transaction.Amount},{transaction.Type.Type},{transaction.Status.Status}");
            }

            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(csv.ToString()));
            return Task.FromResult(stream);
        }
    }
}
