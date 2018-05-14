using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace ReportService.Domain
{
    public class Report
    {
	    private StringBuilder s;
	    private StringBuilder S => s ?? (s = new StringBuilder());
	    public string Path = "D:\\report.txt";
		public string Text => S.ToString();
	    public void Add(string str)
	    {
		    Contract.Assert(!string.IsNullOrEmpty(str));
			S.Append(str);
	    }	    
	    public void Save()
	    {
		    Contract.Assert(!string.IsNullOrEmpty(Text));
		    using (var file = new StreamWriter(Path))
		    {
			    file.Write(Text);
		    }
	    }
    }
}
