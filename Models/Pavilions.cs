//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KingIT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pavilions
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Pavilions()
        {
            this.Rent = new HashSet<Rent>();
        }
    
        public string pavilionNumber { get; set; }
        public int shopCenterNumber { get; set; }
        public int stage { get; set; }
        public string status { get; set; }
        public double area { get; set; }
        public double costPerSquareMeter { get; set; }
        public double valueAddedFactor { get; set; }
    
        public virtual ShopCenters ShopCenters { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rent> Rent { get; set; }
    }
}
