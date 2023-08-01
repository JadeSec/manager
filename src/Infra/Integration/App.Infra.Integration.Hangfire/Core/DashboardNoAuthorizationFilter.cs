using Hangfire.Dashboard;

namespace App.Infra.Integration.Hangfire.Core
{
    public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext dashboardContext)
            => true;
    }
}
