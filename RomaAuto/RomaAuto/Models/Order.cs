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
    
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Seller_Order = new HashSet<Seller_Order>();
        }
    
        public int OrderID { get; set; }
        public Nullable<int> ManufacturerID { get; set; }
        public Nullable<int> CarModelID { get; set; }
        public System.DateTime OpenDate { get; set; }
        public Nullable<int> CarCategoryID { get; set; }
        public int OutputDate { get; set; }
        public Nullable<int> TransmisionID { get; set; }
        public Nullable<int> CityID { get; set; }
        public string Phone { get; set; }
        public string Part { get; set; }
        public string Note { get; set; }
        public Nullable<System.DateTime> CloseDate { get; set; }
        public int OpenOperatorID { get; set; }
        public Nullable<int> CloseOperatorID { get; set; }
        public bool IsClosed { get; set; }
        public double Kubatura { get; set; }
        public Nullable<bool> IsShop { get; set; }
    
        public virtual CarModel CarModel { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Operator Operator { get; set; }
        public virtual Operator Operator1 { get; set; }
        public virtual CarCategory CarCategory { get; set; }
        public virtual City City { get; set; }
        public virtual Transmision Transmision { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Seller_Order> Seller_Order { get; set; }
    }
}
