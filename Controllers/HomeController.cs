using BestWayWebApp.Models;
using BestWayWebApp.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace BestWayWebApp.Controllers
{
    public class HomeController : ApiController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

       
        //angular icin koordinatlari verecek
        public List<SensorRecord> GetDugumler(double longitude1, double latitude1,double longitude2,double latitude2, DateTime date)
        {
           
            string metin = "";
            qSensorRecord qSensor = new qSensorRecord();
            List<double> BaslangicNoktasi = new List<double>();
            List<double> BitisNoktasi = new List<double>();
            List<SensorRecord> sensorRecords = new List<SensorRecord>();

            List<SensorRecord> listOfSensor = new List<SensorRecord>();

            BaslangicNoktasi[0] = longitude1;
            BaslangicNoktasi[1] = latitude1;

            BitisNoktasi[0] = longitude2;
            BitisNoktasi[1] = latitude2;


            Sel_sensor selectSensors = new Sel_sensor();
            DataTable Sensorlerr = selectSensors.sel_tarih(date);
            bool AnalizYapilmisMi = false;
            if(Sensorlerr.Rows.Count > 0)
            {
                AnalizYapilmisMi = true;
            }

            
            

            DataTable dt = qSensor.getRecords();
            int x1 = 1;
            double y1 = 2;
            double z1 = 3;
            var Slist = new List<(int,double,double)>();
            metin += dt.Rows.Count + "\n";
            var uzakliklar = "";

            if (AnalizYapilmisMi == true)
            {
                VerileriGetir();
            }

            foreach (DataRow row in dt.Rows)
            {
               
                int _id = (int)row["Id"];
                double _Longitute = (double)row["Longitute"];
                double _Latitude = (double)row["Latitude"];
                metin += _id + " " + _Longitute+ " " + _Latitude/10000000000000+" \n";
                var m = (_id,_Longitute / 10000000000000, _Latitude / 10000000000000); 
                try
                {
                    Slist.Add(m);
                }
                catch(Exception e)
                {
                    metin += " sorun \n";
                }
                
                //foreach (DataColumn col in dt.Columns)
                //{
                //    metin += row[col] + "-";
                //}
                //metin += "\n";

            }
            //myList düğümler arası mesafeyi bulmak için kullandığım her bir düğümün birbiriyle enlem ve boylam bilgisi, permütasyonu
            List<List<double>> myList = new List<List<double>>();
            List<Coordinate> coordinates = new List<Coordinate>(); // hem park hem de sensörlerin bir arada yutulacağı liste
            for (int i = 0; i < Slist.Count; i++)
            {
                for (int j = i + 1; j < Slist.Count; j++)
                {
                    List<double> vs = new List<double>{ Slist[i].Item2, Slist[i].Item3, Slist[j].Item2, Slist[j].Item3 };
                    myList.Add(vs);
                }
            }
            
            List<(SensorRecord, double)> dijkstraicin = new List<(SensorRecord, double)>();


            Distance distance = new Distance();
            using StreamWriter file1 = new StreamWriter("C:\\Users\\Alparslan\\Desktop\\Bitirme Tezi\\WriteLines.txt", append: true);
            // uzaklikList her bir düğümün birbiriyle olan uzaklığının tutulduğu liste
            var uzaklikList = new List<double>();
            for (int i = 0; i < myList.Count; i++)
            {
                var dst = Math.Round(distance.Calculate(myList[i][1], myList[i][0], myList[i][3], myList[i][2]), 2);
                uzaklikList.Add(dst);
                file1.WriteLine(dst + "\n"); 
            }

            
           
            foreach ( var l in Slist)
            {
                metin += l.ToString() + " ";
                
            }

            // file1.WriteLine(metin);

            //
            // dList dijkstra metoduna parametre olarak verdiğim değişkenlerin listesi
            List<(char, char, int)> dList = new List<(char, char, int)>();
            int b = 0;
            int dugumSayisi = 0;
            List<string> sensorKodlari = new List<string>();
            for (int i = 0; i < Slist.Count; i++)
            {

                sensorKodlari.Add(Slist[i].Item2.ToString());
                dugumSayisi++;
               
            }
            
            foreach (var d in dList)
            {
                file1.WriteLine(d.Item1 + "& " + d.Item2 + " &" + d.Item3 + "\n");
            }

            Graph graph = new Graph(dugumSayisi);
            Connection connection = new Connection(); //dugumleri birbirine bagliyacak



            foreach(var x in Slist)
            {
                var uzaklık = distance.UzakliklariHesapla(BaslangicNoktasi[0], BaslangicNoktasi[1], x.Item2, x.Item3);
                uzaklikList.Add(uzaklık);

            }


            connection.Siralama(sensorRecords);//düğümleri sıraya diz
            foreach(var sensor in sensorRecords)
            {
                var crd = new Coordinate() { latitude = sensor.Latitude, longitude = sensor.Longitute };
                coordinates.Add(crd);
            }
            Func<char, int> id = c => c - 'A';
            Func<int, char> name = i => (char)(i + 'A');


            foreach (var (start, end, cost) in dList)
            {
                graph.AddEdge(id(start), id(end), cost);
            }

            var path = graph.FindPath(id('A'));
            string str = "";
            for (int d = id('B'); d <= id('I'); d++)
            {
                string s = "";
                foreach(var x in Path(id('A'), d).Select(p => $"{name(p.node)}({p.distance})"))
                {
                    s += x + "<-";
                }
                file1.WriteLine("sonuc->" + s);
            }

            //Path icindeki sensorleri goster
            IEnumerable<(double distance, int node)> Path(int start, int destination)
            {
                yield return (path[destination].distance, destination);
                for (int i = destination; i != start; i = path[i].prev)
                {
                    yield return (path[path[i].prev].distance, path[i].prev);
                }
            }
            var parkYerleri = isExistBikeParc(listOfSensor);
            if(parkYerleri.Count > 0)
            {
                string havaDurumu = WeatherConsition();
                foreach(var x in parkYerleri)
                {
                    if(havaDurumu == "sunny")
                    {
                        coordinates.Add(x); //eğer hava güneşli ise park noktaları gösterilecek

                    }
                }
            }
            return listOfSensor;
        }
        //Zaten analiz işlemi yapılmışsa 
        public List<SensorRecord> VerileriGetir()
        {
            List<SensorRecord> records = new List<SensorRecord>();
            Sel_sensor sensorleriGetir = new Sel_sensor();
            DataTable sensorler = sensorleriGetir.sel_sensor();

            if(sensorler.Rows.Count > 0)
            {
                foreach(DataRow x in sensorler.Rows)
                {
                    records.Add(new SensorRecord() {Latitude=(long)x[0],Longitute= (long)x[1] });
                }
            }
            return records;

        }
        public List<Coordinate> isExistBikeParc(List<SensorRecord> records)
        {
            bool bisikleteUygun = false;
            SensorRecord bitisNoktasi = new SensorRecord();
            List<Coordinate> parkYerleri = new List<Coordinate>();

            Sel_sensor sel = new Sel_sensor();
            DataTable table = sel.getCycleCoordinate();
            foreach(DataRow row in table.Rows)
            {
                double _longitude = (double)row["Longitude"];
                double _latitude = (double)row["Latitude"];
                if(records.Contains(row["adres"]))
                {
                    if(records.Where(x=>x.Geohash == row["kod"]).FirstOrDefault() != null)
                    {
                        bisikleteUygun = true;
                        bitisNoktasi = records.Where(x => x.Geohash == row["kod"]).FirstOrDefault();
                    }
                    else
                    {
                        bisikleteUygun = false;
                    }
                }
                DataTable parkYerleriTablosu =  sel.getCoordinateByAdres(row["Adres"].ToString());
                foreach(DataRow parkRow in parkYerleriTablosu.Rows)
                {
                    double x = (double)parkRow["Longitude"];
                    double y =(double) parkRow["Latitude"];
                    parkYerleri.Add(new Coordinate() { longitude = x,latitude = y });
                }
            }
            return parkYerleri;
        } //güzergah üzerinde bisiklet parkları var mı?
        public string WeatherConsition()
        {
            string condition = "";

            Weather openWeatherMap = new Weather();
            openWeatherMap.cities = new Dictionary<string, string>();
            openWeatherMap.cities.Add("İstanbul", "7839805");
            var cities = openWeatherMap.cities;
            if (cities != null)
            {
                /*Calling API http://openweathermap.org/api */
                string apiKey = "Your API KEY";
                HttpWebRequest apiRequest =
                WebRequest.Create("http://api.openweathermap.org/data/2.5/weather?id=" +
                cities + "&appid=" + apiKey + "&units=metric") as HttpWebRequest;

                string apiResponse = "";
                using (HttpWebResponse response = apiRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    apiResponse = reader.ReadToEnd();
                }
                /*End*/

                /*http://json2csharp.com*/
                Response rootObject = JsonConvert.DeserializeObject<Response>(apiResponse);

                StringBuilder sb = new StringBuilder();
                sb.Append("<table><tr><th>Weather Description</th></tr>");
                sb.Append("<tr><td>City:</td><td>" +
                rootObject.name + "</td></tr>");
                sb.Append("<tr><td>Country:</td><td>" +
                rootObject.sys.country + "</td></tr>");
                sb.Append("<tr><td>Wind:</td><td>" +
                rootObject.wind.speed + " Km/h</td></tr>");
                sb.Append("<tr><td>Current Temperature:</td><td>" +
                rootObject.main.temp + " °C</td></tr>");
                sb.Append("<tr><td>Humidity:</td><td>" +
                rootObject.main.humidity + "</td></tr>");
               
                
                openWeatherMap.apiResponse = sb.ToString();
            }
          
            return condition;
        }





    }
}
