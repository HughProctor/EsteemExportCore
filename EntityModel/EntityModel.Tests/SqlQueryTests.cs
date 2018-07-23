using EntityModel.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntityModel.Test
{
    [TestClass]
    public class SqlQueryTests
    {
        SCAuditService _sCAuditService;

        [TestInitialize]
        public void Startup()
        {
            _sCAuditService = new SCAuditService();
        }

        [TestMethod]
        public void Query_builder_contains_page_count()
        {
            _sCAuditService.PageCount = 10;
            var queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("TOP 10 "), "Query string builder doesn't contain PageCount");

            _sCAuditService.PageCount = 0;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(!queryString.Contains("TOP 10 "), "Query string builder does contain PageCount");
        }

        [TestMethod]
        public void Query_builder_contains_where_with_date_start_and_end()
        {
            var startDateTimeString = "";
            var endDateTimeString = "";
            var queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(!queryString.Contains("WHERE "), "Query string should not contain WHERE clause");

            startDateTimeString = "10/06/2017";
            DateTime.TryParse(startDateTimeString, out var startDateTime);
            _sCAuditService.StartDate = startDateTime;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should not contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '"+ startDateTimeString +""), "Query string does not contain correct start date: " + queryString);

            endDateTimeString = "12/06/2017";
            DateTime.TryParse(endDateTimeString, out var endDateTime);
            _sCAuditService.StartDate = startDateTime;

            //---------------------------- NOTE:::: EndDateInclusive FALSE :::: has to be set before EndDate :::::::::::: ------------//
            _sCAuditService.EndDateInclusive = false;
            _sCAuditService.EndDate = endDateTime;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTime.Date + ""), "Query string does not contain correct end date: " + queryString);

            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.EndDate = endDateTime;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTime.AddDays(1).Date + ""), "Query string does not contain correct end date: " + queryString);
        }

        [TestMethod]
        public void Query_builder_contains_where_with_date_start_and_Range()
        {
            var startDateTimeString = "";
            var endDateTimeString = "";
            var queryString = "";

            startDateTimeString = "10/06/2017";
            endDateTimeString = "20/06/2017";
            DateTime.TryParse(startDateTimeString, out var startDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.DateRange = 10;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);

            endDateTimeString = "30/06/2017";
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.DateRange = 20;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);

            endDateTimeString = "10/07/2017";
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.DateRange = 30;
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);
        }

        [TestMethod]
        public void Query_builder_contains_where_with_date_start_and_end_time()
        {
            var startDateTimeString = "";
            var endDateTimeString = "";
            var queryString = "";

            startDateTimeString = "10/06/2017";
            DateTime.TryParse(startDateTimeString, out var startDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.TimeRange = 1;
            endDateTimeString = startDateTime.AddHours(1).ToString();
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);

            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.TimeRange = 6;
            endDateTimeString = startDateTime.AddHours(6).ToString();
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);

            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.TimeRange = 24;
            endDateTimeString = startDateTime.AddHours(24).ToString();
            queryString = _sCAuditService.QueryBuilder();
            Assert.IsTrue(queryString.Contains("WHERE "), "Query string should contain WHERE clause");
            Assert.IsTrue(queryString.Contains(">= CONVERT(datetime, '" + startDateTimeString + ""), "Query string should not contain correct start date: " + queryString);
            Assert.IsTrue(queryString.Contains("<= CONVERT(datetime, '" + endDateTimeString + ""), "Query string should not contain correct end date: " + queryString);
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_100_records_if_not_overriden()
        {
            var returnList = new List<SCAudit>();
            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count == 100, "ReturnList didn't return default 100 records : " + returnList.Count.ToString());
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_10_records_if_PageCount_overriden()
        {
            var returnList = new List<SCAudit>();
            _sCAuditService.PageCount = 10;
            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count == 10, "ReturnList didn't return 10 records : " + returnList.Count.ToString());
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_50_records_if_PageCount_overriden()
        {
            var returnList = new List<SCAudit>();
            _sCAuditService.PageCount = 50;
            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count == 50, "ReturnList didn't return 50 records : " + returnList.Count.ToString());
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_50_records_if_DateTime_overriden()
        {
            var returnList = new List<SCAudit>();
            var startDateTimeString = "";
            startDateTimeString = "10/06/2017";
            DateTime.TryParse(startDateTimeString, out var startDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.PageCount = 10;

            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count == 10, "ReturnList didn't return 10 records : " + returnList.Count.ToString());
            foreach(var item in returnList)
                Assert.IsTrue(item.Audit_Last_Update.Date == startDateTime.Date, "ReturnList didn't return 10 records : " + returnList.First().Audit_Last_Update.ToString());
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_1000s_records_if_DateTime_EndDate_overriden_and_includes_enddate_data()
        {
            var returnList = new List<SCAudit>();
            var startDateTimeString = "";
            var endDateTimeString = "";

            startDateTimeString = "10/06/2017";
            endDateTimeString = "20/06/2017";

            DateTime.TryParse(startDateTimeString, out var startDateTime);
            DateTime.TryParse(endDateTimeString, out var endDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.EndDate = endDateTime;
            _sCAuditService.PageCount = 10000;

            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count > 1000, "ReturnList didn't return 10 records : " + returnList.Count.ToString());
            foreach (var item in returnList)
            {
                Assert.IsTrue(item.Audit_Last_Update.Date >= startDateTime.Date, "ReturnList didn't return 10 records : " + returnList.First().Audit_Last_Update.ToString());
                Assert.IsTrue(item.Audit_Last_Update.Date <= endDateTime.Date, "ReturnList didn't return 10 records : " + returnList.Last().Audit_Last_Update.ToString());
            }
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_100s_records_if_Date_Range()
        {
            var returnList = new List<SCAudit>();
            var startDateTimeString = "";

            startDateTimeString = "10/06/2017";

            DateTime.TryParse(startDateTimeString, out var startDateTime);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.DateRange = 1;
            _sCAuditService.PageCount = 100;

            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Count > 10, "ReturnList didn't return 10 records : " + returnList.Count.ToString());
            foreach (var item in returnList)
            {
                Assert.IsTrue(item.Audit_Last_Update.Date >= startDateTime.Date, "ReturnList didn't return 10 records : " + returnList.First().Audit_Last_Update.ToString());
                Assert.IsTrue(item.Audit_Last_Update.Date < startDateTime.AddDays(1).Date, "ReturnList didn't return 10 records : " + returnList.Last().Audit_Last_Update.ToString());
            }
        }

        [TestMethod]
        public void Query_builder_PageCount_returns_100s_records_if_Time_Range()
        {
            var returnList = new List<SCAudit>();
            var startDateTimeString = "";
            var year = 2017;
            var month = 6;
            var day = 10;
            var hour = 10;
            var minute = 20;
            var second = 25;

            startDateTimeString = "10/06/2017 " + hour + minute + second;// 10:05:00";
            var startDateTime = new DateTime(year, month, day, hour, minute, second);
            //DateTime.TryParse(startDateTimeString, out);
            _sCAuditService.StartDate = startDateTime;
            _sCAuditService.TimeRange = 1;
            _sCAuditService.PageCount = 100;

            returnList = _sCAuditService.GetAll();

            Assert.IsNotNull(returnList, "ReturnList returned null");
            Assert.IsTrue(returnList.Any(), "ReturnList didn't return 10 records : " + returnList.Count.ToString());
            foreach (var item in returnList)
            {
                Assert.IsTrue(item.Audit_Last_Update.Date >= startDateTime.Date, "ReturnList didn't return 10 records : " + returnList.First().Audit_Last_Update.ToString());
                Assert.IsTrue(item.Audit_Last_Update.Date < startDateTime.AddDays(1).Date, "ReturnList didn't return 10 records : " + returnList.Last().Audit_Last_Update.ToString());
                Assert.IsTrue(item.Audit_Last_Update.Hour == hour, "ReturnList didn't return 10 records : " + returnList.Last().Audit_Last_Update.ToString());
            }
        }
    }
}
