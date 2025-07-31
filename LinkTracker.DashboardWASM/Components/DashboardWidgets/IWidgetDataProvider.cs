using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.DashboardWASM.Components.DashboardWidgets
{
    public interface IWidgetDataProvider<Traw, Tout>
    {
        public Task<IEnumerable<Traw>> GetData();

        public IEnumerable<Tout> ConvertData(IEnumerable<Traw> data);
    }
}
