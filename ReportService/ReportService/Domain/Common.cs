using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace ReportService.Domain
{
    public static class EmployeeCommonMethods
    {
	    public static int GetSalary(string inn, string buhCode)
	    {
		    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://salary.local/api/empcode/" + inn);
		    httpWebRequest.ContentType = "application/json";
		    httpWebRequest.Method = "POST";

		    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
		    {
			    var json = JsonConvert.SerializeObject(new { buhCode });
			    streamWriter.Write(json);
			    streamWriter.Flush();
			    streamWriter.Close();
		    }

		    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
		    var reader = new StreamReader(httpResponse.GetResponseStream(), true);
		    var responseText = reader.ReadToEnd();
		    return (int)decimal.Parse(responseText);
	    }

	}
}
