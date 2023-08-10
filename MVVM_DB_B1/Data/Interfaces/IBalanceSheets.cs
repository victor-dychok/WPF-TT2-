using MVVM_DB_B1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_DB_B1.Data.Interfaces
{
    public interface IBalanceSheets
    {
        IEnumerable<BalanceSheetDB> GetAllBalanceSheets { get; }
        IEnumerable<Class> GetAllClasses { get; }
        IEnumerable<FileName> GetAllFiles { get; }
        BalanceSheetDB GetBalanceById(int id);
        Class GetClassById(int id);
        FileName GetFileNameById(int id);
        void AddBalanceSheet(BalanceSheetDB balanceSheet);
        void AddClass(Class clas);
        void AddFile(FileName file);
        void SaveChanges();

    }
}
