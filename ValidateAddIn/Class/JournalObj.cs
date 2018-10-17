using System;
using System.Collections.Generic;

namespace ValidateAddIn.Class
{
    public class JournalObj
    {
        public int SysId { get; set; }
        public int BusinessId { get; set; }
        public double JournalId { get; set; }
        public DateTime AccountingDate { get; set; }
        public string Description { get; set; }
        public int RecordNumbers { get; set; }

        public List<DetailObj> JournalDetail { get; set; }

    }

    public class DetailObj
    {
        public int? SysId { get; set; }

        public int? JournalId { get; set; }

        public int LineNumber { get; set; }

        public int BusinessUnit { get; set; }

        public string Ledger { get; set; }

        public string Account { get; set; }

        public string AltAccount { get; set; }

        public string OperatingUnit { get; set; }

        public string Department { get; set; }

        public string Product { get; set; }

        public string AccExpYr { get; set; }

        public string Mcc { get; set; }

        public string DistributionChannel { get; set; }

        public string Geocode { get; set; }

        public string Function { get; set; }

        public string Project { get; set; }

        public int YearOfAccount { get; set; }

        public string Affiliate { get; set; }

        public string TransactionCurrency { get; set; }

        public decimal TransactionAmount { get; set; }

        public string RateType { get; set; }

        public string RateMultiplier { get; set; }

        public string BaseAmount { get; set; }

        public string StatisticalAmount { get; set; }

        public string Reference { get; set; }

        public string JournalLineDescription { get; set; }
    }
}
