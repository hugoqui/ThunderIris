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
    
    public partial class tcClientDetail
    {
        public int Id { get; set; }
        public string ClientId { get; set; }
        public Nullable<decimal> Payment { get; set; }
        public Nullable<decimal> Credit { get; set; }
        public Nullable<int> PaymentMethod { get; set; }
        public Nullable<int> BillId { get; set; }
    
        public virtual tcBill tcBill { get; set; }
        public virtual tcClient tcClient { get; set; }
    }
}