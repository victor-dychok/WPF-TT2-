using Microsoft.EntityFrameworkCore;
using MVVM_DB_B1.Data.Interfaces;
using MVVM_DB_B1.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_DB_B1.Data.Repository
{
    class DatabaseRepository : IBalanceSheets
    {
        private DBContext _context;
        public DatabaseRepository(DBContext context)
        {
            _context = context;
        }

        public IEnumerable<BalanceSheetDB> GetAllBalanceSheets => _context.BalanceSheets;//.Include(c => c.Class).Include(f => f.FileName);

        public IEnumerable<Class> GetAllClasses => _context.Classes;

        public IEnumerable<FileName> GetAllFiles => _context.FileNames;

        public void AddBalanceSheet(BalanceSheetDB balanceSheet) => _context.BalanceSheets.Add(balanceSheet);

        public void AddClass(Class clas) => _context.Classes.Add(clas);

        public void AddFile(FileName file) => _context.FileNames.Add(file);

        public BalanceSheetDB GetBalanceById(int id) => _context.BalanceSheets.FirstOrDefault(s => s.BalanceId == id);

        public Class GetClassById(int id) => _context.Classes.FirstOrDefault(s => s.Id == id);

        public FileName GetFileNameById(int id) => _context.FileNames.FirstOrDefault(s => s.Id == id);

        public void SaveChanges() => _context.SaveChanges();
    }
}
