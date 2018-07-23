using System;

namespace EntityModel.Service
{
    public class QueryBuilder
    {
        public string QueryString { get; set; }
        public string LastQueryString { get; set; }
        public bool UseQueryBuilder { get; set; } = true;
        public int PageCount { get; set; } = 100;
        public int SkipCount { get; set; } = 0;
        public DateTime StartDate { get; set; }
        public string StartDateString { set { DateTime.TryParse(value, out var _date); StartDate = _date; } }
        private DateTime _endDate;
        public DateTime EndDate {
            get { return _endDate; }
            set {
                if (EndDateInclusive)
                    _endDate = value.AddDays(1);
                else _endDate = value;
            }
        }
        public bool EndDateInclusive { get; set; } = true;
        public string EndDateString { set { DateTime.TryParse(value, out var _date); EndDate = _date; } }
        public int DateRange { get; set; }
        public int TimeRange { get; set; }
        public string WhereExpression { get; set; }
        public string OrderBy { get; set; }

        public void Reset()
        {
            QueryString = WhereExpression = OrderBy = string.Empty;
            UseQueryBuilder = true;
            PageCount = 100;
            SkipCount = DateRange = TimeRange = 0;
            StartDate = EndDate = new DateTime();
            EndDateInclusive = true;
        }
    }
}