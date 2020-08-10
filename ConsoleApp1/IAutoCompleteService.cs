using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public interface IAutoCompleteService
	{
		Task<bool> CreateIndexAsync(string indexName);
		Task IndexAsync(string indexName, List<YKisi> kisiler);
		KisiSuggestResponse SuggestAsync(string indexName, string keyword);

	}
}
