using Microsoft.AspNetCore.Authorization;

namespace WebApp.Authorize
{
    public class EmployeePolicyHandler : AuthorizationHandler<EmployeePolicyRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, EmployeePolicyRequirement requirement)
        {
            if (!context.User.HasClaim( c => c.Type.Equals("ProbationDate")))
                return Task.CompletedTask;

            if (DateTime.TryParse(context.User.FindFirst(c => c.Type.Equals("ProbationDate"))?.Value,out DateTime currentDate))
            {
                if (DateTime.Now > currentDate.AddMonths(requirement.MonthsRequiredToProbation))
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
