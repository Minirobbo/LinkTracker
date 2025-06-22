using LinkTracker.API.Services.Analytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.TestSuite.Services.Analytics
{
    public abstract class AnalyticsTrackerTests<T> where T : IAnalyticsTracker
    {
        public abstract T GetBasicAnalytics();

        [Fact]
        public async Task GetVisits_NoVisits_ShouldReturnEmpty()
        {
            var analytics = GetBasicAnalytics();

            Assert.Empty(await analytics.GetVisits());
        }

        [Fact]
        public async Task GetVisits_NoVisitsSpecifiedFile_ShouldReturnEmpty()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";

            Assert.Empty(await analytics.GetVisits(o => o.FileName = FILENAME));
        }

        [Fact]
        public async Task GetVisits_SingleVisitSpecifiedOtherFile_ShouldReturnEmpty()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";

            await analytics.RecordVisit(FILENAME);

            Assert.Empty(await analytics.GetVisits(o => o.FileName = FILENAME2));
        }

        [Fact]
        public async Task GetVisits_MultipleVisitSpecifiedFile_ShouldReturnMatchingFiles()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";

            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME2);

            var visits = await analytics.GetVisits(o => o.FileName = FILENAME);
            Assert.True(visits.Count() == 2);
            Assert.True(visits.All(v => v.Filename == FILENAME));
        }

        [Fact]
        public async Task RecordVisit_SingleVisitNoReferral_ReturnsSingleVisit()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";

            await analytics.RecordVisit(FILENAME);

            var visits = await analytics.GetVisits();
            Assert.True(visits.Count() == 1);
            Assert.True(visits.First().Filename == FILENAME);
        }

        [Fact]
        public async Task RecordVisit_MultipleVisitNoReferral_ReturnsAllVisits()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";

            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME);

            var visits = await analytics.GetVisits();
            Assert.True(visits.Count() == 2);
            Assert.True(visits.All(v => v.Filename == FILENAME));
        }

        [Fact]
        public async Task RecordVisit_MultipleDifferentVisitNoReferral_ReturnsAllVisits()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";

            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME2);

            var visits = await analytics.GetVisits();
            Assert.True(visits.Count() == 2);
            Assert.True(visits.Count(v => v.Filename == FILENAME) == 1);
            Assert.True(visits.Count(v => v.Filename == FILENAME2) == 1);
        }

        //TODO: Add tests for referrals
    }

    public class InMemAnalyticsTrackerTests : AnalyticsTrackerTests<InMemAnalytics>
    {
        public override InMemAnalytics GetBasicAnalytics()
        {
            return new InMemAnalytics();
        }
    }
}
