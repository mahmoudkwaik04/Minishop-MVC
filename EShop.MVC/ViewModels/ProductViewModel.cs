using System.ComponentModel.DataAnnotations;

namespace EShop.MVC.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "اسم المنتج مطلوب")]
        [Display(Name = "اسم المنتج")]
        [StringLength(100, ErrorMessage = "اسم المنتج يجب أن يكون أقل من 100 حرف")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "وصف المنتج مطلوب")]
        [Display(Name = "وصف المنتج")]
        [StringLength(500, ErrorMessage = "وصف المنتج يجب أن يكون أقل من 500 حرف")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "سعر المنتج مطلوب")]
        [Display(Name = "السعر (ريال سعودي)")]
        [Range(0.01, 999999.99, ErrorMessage = "السعر يجب أن يكون بين 0.01 و 999999.99")]
        public decimal Price { get; set; }

        [Display(Name = "رابط الصورة")]
        [Url(ErrorMessage = "رابط الصورة غير صحيح")]
        public string? ImageUrl { get; set; }

        [Display(Name = "الكمية المتوفرة")]
        [Range(0, int.MaxValue, ErrorMessage = "الكمية يجب أن تكون رقم موجب")]
        public int Stock { get; set; } = 0;

        [Display(Name = "الفئة")]
        public string? Category { get; set; }

        [Display(Name = "متوفر للبيع")]
        public bool IsAvailable { get; set; } = true;

        [Display(Name = "منتج مميز")]
        public bool IsFeatured { get; set; } = false;
    }
}

