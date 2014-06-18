using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZplrmApp.Models
{
    public class RabotilniciViewModel
    {
        public int RabotilnicaId { get; set; }
        public string Tema { get; set; }
        public string Grad { get; set; }
        public string Mesto { get; set; }
        public string Lokacija { get; set; }
        
        public System.DateTime Datum { get; set; }
        public string OdDo { get; set; }
        public string Pocetok { get; set; }
        public string Kraj { get; set; }
        public Nullable<int> Bodovi { get; set; }
        public Nullable<int> OptimumPosetiteli { get; set; }
    }
}