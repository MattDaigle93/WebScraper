using CsvHelper;
using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Web;

namespace Webscraper
{
	public class Scraper
    {
		public ObservableCollection<EntryModel> _entries = new ObservableCollection<EntryModel>();

		public ObservableCollection<EntryModel> Entries
		{
			get { return _entries; }
			set { _entries = value; }
		}

		public void ScrapeData(string page)
		{
			var web = new HtmlWeb();
			var doc = web.Load(page);

			var Businesses = doc.DocumentNode.SelectNodes("//*[@class = 'v-card']");

			foreach (var business in Businesses)
			{
                var name = HttpUtility.HtmlDecode(business.SelectSingleNode(".//a[@class = 'business-name']").InnerText);
				var number = HttpUtility.HtmlDecode(business.SelectSingleNode("//div[@class = 'info-section info-secondary']").InnerText);
                var website = HttpUtility.HtmlDecode(business.SelectSingleNode("//a[@class = 'track-visit-website']").GetAttributeValue("href", string.Empty));
                //var address = HttpUtility.HtmlDecode(business.SelectSingleNode("//div[@class = 'adr']").InnerText);

				_entries.Add(new EntryModel { Name = name, Number = number, Website = website });

			}
		}
        public void Export()
        {
            using (TextWriter tw = File.CreateText("SampleData.csv"))
            {
                using (var cw = new CsvWriter(tw, CultureInfo.InvariantCulture))
                {
                    foreach (var entry in Entries)
                    {
                        cw.WriteRecord(entry);
                        cw.NextRecord();
                    }
                }
            }
        }

    }
}
