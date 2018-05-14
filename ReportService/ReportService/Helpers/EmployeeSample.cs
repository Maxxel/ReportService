using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Npgsql;
using ReportService.Domain;

namespace ReportService.Helpers
{
    public class EmployeeSample
	{	  
	    public List<Employee> GetEmployeeList()
	    {		    
		    var xml = ReportHelper.XmlDocPath;
			const string connectionString = @"Host=192.168.99.100;Username=postgres;Password=1;Database=employee";

		    return ReportHelper.IsDebugMode ? GetEmployeeXml(xml) : GetEmployeeNpqsq(connectionString);			
	    }

	    private List<Employee> GetEmployeeXml(string xml)
	    {
			Contract.Assert(!string.IsNullOrEmpty(xml));
			var deps = GetDepsXml(xml);
		    var emps = GetEmpsXml(xml);

		    var query = from e in emps
			    join d in deps on e.DepartmentId equals d.Id
			    select new Employee
			    {
				    Name = e.Name,
				    Inn = e.Inn,
				    Department = d.Name,
				    BuhCode = "1",
				    Salary = 1000,
			    };
		   return query.ToList();
		}

		private List<Employee> GetEmployeeNpqsq(string connectionString)
	    {
		    var conn = new NpgsqlConnection(connectionString);
		    conn.Open();
			var result = new List<Employee>();
			var cmd = new NpgsqlCommand("SELECT e.name, e.inn, d.name from emps e left join deps d on e.departmentid = d.id where d.active = true", conn);
		    var reader1 = cmd.ExecuteReader();
		    while (reader1.Read())
		    {
			    var inn = reader1.GetString(1);
			    var buhCode = EmpCodeResolver.GetCode(inn).Result;
			    var salary = EmployeeCommonMethods.GetSalary(inn, buhCode);
				var emp = new Employee
								{
									Name = reader1.GetString(0),
									Inn = inn,
									Department = reader1.GetString(2),
									BuhCode = EmpCodeResolver.GetCode(inn).Result,
									Salary = salary,
				};			   
				result.Add(emp);			    
			}
		    conn.Close();
			return result;
	    }

	    private List<DepModel> GetDepsXml(string xmlDoc)
	    {
		    var xmlStr = File.ReadAllText(xmlDoc);
		    var str = XElement.Parse(xmlStr);			
		    return str.Elements("deps").
						Where(el=>el.Element("active").Value == "true").
						Select(xElement => new DepModel
						{
							Id = xElement.Element("id").Value,				    
							Name = xElement.Element("name").Value,
						}).ToList();
	    }

	    private List<EmpModel> GetEmpsXml(string xmlDoc)
	    {
			var xmlStr = File.ReadAllText(xmlDoc);
		    var str = XElement.Parse(xmlStr);			
		    return str.Elements("emps").
					Select(xElement => new EmpModel
					{
						Name = xElement.Element("name").Value,
						DepartmentId = xElement.Element("departmentid").Value,
						Inn = xElement.Element("inn").Value,
					}).ToList();
	    }
	}
}
