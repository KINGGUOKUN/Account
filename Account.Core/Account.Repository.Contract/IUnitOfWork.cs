using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Account.Repository.Contract
{
    public interface IUnitOfWork
    {
        DbTransaction BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}
