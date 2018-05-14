using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportService.Controllers;
using ReportService.Domain;

namespace ReportServiceTests
{
	[TestClass]
    public class Test
    {
		private int Year { get; set; }
		private int Month { get; set; }

		private ReportController Controller { get; set; }

	    [TestInitialize]
	    public void Initialization()
	    {
		    Year = 2017;
		    Month = 2;
		    ReportService.Helpers.ReportHelper.IsDebugMode = true;		    
			ReportService.Helpers.ReportHelper.XmlDocPath = @"testDataEmps.xml";		    
			Controller = new ReportController();
		}

        [TestMethod]
        public void ResponseTest()
        {
	        var departmentCount = 6;
	        var employeeCount = 29;

			var response = Controller.Download(Year, Month) as FileContentResult;
			Assert.IsNotNull(response);

	        var text = System.Text.Encoding.Default.GetString(response.FileContents);
			Assert.IsTrue(!string.IsNullOrEmpty(text));

	        var array = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);			

	        var currentDepCount = -1;
	        var currentEmpCount = 0;

	        CountingDepsAndEmps(ref currentDepCount, ref currentEmpCount, array);

			Assert.AreEqual(departmentCount, currentDepCount, string.Format(CountError,"Department",6));
	        Assert.AreEqual(employeeCount, currentEmpCount, string.Format(CountError, "Employee", 29));
		}

	    private void CountingDepsAndEmps(ref int depCount, ref int empCount, string[] array)
	    {
			foreach (var line in array)
			{
				if (string.Equals(line, ReportFormatter.WL))
				{
					depCount++;
				}
				else if (line.Contains("Фамилия"))
				{
					empCount++;
				}
			}
		}

	    private string CountError = @"{0} number mismatch. Expected : {1}";
    }
}
