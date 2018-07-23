using EntityModel.Service;
using EntityModel.Test.FileExort;
using EntityModel.Test.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EntityModel.Test
{
    [TestClass]
    public class ExportDataTests
    {
        SCAuditService _sCAuditService;

        [TestInitialize]
        public void Startup()
        {
            _sCAuditService = new SCAuditService();
        }

        private List<SCAudit> GetAll_BaseQuery()
        {
            var returnList = new List<SCAudit>();
            var startDateTimeString = "01/11/2017";
            var endDateTimeString = "30/11/2017";

            DateTime.TryParse(startDateTimeString, out var startDateTime);
            DateTime.TryParse(endDateTimeString, out var endDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.EndDate = endDateTime;
            _sCAuditService.PageCount = 10000;

            returnList = _sCAuditService.GetAll();
            return returnList;
        }

        [TestMethod]
        public void ExportToJson()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            JSON_FileExport.WriteFile("00_NEWITEM_ALL", returnList, returnList.Count);
        }

        [TestMethod]
        public void Specification_01_BNL_and_AddedPO()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var SPEC_NEWITEMESTEEM = (BNL_SPEC.SPEC_PART_IS_BNL.AND(BNL_SPEC.SPEC_NEW_PO));

            var newItemList = returnList.FindAll(x => SPEC_NEWITEMESTEEM.IsSatisfiedBy(x));

            var count = 0;
            foreach(var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Part_Num.StartsWith("BNL"), "Item No: " + count.ToString() + " doesn't StartWith BNL :" + item.Audit_Part_Num);
                Assert.IsTrue(item.Audit_Rem.StartsWith("Added PO"), "Item No: " + count.ToString() + " doesn't StartWith Added PO :" + item.Audit_Rem);
            }

            JSON_FileExport.WriteFile("01_BNL_AND_ADDEDPO", newItemList, newItemList.Count);
        }

        [TestMethod]
        public void Specification_02_STAGE_E_andor_LTX()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var SPEC_NEWITEMESTEEM = (BNL_SPEC.SPEC_PART_IS_BNL.AND(BNL_SPEC.SPEC_NEW_PO));
            var SPEC_STAGE_E_OR_LTX = (BNL_SPEC.SPEC_STAGE_E.OR(BNL_SPEC.SPEC_DESTINATION_LTX));

            var newItemList = returnList.FindAll(x => SPEC_STAGE_E_OR_LTX.IsSatisfiedBy(x));

            var count = 0;
            foreach (var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX") || item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"), 
                    "Item No: " + count.ToString() + " doesn't StartWith either LTX or E- :" + item.Audit_Dest_Site_Num);
            }

            JSON_FileExport.WriteFile("02_STAGE_E_ANDOR_LTX", newItemList, newItemList.Count);
        }

        [TestMethod]
        public void Specification_03_STAGE_E_andor_LTX_2()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var SPEC = BNL_SPEC.SPEC_STAGE_E_OR_LTX;
            Assert.IsNotNull(SPEC, "SPEC is null");
            var newItemList = returnList.FindAll(x => SPEC.IsSatisfiedBy(x));

            var count = 0;
            foreach (var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX") || item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"),
                    "Item No: " + count.ToString() + " doesn't StartWith either LTX or E- :" + item.Audit_Dest_Site_Num);
            }

            JSON_FileExport.WriteFile("03_STAGE_E_ANDOR_LTX_2", newItemList, newItemList.Count);
        }

        [TestMethod]
        public void Specification_04_BNL_AddedPO_STAGE_E_andor_LTX()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var SPEC = BNL_SPEC.SPEC_NEWITEM;

            var newItemList = returnList.FindAll(x => SPEC.IsSatisfiedBy(x));

            var count = 0;
            foreach (var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Part_Num.StartsWith("BNL"), "Item No: " + count.ToString() + " doesn't StartWith BNL :" + item.Audit_Part_Num);
                Assert.IsTrue(item.Audit_Rem.StartsWith("Added PO"), "Item No: " + count.ToString() + " doesn't StartWith Added PO :" + item.Audit_Rem);
                Assert.IsTrue(item.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX") || item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"),
                    "Item No: " + count.ToString() + " doesn't StartWith either LTX or E- :" + item.Audit_Dest_Site_Num);
            }

            JSON_FileExport.WriteFile("04_BNL_ADDEDPO_STAGE_E_ANDOR_LTX", newItemList, newItemList.Count);
        }

        [TestMethod]
        public void LinqQuery_05_STAGE_E_andor_LTX_3()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var newItemList = (from item in returnList
                               where item.Audit_Part_Num.ToUpper().StartsWith("BNL")
                                 && item.Audit_Rem.StartsWith("Added PO")
                                 && (item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-") || item.Audit_Dest_Site_Num.ToUpper() == "LTX")
                               select item).ToList();


            var count = 0;
            foreach (var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX") || item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"),
                    "Item No: " + count.ToString() + " doesn't StartWith either LTX or E- :" + item.Audit_Dest_Site_Num);
            }

            JSON_FileExport.WriteFile("02_STAGE_E_ANDOR_LTX", newItemList, newItemList.Count);
        }

        [TestMethod]
        public void LambdaQuery_06_STAGE_E_andor_LTX_4()
        {
            var returnList = GetAll_BaseQuery();
            Assert.IsTrue(returnList.Any(), "Query didn't return any results");

            var newItemList = returnList
                .Where
                (
                    item => item.Audit_Part_Num.ToUpper().StartsWith("BNL")
                        && item.Audit_Rem.StartsWith("Added PO")
                        && (item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-") || item.Audit_Dest_Site_Num.ToUpper() == "LTX")
                ).ToList();


            var count = 0;
            foreach (var item in newItemList)
            {
                count++;
                Assert.IsTrue(item.Audit_Dest_Site_Num.ToUpper().StartsWith("LTX") || item.Audit_Dest_Site_Num.ToUpper().StartsWith("E-"),
                    "Item No: " + count.ToString() + " doesn't StartWith either LTX or E- :" + item.Audit_Dest_Site_Num);
            }

            JSON_FileExport.WriteFile("02_STAGE_E_ANDOR_LTX", newItemList, newItemList.Count);
        }

    }
}
