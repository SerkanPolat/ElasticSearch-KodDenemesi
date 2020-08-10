using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class KisiSuggestResponse
	{
		public IEnumerable<KisiSuggest> suggests { get; set; }

	}
	public class KisiSuggest
	{
		public int Id { get; set; }
		public CompletionField Ad { get; set; }
		public double Score { get; set; }
		public string SuggestedName { get; internal set; }
	}
}
