using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_OblsugaPlikow.Models;

namespace WPF_OblsugaPlikow.Data
{
    class DaneContext
    {
        public List<OsobaDTO> Osoby { get; set; }
        private readonly string _pathFile;

        public DaneContext(string pathFile)
        {
            if (!File.Exists(pathFile))
            {
                FileStream fs = File.Create(pathFile);
                fs.Close();
            }
            Osoby = new List<OsobaDTO>();
            _pathFile = pathFile;
            CzytajDaneZPliku();
        }
        private string KonwertujDaneDoCSV(OsobaDTO osoba)
        {
            return $"{osoba.Id},{osoba.ImieNazwisko},{osoba.Wiek}";
        }
        private string KonwertujDaneDoMojegoFormatu(OsobaDTO osoba)
        {
            return $"start\n{osoba.ImieNazwisko}\n{osoba.Wiek}\nend";
        }
        private void CzytajDaneZPliku()
        {
            StreamReader sr = new StreamReader(_pathFile);
            var text = sr.ReadToEnd();
            var linie = text.Split("\r\n");
            foreach (var x in linie)
            {
                if (x != string.Empty)
                {
                    var dane = x.Split(",");
                    Osoby.Add(new OsobaDTO() { Id = int.Parse(dane[0]), ImieNazwisko = dane[1], Wiek = int.Parse(dane[2]) });
                }
            }
            sr.Close();
        }
        public void ZapiszZmiany()
        {
            File.WriteAllText(_pathFile, string.Empty);
            StreamWriter sw = File.AppendText(_pathFile);
            Osoby.ForEach(x => { sw.WriteLine(KonwertujDaneDoCSV(x)); });
            sw.Close();
        }

    }
}
