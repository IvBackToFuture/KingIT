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
    
    public partial class ShopCenters
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ShopCenters()
        {
            this.Pavilions = new HashSet<Pavilions>();
        }
    
        public int shopCenterNumber { get; set; }
        public string shopCenterName { get; set; }
        public string status { get; set; }
        public string city { get; set; }
        public double price { get; set; }
        public double valueAddedFactor { get; set; }
        public int numberOfStoreys { get; set; }
        public byte[] photo { get; set; }
        public Nullable<int> countOfPavilions { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pavilions> Pavilions { get; set; }
    }
}
