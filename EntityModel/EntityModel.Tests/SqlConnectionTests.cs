using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using EntityModel.Connection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;

namespace EntityModel.Test
{
    [TestClass]
    public class SqlConnectionTests
    {
        [TestMethod]
        public void OpenConnection_Test()
        {
            // Prepare
            var connectionString = "Server=Tesseract;Database=LineTess;Uid=Hugh;Pwd=Pr0ct0r";
            var sqlDbConnection = new SqlDbConnection();
            var connection = sqlDbConnection.CreateConnection(connectionString);
            // Act
            // Assert
            Assert.IsTrue(sqlDbConnection.OpenConnection_Test(connectionString), "Failed to Open Database Connection");
            connection.Close();
        }

        [TestMethod]
        public void Inline_Linq_Query_To_DB()
        {
            var connectionString = "Server=Tesseract;Database=LineTess;Uid=Hugh;Pwd=Pr0ct0r";
            var sqlDbConnection = new SqlDbConnection();
            var connection = sqlDbConnection.CreateConnection(connectionString);
            // Act
            // Assert
            Assert.IsTrue(sqlDbConnection.OpenConnection_Test(connectionString), "Failed to Open Database Connection");
        }

        [TestMethod]
        public void Basic_select_query_to_DB()
        {
            var connectionString = "Server=Tesseract;Database=LineTess;Uid=Hugh;Pwd=Pr0ct0r";

            var queryString = "select TOP 10 " +
                "[Audit_Ser_Num], " +
                "[Audit_Part_Num], " +
                "[Audit_Source_Site_Num], " +
                "[Audit_Rem], " +
                "[Audit_User], " +
                "[Audit_Move_Date], " +
                "[Audit_Last_Update] " +
                "FROM scaudit (nolock) " +
                "WHERE audit_last_update >= '2017-06-01' AND audit_last_update <= '2017-06-30' " +
                "ORDER BY Audit_Move_date desc";
            var scAuditModel = new SCAuditModel();
            scAuditModel.SCAuditList = scAuditModel.SCAuditList ?? new List<SCAudit>();

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, sqlConnection);

                try
                {
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        scAuditModel.SCAuditList.Add(new SCAudit()
                        {
                            Audit_Dest_Site_Num = reader[0].ToString(),
                            Audit_Part_Num = reader[1].ToString(),
                            Audit_Source_Site_Num = reader[2].ToString(),
                            Audit_Rem = reader[3].ToString(),
                            Audit_User = reader[4].ToString(),
                            Audit_Move_Date = DateTime.Parse(reader[5].ToString()),
                            Audit_Last_Update = DateTime.Parse(reader[6].ToString())
                        });
                    }
                    reader.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            Assert.IsNotNull(scAuditModel.SCAuditList);
            Assert.IsTrue(scAuditModel.SCAuditList.Any());
            Assert.IsTrue(scAuditModel.SCAuditList.Count == 10);
            Assert.IsTrue(!string.IsNullOrEmpty(scAuditModel.SCAuditList.First().Audit_Part_Num));
        }

        [TestMethod]
        public void Get_connectionstring_from_config()
        {
            var tesseractConnectionString = "Tesseract_Test_ConnectionString";
            var connectionStringSection = ConfigurationManager.ConnectionStrings[tesseractConnectionString];
            Assert.IsNotNull(connectionStringSection, "App.Config ConnectionString settings section is null or empty");   
            var connectionString = connectionStringSection.ConnectionString;
            Assert.IsNotNull(connectionString, "ConnectionString section " + tesseractConnectionString + "is null or empty");
            var sqlDbConnection = new SqlDbConnection();
            var connection = sqlDbConnection.CreateConnection(connectionString);
            Assert.IsTrue(sqlDbConnection.OpenConnection_Test(connectionString), "Failed to Open Database Connection");
        }

        [TestMethod]
        public void Get_connectionstring_from_config2()
        {
            var sqlDbConnection = new SqlDbConnection();
            var connectionString = sqlDbConnection.GetConnectionString();
            Assert.IsTrue(sqlDbConnection.OpenConnection_Test(connectionString), "Failed to Open Database Connection");
        }
    }
}
