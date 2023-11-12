using Microsoft.AspNetCore.Authorization;

namespace WebApp.Authorize
{
    public class EmployeePolicyRequirement : IAuthorizationRequirement
    {
        public int MonthsRequiredToProbation { get; }

        public EmployeePolicyRequirement(int monthsRequiredToProbation)
        {
            MonthsRequiredToProbation = monthsRequiredToProbation;
        }
    }
}
