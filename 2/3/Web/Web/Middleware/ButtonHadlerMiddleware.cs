using InsuranceLib.Models;
using System.Text;
using Web.Services;

namespace Web.Middleware
{
    public class ButtonHadlerMiddleware
    {

        private readonly RequestDelegate _next;


        public ButtonHadlerMiddleware(RequestDelegate next)
        {
            this._next= next;
        }

        public async Task InvokeAsync(HttpContext context, PolicyService policyService, EmployeeService employeeService)
        {
            await _next.Invoke(context);
            if(context.Request.Path == @"/Policy/Search")
            {
                var startDate = DateTime.Parse( GetValueFromCookie(context,"startDate","01.01.2022")).ToString("dd.MM.yyyy");// context.Request.Query["startDate"].Count() > 0 ? DateTime.Parse( context.Request.Query["startDate"][0]).ToString("dd.MM.yyyy") : DateTime.Parse("01.01.2000").ToString("dd.MM.yyyy");
                var finishDate = DateTime.Parse( GetValueFromCookie(context, "finishDate", "01.01.2022")).ToString("dd.MM.yyyy");//context.Request.Query["finishDate"].Count() > 0 ? DateTime.Parse( context.Request.Query["finishDate"][0]).ToString("dd.MM.yyyy") : DateTime.Parse("01.01.2000").ToString("dd.MM.yyyy");
                var price = double.Parse( GetValueFromCookie(context,"price", "0"));//context.Request.Query["price"].Count() > 0 ? Double.Parse(context.Request.Query["price"][0]) : 0;
                var employeeId = int.Parse(GetValueFromCookie(context,"employeeMiddleName", "0"));//context.Request.Query["employeeMiddleName"].Count() > 0 ? int.Parse(context.Request.Query["employeeMiddleName"][0]) : 0;

                IEnumerable<Policy> policies;
                if(employeeId == 0)
                {
                    policies = policyService.GetByCondition(x =>
                    {
                        return x.StartDate == DateTime.Parse(startDate) && x.FinishDate == DateTime.Parse(finishDate) && x.Price == price;
                    });
                }
                else
                {
                    policies = policyService.GetByCondition(x =>
                    {
                        return x.StartDate == DateTime.Parse(startDate) && x.FinishDate == DateTime.Parse(finishDate) && x.Price == price && x.EmployeeId == employeeId;
                    });
                }

                var builder = new StringBuilder();
                builder.Append("<div>");
                builder.Append("<H1>Policy Search<H1>");
                builder.Append("<form method='get'>");

                builder.Append("<p><b>Policy Start date</b></p>");
                builder.Append($"<input type = 'text' name = 'startDate' value = '{startDate}'></input>");

                builder.Append("<p><b> Policy Finish date </b></p>");
                builder.Append($"<input type = 'text' name = 'finishDate' value = '{finishDate}'></input>");

                builder.Append("<p><b> Policy Price </b></p>");
                builder.Append($"<input type = 'text' name = 'price' value = '{price}'></input>");

                builder.Append("<p><b> Employee </b></p>");
                builder.Append($"<select name = 'employeeMiddleName' value = '{employeeId}'>");
                builder.Append($"<option value = '0'> Any </option>");

                foreach (var employee in employeeService.GetAll())
                {
                    if (employeeId == employee.Id)
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

                if(policies.Count() != 0)
                {
                    builder.Append("<div>");
                    builder.Append("<H1>Policy table</H1>");
                    builder.Append("<table border=1>");
                    builder.Append($"<td>StartDate</td><td>FinishDate</td><td>Price</td><td>Employee</td>");
                    
                    foreach(var policy in policies)
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

                context.Response.Cookies.Append("startDate", startDate.ToString());
                context.Response.Cookies.Append("finishDate", finishDate.ToString());
                context.Response.Cookies.Append("price", price.ToString());
                context.Response.Cookies.Append("employeeMiddleName", employeeId.ToString());

                await context.Response.WriteAsync(builder.ToString());
            }
        }

        private string GetValueFromCookie(HttpContext context, string cookieName, string defaultValue = "")
        {
            if (context.Request.Query[cookieName].Count() > 0)
            {
                return context.Request.Query[cookieName][0];
            }
            else if (context.Request.Cookies[cookieName] != null)
            {
                return context.Request.Cookies[cookieName];
            }
            else
            {
                return defaultValue;
            }
        }
    }
}
