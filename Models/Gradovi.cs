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
    
    public partial class Gradovi
    {
        public Gradovi()
        {
            this.Doktori = new HashSet<Doktori>();
            this.Prezenteri = new HashSet<Prezenteri>();
            this.Rabotilnici = new HashSet<Rabotilnici>();
        }
    
        public int GradId { get; set; }
        public string GradIme { get; set; }
    
        public virtual ICollection<Doktori> Doktori { get; set; }
        public virtual ICollection<Prezenteri> Prezenteri { get; set; }
        public virtual ICollection<Rabotilnici> Rabotilnici { get; set; }
    }
}
