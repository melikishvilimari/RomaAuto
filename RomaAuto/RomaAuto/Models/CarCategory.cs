//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace RomaAuto.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CarCategory
    {
        public CarCategory()
        {
            this.SalersParts = new HashSet<SalersPart>();
            this.Orders = new HashSet<Order>();
        }
    
        public int CarCategoryID { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<SalersPart> SalersParts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}