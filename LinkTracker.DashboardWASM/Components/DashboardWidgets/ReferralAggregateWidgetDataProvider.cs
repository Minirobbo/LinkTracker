using LinkTracker.DashboardWASM.Services;
using LinkTracker.Shared.Extensions;
using LinkTracker.Shared.Models;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.TimeSeriesChartSeries;

namespace LinkTracker.DashboardWASM.Components.DashboardWidgets
{
    public sealed record ReferralCount(string Label, double Count);

    public class ReferralAggregateWidgetDataProvider(IFetchAnalytics analytics, AnalyticsQuery query) 
        : IWidgetDataProvider<Visit, ReferralCount>
    {
        public AnalyticsQuery Query { get; set; } = query;

        public async Task<IEnumerable<Visit>> GetData()
            => await analytics.GetVisitsAsync(Query);

        public IEnumerable<ReferralCount> ConvertData(IEnumerable<Visit> visits)
        {
            return visits.GroupBy(v => v.ReferralId)
                          .OrderBy(v => v.Key)
                          .Select(v => new ReferralCount(v.Key ?? "None", v.Count()));
        }
    }
}
