using App5.Models;
using App5.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Forms;

namespace App5
{

    public partial class MainPage : ContentPage
    {
        List<Offer> offers;
        string q;


        public MainPage()
        {
            InitializeComponent();
            Init_offers_ListView();
        }
        //private void PrependHTTP(out string body)
        //{
        //    WebClient wc = new WebClient();
        //    byte[] data = wc.DownloadData("https://yastatic.net/market-export/_/partner/help/YML.xml");
        //    body = Encoding.ASCII.GetString(data);              
        //}
        private void Init_offers_ListView()
        {
            offers = new List<Offer>();

            Task<string> t = DownloadPageAsync();
            t.Wait();
            q = t.Result;

            XDocument xdoc = XDocument.Parse(q);
            var lv1s = from lv1 in xdoc.Descendants("offer")
                       select new
                       {
                           Header = lv1.ToString(),
                           Children = lv1.Attribute("id").Value.ToString()
                       };

            string json;
            foreach (var lv1 in lv1s)
            {
                XmlDocument tempdoc = new XmlDocument();
                tempdoc.LoadXml(lv1.Header);
                json = JsonConvert.SerializeXmlNode(tempdoc);
                offers.Add(new Offer(Int32.Parse(lv1.Children), json));
            };
            offers_ListView.ItemsSource = offers;
            offers_ListView.ItemTapped += Offers_ListView_ItemTapped;

        }


        private async void Offers_ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Offer q = (Offer)e.Item;
            await Navigation.PushAsync(new DetailView(q));
        }

        private static async Task<string> DownloadPageAsync()
        {
            string url = "https://yastatic.net/market-export/_/partner/help/YML.xml";
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url).ConfigureAwait(continueOnCapturedContext: false))
                {
                    var byteArray = response.Content.ReadAsByteArrayAsync().Result;
                    var result = Encoding.ASCII.GetString(byteArray, 0, byteArray.Length);
                    return result;
                }
            }
        }
    }
}