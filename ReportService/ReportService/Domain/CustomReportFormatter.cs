using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportService.Domain
{
    public static class CustomReportFormatter
    {
	    private static int Sum = 0;		

		public static string GetReport(IEnumerable<IGrouping<string, Employee>> grpoupList)
	    {
		    var reportText = new StringBuilder();
			FillStringBuilder(ref reportText, grpoupList);
			reportText.Append(ReportFormatter.NL);
		    reportText.Append(ReportFormatter.WL);
		    reportText.Append(ReportFormatter.NL);
		    reportText.Append(string.Format("Всего по предприятию {0}p", Sum));
		    return reportText.ToString();
	    }
		
	    private static void FillStringBuilder(ref StringBuilder reportText,
		    IEnumerable<IGrouping<string, Employee>> grpoupList)
	    {
		    foreach (var groupEmps in grpoupList)
		    {
			    reportText.Append(ReportPartAction(groupEmps));				
			}
	    }

		private static string ReportPartAction(IGrouping<string, Employee> groupEmps) 
	    {		  
			var result = new StringBuilder();
		    result.Append(ReportFormatter.NL);
		    result.Append(ReportFormatter.WL);
		    result.Append(ReportFormatter.NL);
		    result.Append(groupEmps.Key);
		    var sumDep = 0;

		    foreach (var emp in groupEmps)
		    {
			    result.Append(ReportFormatter.NL);
			    result.Append(emp.Name);
			    result.Append(ReportFormatter.WT);
			    result.Append(emp.Salary + "p");
			    sumDep += emp.Salary;
		    }
		    Sum += sumDep;
		    result.Append(ReportFormatter.NL);
		    result.Append(ReportFormatter.NL);
		    result.Append(string.Format("Всего по отделу {0}p", sumDep));

		    return result.ToString();
	    }
    }
}
