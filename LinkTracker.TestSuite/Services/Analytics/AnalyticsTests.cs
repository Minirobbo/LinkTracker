using LinkTracker.API.Services.Analytics;
using LinkTracker.Shared.Models;
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

            Assert.Empty(await analytics.GetVisits(AnalyticsFilter.Filename(FILENAME)));
        }

        [Fact]
        public async Task GetVisits_SingleVisitSpecifiedOtherFile_ShouldReturnEmpty()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";

            await analytics.RecordVisit(FILENAME);

            Assert.Empty(await analytics.GetVisits(AnalyticsFilter.Filename(FILENAME2)));
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

            var visits = await analytics.GetVisits(AnalyticsFilter.Filename(FILENAME2));
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

        [Fact]
        public async Task GetVisits_NoVisitsSpecifiedReferral_ShouldReturnEmpty()
        {
            var analytics = GetBasicAnalytics();
            const string REFERRAL = "RefferralCode";

            Assert.Empty(await analytics.GetVisits(AnalyticsFilter.Referral(REFERRAL)));
        }

        [Fact]
        public async Task RecordVisit_SingleVisitWithReferral_ReturnsSingleVisit()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string REFERRAL = "RefferralCode";

            await analytics.RecordVisit(FILENAME, REFERRAL);

            var visits = await analytics.GetVisits(AnalyticsFilter.Referral(REFERRAL));
            Assert.True(visits.Count() == 1);
            Assert.True(visits.First().ReferralId == REFERRAL);
        }

        [Fact]
        public async Task RecordVisit_MultipleVisitSWithSingleReferral_ReturnsSingleVisit()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string REFERRAL = "RefferralCode";

            await analytics.RecordVisit(FILENAME, REFERRAL);
            await analytics.RecordVisit(FILENAME);

            var visits = await analytics.GetVisits(AnalyticsFilter.Referral(REFERRAL));
            Assert.True(visits.Count() == 1);
            Assert.True(visits.First().ReferralId == REFERRAL);
        }

        [Fact]
        public async Task RecordVisit_MultipleDifferentVisitsAllWithReferrals_ReturnsAllVisits()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";
            const string REFERRAL = "RefferralCode";

            await analytics.RecordVisit(FILENAME, REFERRAL);
            await analytics.RecordVisit(FILENAME2, REFERRAL);

            var visits = await analytics.GetVisits(AnalyticsFilter.Referral(REFERRAL));
            Assert.True(visits.Count() == 2);
            Assert.True(visits.Count(v => v.Filename == FILENAME) == 1);
            Assert.True(visits.Count(v => v.Filename == FILENAME2) == 1);
            Assert.True(visits.All(v => v.ReferralId == REFERRAL));
        }

        [Fact]
        public async Task RecordVisit_MultipleDifferentVisitsSomeWithReferrals_ReturnsAllReferredVisits()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";
            const string REFERRAL = "RefferralCode";

            await analytics.RecordVisit(FILENAME, REFERRAL);
            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME2, REFERRAL);
            await analytics.RecordVisit(FILENAME2);

            var visits = await analytics.GetVisits(AnalyticsFilter.Referral(REFERRAL));
            Assert.True(visits.Count() == 2);
            Assert.True(visits.Count(v => v.Filename == FILENAME) == 1);
            Assert.True(visits.Count(v => v.Filename == FILENAME2) == 1);
            Assert.True(visits.All(v => v.ReferralId == REFERRAL));
        }

        [Fact]
        public async Task RecordVisit_MultipleDifferentVisitsSomeWithReferralsRequestSingleFilename_ReturnsAllSingleVisit()
        {
            var analytics = GetBasicAnalytics();
            const string FILENAME = "exampleFile.txt";
            const string FILENAME2 = "exampleFile2.txt";
            const string REFERRAL = "RefferralCode";

            await analytics.RecordVisit(FILENAME, REFERRAL);
            await analytics.RecordVisit(FILENAME);
            await analytics.RecordVisit(FILENAME2, REFERRAL);
            await analytics.RecordVisit(FILENAME2);

            var visits = await analytics.GetVisits(new AnalyticsQuery().Where(AnalyticsFilter.Filename(FILENAME)).Where(AnalyticsFilter.Referral(REFERRAL)));
            Assert.True(visits.Count() == 1);
            Assert.True(visits.First().ReferralId == REFERRAL);
            Assert.True(visits.First().Filename == FILENAME);
        }
    }

    public class InMemAnalyticsTrackerTests : AnalyticsTrackerTests<InMemAnalytics>
    {
        public override InMemAnalytics GetBasicAnalytics()
        {
            return new InMemAnalytics();
        }
    }
}
