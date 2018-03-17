using Account.Repository.Contract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Account.Repository.EF
{
    public class EFUnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public EFUnitOfWork(DbContext context)
        {
            _context = context;
        }

        public DbTransaction BeginTransaction()
        {
            var transaction = _context.Database.GetDbConnection().BeginTransaction();
            _context.Database.UseTransaction(transaction);

            return transaction;
        }

        public void CommitTransaction()
        {
            _context.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            _context.Database.RollbackTransaction();
        }
    }
}
