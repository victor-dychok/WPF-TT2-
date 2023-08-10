using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVM_DB_B1.Data.Models
{
    public class BalanceSheetDB
    {
        public int Id { get; set; }
        public int BalanceId { get; set; }
        public long OpeningBalanceAssets { get; set; }
        public long OpeningBalancePassive { get; set; }
        public long Debit { get; set; }
        public long Credit { get; set; }
        public long ClosingBalanceAssets { get; set; }
        public long ClosingBalancePassive { get; set; }

        public override string ToString()
        {
            return $"{BalanceId} : {OpeningBalanceAssets} | {OpeningBalancePassive} # {Debit} | {Credit} # {ClosingBalanceAssets} | {ClosingBalancePassive}";
        }
    }
}
