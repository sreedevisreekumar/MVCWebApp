using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCATM.Models
{
   public interface IRepository
    {
         int GetCountCheckingAccounts();
        List<CheckingAccount> GetCheckingAccounts();
        List<Transaction> GetTransactions();
        CheckingAccount GetCheckingAccountById(int id);
        CheckingAccount GetAccountByNumber(string accountNumber);
        CheckingAccount GetAccountByUserId(string userId);
        CheckingAccount CreateCheckingAccount(CheckingAccount checkingAccount);
        TransactionStatus GetStatusById(int id);
        Transaction AddTransaction(Transaction transaction);
        Transaction SaveTransaction(Transaction transaction);
        CheckingAccount AddCheckingAccount(CheckingAccount checkingAccount);
        CheckingAccount SaveCheckingAccount(CheckingAccount checkingAccount);
        TransactionStatus AddTransactionStatus(TransactionStatus transactionStatus);
       TransactionStatus SaveTransactionStatus(TransactionStatus transactionStatus);
        List<Transaction> GetTransactionsByCheckingAccount(int checkingAccountId);
    }
}
