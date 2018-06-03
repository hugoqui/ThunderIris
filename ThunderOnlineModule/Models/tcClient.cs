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
    
    public partial class tcClient
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tcClient()
        {
            this.tcBills = new HashSet<tcBill>();
            this.tcClientDetails = new HashSet<tcClientDetail>();
        }
    
        public string Id { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public Nullable<int> Group { get; set; }
        public string Email { get; set; }
        public string Parent { get; set; }
        public string Phone { get; set; }
        public string Comments { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tcBill> tcBills { get; set; }
        public virtual tcGroup tcGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tcClientDetail> tcClientDetails { get; set; }
    }
}
