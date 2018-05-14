using System;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using ReportService.Domain;
using ReportService.Helpers;

namespace ReportService.Controllers
{
    [Route("api/[controller]")]
    public class ReportController : Controller
    {
        [HttpGet]
        [Route("{year}/{month}")]
        public IActionResult Download(int year, int month)
        {
	        var helper = new EmployeeSample();
	        var employeeByDeps = helper.GetEmployeeList().GroupBy(i=>i.Department);

	        var report = new Report();
			report.Add(MonthNameResolver.MonthName.GetName(year, month));
			report.Add(CustomReportFormatter.GetReport(employeeByDeps));	        	        
	        report.Save();

	        var bytes = Encoding.Default.GetBytes(report.Text);
	        var response = File(bytes, "application/octet-stream", "report.txt");
			
			return response ?? throw new Exception("Report generating error!");			
        }
    }
}
