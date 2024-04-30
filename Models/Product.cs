using System.ComponentModel.DataAnnotations;

namespace FormsApp.Models
{
    public class Product
    {
        [Display (Name="Ürün No")]
        public int ProductId { get; set; }

        [Required(ErrorMessage ="Ürün Adını Giriniz")]
        [Display (Name="Ürün Adı")]
        public string Name { get; set; }=null!;

        [Display (Name="Fiyat")]
        [Range(0,999999)]
        public decimal Price { get; set; }

        // [Required]
        [Display (Name="Görsel")]
        public string? Image { get; set; }

        [Display (Name="Aktiflik")]
        public bool IsActive { get; set; }

        [Required]
        [Display (Name="Kategori")]
        public int? CategoryId { get; set; }
    }
}

// ProductId:  1  name: iphone 14 categoryID: 1 
