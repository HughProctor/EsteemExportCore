namespace EntityModel.Test.Specifications
{
    /// <summary>
    /// Important to note the order in creating thes Specifications... 
    /// Combined Specifications MUST come below the Original single SPEC
    /// </summary>
    public static class BNL_SPEC
    {
        public static readonly ISpecification<SCAudit> SPEC_PART_IS_BNL = new Specification<SCAudit>(o => o.Audit_Part_Num.StartsWith("BNL"));
        public static readonly ISpecification<SCAudit> SPEC_NEW_PO = new Specification<SCAudit>(o => o.Audit_Rem.StartsWith("Added PO"));

        /// <summary>
        ///  SCSITE TABLE
        /// </summary>
        public static readonly ISpecification<SCAudit> SPEC_SITE_E = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("E"));
        public static readonly ISpecification<SCAudit> SPEC_STAGE_E = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"));
        public static readonly ISpecification<SCAudit> SPEC_STAGE_K = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("K-"));
        // Project Site
        public static readonly ISpecification<SCAudit> SPEC_STAGE_P = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("P-"));
        public static readonly ISpecification<SCAudit> SPEC_STAGE_T = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("T-"));
        public static readonly ISpecification<SCAudit> SPEC_STAGE_S = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("S-"));

        // Linetex Good
        public static readonly ISpecification<SCAudit> SPEC_DESTINATION_LTX = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX"));
        // Repair
        public static readonly ISpecification<SCAudit> SPEC_DESTINATION_LTXR = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("LTXR"));
        // Faulty
        public static readonly ISpecification<SCAudit> SPEC_DESTINATION_LTXF = new Specification<SCAudit>(o => o.Audit_Dest_Site_Num.ToUpper().StartsWith("LTXF"));

        /// --------------------------- These Must Come at end of File --------------------------------
        public static readonly ISpecification<SCAudit> SPEC_NEWITEMESTEEM = (SPEC_PART_IS_BNL.AND(SPEC_NEW_PO));
        public static readonly ISpecification<SCAudit> SPEC_STAGE_E_OR_LTX = (SPEC_STAGE_E.OR(SPEC_DESTINATION_LTX));

        public static readonly ISpecification<SCAudit> SPEC_NEWITEM = (SPEC_NEWITEMESTEEM.AND(SPEC_STAGE_E_OR_LTX));

    }
}
