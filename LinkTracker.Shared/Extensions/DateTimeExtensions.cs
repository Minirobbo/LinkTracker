using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.Shared.Extensions
{
    public static class DateTimeExtensions
    {
        public record TimeCount(DateTime Time, int Count)
        {
            public TimeCount AddTime(TimeSpan span) => new(Time.Add(span), Count);
        }

        public static IEnumerable<TimeCount> FillMissingTimes(this IEnumerable<TimeCount> dateTimes, TimeSpan spanBetweenTimes)
        {
            using var times = dateTimes.GetEnumerator();
            if (times.MoveNext())
            {
                yield return times.Current;
                TimeCount last = times.Current;
                while (times.MoveNext())
                {
                    last = last.AddTime(spanBetweenTimes);
                    while (last.Time < times.Current.Time)
                    {
                        yield return new(last.Time, 0);
                        last = last.AddTime(spanBetweenTimes);
                    }
                    yield return times.Current;
                }
            }

            yield break;
        }

        public static DateTime Round(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + (span.Ticks / 2) + 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }
        public static DateTime Floor(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks / span.Ticks);
            return new DateTime(ticks * span.Ticks);
        }
        public static DateTime Ceil(this DateTime date, TimeSpan span)
        {
            long ticks = (date.Ticks + span.Ticks - 1) / span.Ticks;
            return new DateTime(ticks * span.Ticks);
        }
    }
}
