//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ZplrmApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Temi
    {
        public Temi()
        {
            this.Rabotilnici = new HashSet<Rabotilnici>();
        }
    
        public int TemaId { get; set; }
        public string TemaIme { get; set; }
        public string TemaOpis { get; set; }
    
        public virtual ICollection<Rabotilnici> Rabotilnici { get; set; }
    }
}