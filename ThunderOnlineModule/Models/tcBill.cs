//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ThunderOnlineModule.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tcBill
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tcBill()
        {
            this.tcProductSales = new HashSet<tcProductSale>();
            this.tcServiceSales = new HashSet<tcServiceSale>();
            this.tcClientDetails = new HashSet<tcClientDetail>();
        }
    
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Nullable<bool> IsCredit { get; set; }
        public Nullable<int> PaymentMethod { get; set; }
        public string User { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    
        public virtual tcClient tcClient { get; set; }
        public virtual tcUser tcUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tcProductSale> tcProductSales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tcServiceSale> tcServiceSales { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tcClientDetail> tcClientDetails { get; set; }
    }
}