using DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountTransactionLogic.Helpers
{
    public class TransactionTypeHelper
    {
        public static TransactionTypeEnum GetType(decimal amount)
        {
            if (amount >= 0) return TransactionTypeEnum.Deposit;
            return TransactionTypeEnum.Withdrawl;
        }
    }
}
