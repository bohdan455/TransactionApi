using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.Services.Interfaces;
using DataAccess;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.Realizations
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationContext _context;
        private readonly IExcelService _excelService;

        public TransactionService(ApplicationContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task LoadDataFromExcel(Stream file, string userId)
        {
            var clientTransactions = await _excelService.LoadDataFromExcel(file);

            using var dbTransaction = _context.Database.BeginTransaction();

            foreach (var clientTransaction in clientTransactions)
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

                if (_context.Transactions.Any(t => t.Id == newCLientTransaction.Id))
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

        public async Task ChangeStatus(int transactionId, string userId, string newStatus)
        {
            var transaction = _context.Transactions.FirstOrDefault(t => t.Id == transactionId && t.UserId == userId)
                ?? throw new ArgumentException("Transaction not found");

            transaction.StatusId = await GetOrCreateTransactionStatus(newStatus);

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
        }   

        public async Task<List<Transaction>> GetTransactions(string userId, List<string>? types = null, string? transactionStatus = null)
        {
            IQueryable<Transaction> transactionsQuery = _context.Transactions.Where(t => t.UserId == userId).Include(t => t.Status).Include(t => t.Type);
            if (types != null)
            {
                transactionsQuery = transactionsQuery.Where(t => types.Contains(t.Type.Type));
            }

            if (transactionStatus != null)
            {
                transactionsQuery = transactionsQuery.Where(t => t.Status.Status == transactionStatus);
            }

            return await transactionsQuery.ToListAsync();
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
    }
}
