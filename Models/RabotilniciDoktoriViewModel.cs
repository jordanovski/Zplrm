using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZplrmApp.Models
{
    public class RabotilniciDoktoriViewModel
    {
        public int RabotilnicaDoktorId { get; set; }
        public string RabotilnicaTema { get; set; }
        public DateTime RabotilnicaDatum { get; set; }
        public string RabotilnicaOdDo { get; set; }
        public string DoktorFaksimil { get; set; }
        public string DoktorImePrezime { get; set; }
    }
}