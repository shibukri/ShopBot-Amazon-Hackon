using Employee_Management_System.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Employee_Management_System.ServiceFilters
{
    public class BuildEmployeeFilter : IAsyncActionFilter
    { 
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var paras = context.ActionArguments.SingleOrDefault(p => p.Value is EmployeeFilterCriteria);
            if (paras.Value == null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }
            EmployeeFilterCriteria filterCriteria = (EmployeeFilterCriteria)paras.Value;
            var statusFilter = filterCriteria.Filters.Find(a => a.FieldName == "status");
            if (statusFilter == null)
            {
                statusFilter = new FilterCriteria();
                statusFilter.FieldName = "status";
                statusFilter.FieldValue = "Active";
                filterCriteria.Filters.Add(statusFilter);
            }

            filterCriteria.Filters.RemoveAll(a => string.IsNullOrEmpty(a.FieldName));

            var result  =   await next();
        }
    }
}
