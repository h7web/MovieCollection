//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace herman.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class director
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public director()
        {
            this.Videos = new HashSet<Video>();
        }
    
        public int dir_id { get; set; }
        public string dir_first_name { get; set; }
        public string dir_mi { get; set; }
        public string dir_last_name { get; set; }
        public System.Guid msrepl_tran_version { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Video> Videos { get; set; }
    }
}
