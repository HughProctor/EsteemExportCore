using EntityModel.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityModel.Service
{
    public class SCAuditService : QueryBuilder
    {
        SqlDbConnection _sqlDbConnection;
        string _connectionString;
        public string _orderBy = "ORDER BY [Audit_Move_Date] desc";

        public SCAuditService(string connectionString = null)
        {
            _sqlDbConnection = new SqlDbConnection();
        }

        public string QueryBuilder(string whereExpression = null)
        {
            var returnQueryString = "";

            QueryString = "SELECT " + (PageCount > 0 ? "TOP " + PageCount.ToString() + " " : "") +
                "NULLIF(LTRIM(RTRIM([Audit_Ser_Num])), '') AS [SERIAL_NO], " +
                "NULLIF(LTRIM(RTRIM([Audit_Part_Num])), '') AS [PART_NO], " +
                "NULLIF(LTRIM(RTRIM([Audit_Source_Site_Num])), '') AS [SOURCE_SITE_NO], " +
                "NULLIF(LTRIM(RTRIM([Audit_Dest_Site_Num])), '') AS [DESTINATION_SITE_NO], " +
                "NULLIF(LTRIM(RTRIM([Audit_Rem])), '') AS [REMARK], " +
                "NULLIF(LTRIM(RTRIM([Audit_User])), '') AS [USER], " +
                "[Audit_Move_Date] AS [MOVE_DATE], " +
                "[Audit_Last_Update] AS [UPDATE_DATE] " +
                "FROM scaudit (nolock) ";

            if (string.IsNullOrEmpty(WhereExpression))
                if (StartDate > DateTime.MinValue)
                {
                    if (TimeRange > 0)
                        WhereExpression = "WHERE audit_last_update >= CONVERT(datetime, '" + StartDate + "', 103) AND audit_last_update <= CONVERT(datetime, '" + StartDate.AddHours(TimeRange) + "', 103) ";
                    else if (DateRange > 0)
                        WhereExpression = "WHERE audit_last_update >= CONVERT(datetime, '" + StartDate + "', 103) AND audit_last_update <= CONVERT(datetime, '" + StartDate.AddDays(DateRange) + "', 103) ";
                    else if (EndDate > DateTime.MinValue)
                        WhereExpression = "WHERE audit_last_update >= CONVERT(datetime, '" + StartDate + "', 103) AND audit_last_update <= CONVERT(datetime, '" + EndDate + "', 103) ";
                    else
                        WhereExpression = "WHERE audit_last_update >= CONVERT(datetime, '" + StartDate + "', 103) AND audit_last_update <= CONVERT(datetime, '" + StartDate.AddDays(1) + "', 103) ";
                }
                else WhereExpression = "";

            OrderBy = !string.IsNullOrEmpty(OrderBy) ? OrderBy : _orderBy;

            LastQueryString = returnQueryString = QueryString + WhereExpression + OrderBy;
            // Need to reset the Where and Order by, otherwise they remain in memory for next request
            Reset();

            return returnQueryString;
        }


        public List<SCAudit> GetAll()
        {
            var scAuditModel = new SCAuditModel();

            using (var connection = _sqlDbConnection.CreateConnection(_connectionString))
            {
                var command = new SqlCommand(UseQueryBuilder ? QueryBuilder() : QueryString, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        scAuditModel.SCAuditList.Add(new SCAudit()
                        {
                            Audit_Ser_Num = reader["SERIAL_NO"].ToString(),
                            Audit_Part_Num = reader["PART_NO"].ToString(),
                            Audit_Source_Site_Num = reader["SOURCE_SITE_NO"].ToString(),
                            Audit_Dest_Site_Num = reader["DESTINATION_SITE_NO"].ToString(),                            
                            Audit_Rem = reader["REMARK"].ToString(),
                            Audit_User = reader["USER"].ToString(),
                            Audit_Move_Date = DateTime.Parse(reader["MOVE_DATE"].ToString()),
                            Audit_Last_Update = DateTime.Parse(reader["UPDATE_DATE"].ToString())
                        });
                    }
                    Reset();
                    reader.Close();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            return scAuditModel.SCAuditList;
        }
    }
}
