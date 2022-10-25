using InsuranceLib.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using Web.Services;

namespace Web.Middleware
{
    public class ButtonHadlerMiddleware1
    {
        private readonly RequestDelegate _next;
        
        public ButtonHadlerMiddleware1(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context, PolicyService policyService, EmployeeService employeeService )
        {
            
            await _next.Invoke(context);
            if (context.Request.Path == @"/Policy/SearchSes")
            {
                var model = GetModelFromSession(context);

                IEnumerable<Policy> policies;
                if (model.EmployeeId == 0)
                {
                    policies = policyService.GetByCondition(x =>
                    {
                        return x.StartDate == model.StartDate && x.FinishDate == model.FinishDate && x.Price == model.Price;
                    });
                }
                else
                {
                    policies = policyService.GetByCondition(x =>
                    {
                        return x.StartDate == model.StartDate && x.FinishDate == model.FinishDate && x.Price == model.Price && x.EmployeeId == model.EmployeeId;
                    });
                }

                var builder = new StringBuilder();
                builder.Append("<div>");
                builder.Append("<H1>Policy Search<H1>");
                builder.Append("<form method='get'>");

                builder.Append("<p><b>Policy Start date</b></p>");
                builder.Append($"<input type = 'text' name = 'startDate' value = '{model.StartDate}'></input>");

                builder.Append("<p><b> Policy Finish date </b></p>");
                builder.Append($"<input type = 'text' name = 'finishDate' value = '{model.FinishDate}'></input>");

                builder.Append("<p><b> Policy Price </b></p>");
                builder.Append($"<input type = 'text' name = 'price' value = '{model.Price}'></input>");

                builder.Append("<p><b> Employee </b></p>");
                builder.Append($"<select name = 'employeeMiddleName' value = '{model.EmployeeId}'>");
                builder.Append($"<option value = '0'> Any </option>");

                foreach (var employee in employeeService.GetAll())
                {
                    if (model.EmployeeId == employee.Id)
                    {
                        builder.Append($"<option selected value = '{employee.Id}'> {employee.MiddleName}</option>");
                    }
                    else
                        builder.Append($"<option value = '{employee.Id}'> {employee.MiddleName} </option>");
                }
                builder.Append("</select>");

                builder.Append("<br><input type ='submit' name = 'submit' value ='Submit'>");
                builder.Append("</form>");
                builder.Append("</div>");

                if (policies.Count() != 0)
                {
                    builder.Append("<div>");
                    builder.Append("<H1>Policy table</H1>");
                    builder.Append("<table border=1>");
                    builder.Append($"<td>StartDate</td><td>FinishDate</td><td>Price</td><td>Employee</td>");

                    foreach (var policy in policies)
                    {
                        builder.Append("<tr>");
                        builder.Append($"<td> {policy.StartDate.ToString("dd.MM.yyyy")}</td><td>{policy.FinishDate.ToString("dd.MM.yyyy")}</td><td>{policy.Price}</td><td>{policy.Employee.MiddleName}</td>");
                        builder.Append("</tr>");
                    }
                    builder.Append("</table>");
                    builder.Append("</div>");
                }
                else
                {
                    builder.Append("<p><h2>policy can not find</h2></p>");
                }
                await context.Response.WriteAsync(builder.ToString());

            }
        }

        private PolicySearchModel GetModelFromSession(HttpContext context)
        {
            if (context.Request.Query["startDate"].Count() > 0)
            {
                PolicySearchModel model = new PolicySearchModel
                {
                    StartDate = DateTime.Parse(context.Request.Query["startDate"]),
                    FinishDate = DateTime.Parse(context.Request.Query["finishDate"]),
                    Price = double.Parse(context.Request.Query["price"]),
                    EmployeeId = int.Parse(context.Request.Query["employeeMiddleName"])
                };
                context.Session.Set("model",model);
                return model;
            }
            else if (context.Session.Keys.Contains("model"))
            {
                PolicySearchModel model = context.Session.Get<PolicySearchModel>("model");
                return model;
            }
            else 
            {
                PolicySearchModel model = new PolicySearchModel
                {
                    StartDate = DateTime.Parse("01.01.2022"),
                    FinishDate = DateTime.Parse("01.01.2022"),
                    Price = 0,
                    EmployeeId = 0

                };
                context.Session.Set("model", model);
                return model;
            }
        }

    }
}
