using Nest;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{

	class Program
	{

		static Random rnd = new Random();
		static string indexAd = "ogrenciarama";
		public static void Main(string[] args)
		{
			/*
			var settings = new ConnectionSettings().DefaultIndex("ogrenciler");
			var client = new ElasticClient(settings);
			
			var searchResponse = client.Search<Kisi>(s=> s.Query(q=>q.MatchAll()).Size(10));
			searchResponse = client.Search<Kisi>(s => s.Scroll("1s").Size(1));
			searchResponse = client.Scroll<Kisi>("1m", searchResponse.ScrollId);
			var sources = searchResponse.HitsMetadata.Hits.Select(h => h.Source);
			Console.WriteLine(sources);
			//Console.WriteLine(indexResponse);
			Console.WriteLine(client);
			Console.Read();
			*/
			/*

			List<YKisi> products = new List<YKisi>();

			products.Add(new YKisi()
			{
				Id = 1,
				Ad = new CompletionField()
				{
					Input = new[] { "Asiye" }
				},
				Suggest = new CompletionField()
				{
					Input = new[] { "Samsung Galaxy Note 8", "Galaxy Note 8", "Note 8" }
				}
			});

			products.Add(new YKisi()
			{
				Id = 2,
				Ad = new CompletionField()
				{
					Input = new[] { "Deneme" }
				},
				Suggest = new CompletionField()
				{
					Input = new[] { "Samsung Galaxy S8", "Galaxy S8", "S8" }
				}
			});

			products.Add(new YKisi()
			{
				Id = 3,
				Ad = new CompletionField()
				{
					Input = new[] { "Deneme2" }
				},
				Suggest = new CompletionField()
				{
					Input = new[] { "Apple Iphone 8", "Iphone 8" }
				}
			});

			products.Add(new YKisi()
			{
				Id = 4,
				Ad = new CompletionField()
				{
					Input = new[] { "Serkan" }
				},
				Suggest = new CompletionField()
				{
					Input = new[] { "Apple Iphone X", "Iphone X" }
				}
			});

			products.Add(new YKisi()
			{
				Id = 5,
				Ad = new CompletionField()
				{
					Input = new[] { "Deneme5" }
				},
				Suggest = new CompletionField()
				{
					Input = new[] { "Apple iPad Pro", "iPad Pro" }
				}
			});

			*/
			/*
			var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"));
			IAutoCompleteService autocompleteService = new AutoCompleteService(connectionSettings);
			string productSuggestIndex = "asd";

			bool isCreated = autocompleteService.CreateIndexAsync(productSuggestIndex).Result;

			if (isCreated)
			{
				autocompleteService.IndexAsync(productSuggestIndex, products).Wait();
			}


			var s = autocompleteService.SuggestAsync(productSuggestIndex, "s");
			*/
			var connectionSettings = new ConnectionSettings(new Uri("http://localhost:9200"));
			IAutoCompleteService autocompleteService = new AutoCompleteService(connectionSettings);
			bool isCreated = autocompleteService.CreateIndexAsync(indexAd).Result;
			Console.WriteLine(NumaraAyarla());
			Console.Read();
			KisiEkleme();



		
		}

		private static void KisiEkleme()
		{

			StreamReader srSehir = new StreamReader("sehirler.txt", Encoding.UTF8);
			string sehir;
			List<string> sehirler = new List<string>();
			while ((sehir = srSehir.ReadLine()) != null)
			{
				sehirler.Add(sehir);
			}

			StreamReader srBolum = new StreamReader("bolumler.txt", Encoding.UTF8);
			string bolum;
			List<string> bolumler = new List<string>();
			while ((bolum = srBolum.ReadLine()) != null)
			{
				bolumler.Add(bolum);
			}
			Console.WriteLine(bolumler.Count);

			StreamReader srAd = new StreamReader("isimler", Encoding.UTF8);
			string isim;
			List<string> isimler = new List<string>();
			while ((isim = srAd.ReadLine()) != null)
			{
				isimler.Add(isim);
			}
			Console.WriteLine(isimler.Count);
			StreamReader srSoyad = new StreamReader("soyisimler", Encoding.UTF8);
			string soyisim;
			List<string> soyisimler = new List<string>();
			while ((soyisim = srSoyad.ReadLine()) != null)
			{
				soyisimler.Add(soyisim);
			}
			Console.WriteLine(soyisimler.Count);

			var settings = new ConnectionSettings().DefaultIndex(indexAd);
			var client = new ElasticClient(settings);

			string isimTemp = "";
			string soyIsimTemp = "";
			string bolumTemp = "";
			for (int i = 0; i < 30000; i++)
			{
				isimTemp = isimler[rnd.Next(isimler.Count)];
				soyIsimTemp = soyisimler[rnd.Next(soyisimler.Count)];
				bolumTemp = bolumler[rnd.Next(bolumler.Count)];
				string Num = NumaraAyarla();
				string Tel = TelefonAyarla();
				sehir = sehirler[rnd.Next(sehirler.Count)];
				List<string> listTemp = new List<string>();
				listTemp.Add(isimTemp);
				listTemp.Add(soyIsimTemp);
				listTemp.Add(Num);
				listTemp.Add(Tel);
				listTemp.Add(sehir);
				listTemp.Add(bolumTemp);
				listTemp.Add(isimTemp + " " + soyIsimTemp);
				
				if(isimTemp.Split(' ').Length == 2)
				{
					listTemp.Add(isimTemp.Split(' ')[1]);
				}
				if (soyIsimTemp.Split(' ').Length == 2)
				{
					listTemp.Add(soyIsimTemp.Split(' ')[1]);
				}
				
				string ret = client.IndexDocument<Kisi>(new Kisi()
				{
					Ad = isimTemp,
					Soyad = soyIsimTemp,
					Bolum = bolumTemp,
					Telefon = Tel,
					Numara = Num,
					Sehir = sehir,
					suggest = new CompletionField()
					{
						Input = listTemp.ToList()
					}
				}).Result.ToString();
				//System.Diagnostics.Debug.WriteLine(ret);
			}
		}
		static int sayac = 0;
		private static string NumaraAyarla()
		{
			string Numara="";
			// B171210357
			if (sayac % 2 == 0)
			{
				Numara += 'B';
			}
			else
			{
				Numara += 'G';
			}
			sayac++;
			Numara += '1';
			for (int i = 0; i < 8; i++)
			{
				Numara += rnd.Next(9).ToString();
			}

			return Numara;
		}

		private static string TelefonAyarla()
		{
			//05315654500
			string Telefon = "05";
			for (int i = 0; i < 9; i++)
			{
				Telefon += rnd.Next(9).ToString();
			}
			return Telefon;
		}
	}	
}
