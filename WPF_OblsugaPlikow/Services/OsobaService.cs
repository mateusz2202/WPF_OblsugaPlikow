using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_OblsugaPlikow.Data;
using WPF_OblsugaPlikow.Models;

namespace WPF_OblsugaPlikow.Services
{
    class OsobaService : IOsobaService
    {
        private readonly DaneContext _daneContext;
        private int _id;

        public OsobaService()
        {
            _daneContext = new DaneContext("dane1.txt");
            _id = GetId();
        }

        public bool AktualizujOsobe(int id, OsobaDTO osobaDTO)
        {
            var osoba = _daneContext.Osoby.FirstOrDefault(x => x.Id == id);
            if (osoba == null) return false;
            osobaDTO.Id = osoba.Id;
            _daneContext.Osoby.Remove(osoba);
            _daneContext.Osoby.Add(osobaDTO);
            _daneContext.ZapiszZmiany();
            return true;
        }

        public OsobaDTO GetOsoba(int id)
        {
            var osoba = _daneContext.Osoby.FirstOrDefault(x => x.Id == id);
            return osoba;
        }

        public List<OsobaDTO> GetOsoby()
        {
            return _daneContext.Osoby;
        }

        public void ResetDanych()
        {
            _daneContext.Osoby.Clear();
            List<OsobaDTO> osoby = new List<OsobaDTO>()
            {
                new OsobaDTO()
                {
                    ImieNazwisko="Jan Kowalski",
                    Wiek=30
                },
                new OsobaDTO()
                {
                    ImieNazwisko="Adam Nowak",
                    Wiek=20
                },
                new OsobaDTO()
                {
                    ImieNazwisko="Piotr Prank",
                    Wiek=35
                },
                new OsobaDTO()
                {
                    ImieNazwisko="Dorian Kran",
                    Wiek=27
                },
                new OsobaDTO()
                {
                    ImieNazwisko="Aldona Zbieg",
                    Wiek=33
                },
                new OsobaDTO()
                {
                    ImieNazwisko="Krystian Klon",
                    Wiek=39
                },
            };
            osoby.ForEach(x => StworzOsobe(x));

        }

        public void StworzOsobe(OsobaDTO osoba)
        {

            osoba.Id = _id++;
            _daneContext.Osoby.Add(osoba);
            _daneContext.ZapiszZmiany();
        }

        public bool UsunOsobe(int id)
        {
            var osoba = _daneContext.Osoby.FirstOrDefault(x => x.Id == id);
            if (osoba == null) return false;
            _daneContext.Osoby.Remove(osoba);
            _daneContext.ZapiszZmiany();
            return true;
        }

        private int GetId()
        {
            if (_daneContext.Osoby.Count == 0) return 0;
            else return _daneContext.Osoby.Last().Id + 1;
        }



    }


}
