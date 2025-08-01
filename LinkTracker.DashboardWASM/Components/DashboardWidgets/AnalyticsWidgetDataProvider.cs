﻿using LinkTracker.DashboardWASM.Services;
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
    public class AnalyticsWidgetDataProvider(IFetchAnalytics analytics, AnalyticsQuery query, TimeSpan timespan) 
        : IWidgetDataProvider<Visit, TimeValue>
    {
        public TimeSpan Timespan { get; set; } = timespan;
        public AnalyticsQuery Query { get; set; } = query;

        public async Task<IEnumerable<Visit>> GetData()
            => await analytics.GetVisitsAsync(Query);

        public IEnumerable<TimeValue> ConvertData(IEnumerable<Visit> visits)
        {
            return visits.Select(v => v.UtcTime.Date.Floor(Timespan))
                          .GroupBy(v => v.Date)
                          .OrderBy(v => v.Key)
                          .Select(v => new DateTimeExtensions.TimeCount(v.Key, v.Count()))
                          .FillMissingTimes(Timespan)
                          .Select(g => new TimeSeriesChartSeries.TimeValue(g.Time, g.Count));
        }
    }
}
