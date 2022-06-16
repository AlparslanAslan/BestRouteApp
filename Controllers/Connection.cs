using BestWayWebApp.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BestWayWebApp.Controllers
{
    public class Connection
    {
        public List<SensorRecord> Siralama(List<SensorRecord> sirasizListe)
        {
            List<SensorRecord> donenListe = new List<SensorRecord>();
            foreach(var eleman in sirasizListe)
            {
                for(int i=0;i<sirasizListe.Count;i++)
                {
                    if(sirasizListe[i] != eleman)
                    {
                       int sonuc =  karsilastir(sirasizListe[i], eleman);
                        Comparer(sirasizListe,eleman);
                        if(sonuc == 1)
                        {
                            donenListe.Add(eleman);
                        }
                        else
                        {
                            donenListe.Add(sirasizListe[i]);
                        }
                    }
                }
            }
            return donenListe;
        }
        public int karsilastir(SensorRecord s1,SensorRecord s2)
        {
            //Parse ediliyor langitude ve latitudeleri elde ettik
            string[] longLatPart1 = Convert.ToString(s1).Split(',');
            string[] longLatPart2 = Convert.ToString(s2).Split(',');
            var var1 = double.Parse(longLatPart1[0]);
            var var2 = double.Parse(longLatPart2[0]);

            if (var1 > var2)
            {
                return -1;
            }
            else if (var1 < var2)
            {
                return 1;
            }

            return var1 > var2 ? -1 : 1;
        }
        //boylama göre sıralandırırken
        public int Comparer(object a, object b)
        {
            SensorRecord o1 = (SensorRecord)a;
            SensorRecord o2 = (SensorRecord)b;
            if (o1.Longitute > o2.Longitute)
            {
                return -1; 
            }
            else if (o1.Latitude < o2.Longitute)
            {
                return 1;  
            }
            
            return o1.Latitude > o2.Latitude ? -1 : 1; 
        }
    }
}
