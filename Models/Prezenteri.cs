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
    
    public partial class Prezenteri
    {
        public Prezenteri()
        {
            this.RabotilniciPrezenteri = new HashSet<RabotilniciPrezenteri>();
        }
    
        public string PrezenterFaksimil { get; set; }
        public string PrezenterImePrezime { get; set; }
        public Nullable<int> GradId { get; set; }
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public string FixedPhone { get; set; }
    
        public virtual Gradovi Gradovi { get; set; }
        public virtual ICollection<RabotilniciPrezenteri> RabotilniciPrezenteri { get; set; }
    }
}