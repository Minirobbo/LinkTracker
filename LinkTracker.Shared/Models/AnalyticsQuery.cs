using System.Globalization;

namespace LinkTracker.Shared.Models
{
    public class AnalyticsQuery
    {
        private AnalyticsFilter[] _filters = [];
        private bool _ascending = true;

        public AnalyticsQuery() { }
        private AnalyticsQuery(AnalyticsFilter[] filters, bool ascending) => (_filters, _ascending) = (filters, ascending);

        public AnalyticsQuery Where(AnalyticsFilter other) => new([.. _filters, other], _ascending);

        public bool HasQuery => _filters.Length > 0;
        public string QueryClause => HasQuery ? $"?{string.Join("&", _filters.Select(f => f.Filter))}" : string.Empty;

        public static implicit operator AnalyticsQuery(AnalyticsFilter filter) => new([filter], true);

        public IEnumerable<Visit> ApplyOptions(IEnumerable<Visit> visits)
        {
            foreach (AnalyticsFilter filter in _filters)
            {
                visits = visits.Where(filter.Condition);
            }
            return visits;
        }

        public bool TryGetFirstValueForProperty(string key, out string value)
        {
            foreach (AnalyticsFilter filter in _filters)
            {
                if (filter.Property == key)
                {
                    value = filter.Value;
                    return true;
                }
            }

            value = string.Empty;
            return false;
        }
    }

    public class AnalyticsFilter
    {
        internal string Property;
        internal string Value;
        internal string Filter => $"{Property}={Value}";
        internal Func<Visit, bool> Condition;

        private AnalyticsFilter(string filterKey, string value, Func<Visit, bool> predicate) => (Property, Value, Condition) = (filterKey, value, predicate);

        public static AnalyticsFilter Filename(string filename) => new("fileName", Uri.EscapeDataString(filename), v => v.Filename == filename);
        public static AnalyticsFilter Referral(string referral) => new("referral", Uri.EscapeDataString(referral), v => v.ReferralId == referral);
        public static AnalyticsFilter StartDateTime(DateTime startDateTime) => new("startTime", Uri.EscapeDataString(startDateTime.ToString("O")), v => v.UtcTime >= startDateTime);
        public static AnalyticsFilter EndDateTime(DateTime endDateTime) => new("endTime", Uri.EscapeDataString(endDateTime.ToString("O")), v => v.UtcTime <= endDateTime);
    }
}
