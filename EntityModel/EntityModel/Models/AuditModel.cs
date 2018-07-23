using System.Collections.Generic;

namespace EntityModel
{
    public class AuditModel
    {
        IEnumerable<AuditItem> AuditList { get; set; }
    }
}
