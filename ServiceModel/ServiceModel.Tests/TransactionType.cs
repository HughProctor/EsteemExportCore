using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceModel.Test
{
    public enum TransactionType
    {
        NewItemEsteem,
        AssetTagChange,
        LocationChange,
        Deployed,
        Returned
    }

    public class Transaction
    {
        public TransactionType TransactionType { get; set; }
    }
}
