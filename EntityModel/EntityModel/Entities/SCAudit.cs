using System;

namespace EntityModel
{
    public class SCAudit
    {
        public string Audit_Ser_Num { get; set; }
        public string Audit_Prod_Num { get; set; }
        public string Audit_Part_Num { get; set; }
        public string Audit_Source_Site_Num { get; set; }
        public string Audit_Dest_Site_Num { get; set; }
        public string Audit_Type { get; set; }
        public string Audit_Rem { get; set; }
        public string Audit_User { get; set; }
        public DateTime Audit_Last_Update { get; set; }
        public DateTime Audit_Move_Date { get; set; }
    }
}
