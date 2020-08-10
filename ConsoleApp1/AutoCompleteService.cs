using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
	public class AutoCompleteService : IAutoCompleteService
	{
		readonly ElasticClient client;
		public AutoCompleteService(ConnectionSettings settings)
		{
			client = new ElasticClient(settings);
		}

		public async Task<bool> CreateIndexAsync(string indexName)
		{
			var createIndexDescriptor =  await client.Indices.CreateAsync(indexName, index =>
			 index.Map<Kisi>(m => m.AutoMap().Properties(ps => ps.Completion(
						c => c.Name(p => p.suggest)))));

			return createIndexDescriptor.IsValid;
		}

		public async Task IndexAsync(string indexName, List<YKisi> kisiler)
		{
			await client.IndexManyAsync(kisiler, indexName);
			
		}

		public KisiSuggestResponse SuggestAsync(string indexName, string keyword)
		{
			ISearchResponse<YKisi> searchResponse = client.Search<YKisi>(s => s
			.Index(indexName)
			.Suggest(su => su
			.Completion("suggestions", c => c
			 .Field(f => f.Ad)
			 .Prefix(keyword)
			 .Fuzzy(F => F
			 .Fuzziness(Fuzziness.Auto)).Size(5))));

			var suggests = from suggest in searchResponse.Suggest["suggestions"]
						   from option in suggest.Options
						   select new KisiSuggest
						   {
							   Id = option.Source.Id,
							   Ad = option.Source.Ad,
							   Score = option.Score,
							   SuggestedName = option.Text

						   };
			return new KisiSuggestResponse
			{
				suggests = suggests
			};
		}
	}
}
