using System.ComponentModel.DataAnnotations;

namespace EShop.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "الاسم الكامل مطلوب")]
        [Display(Name = "الاسم الكامل")]
        [StringLength(100, ErrorMessage = "الاسم يجب أن يكون أقل من 100 حرف")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [EmailAddress(ErrorMessage = "البريد الإلكتروني غير صحيح")]
        [Display(Name = "البريد الإلكتروني")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [StringLength(100, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل {2} أحرف", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة المرور")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد كلمة المرور")]
        [Compare("Password", ErrorMessage = "كلمة المرور وتأكيدها غير متطابقتين")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "أوافق على الشروط والأحكام")]
        [Range(typeof(bool), "true", "true", ErrorMessage = "يجب الموافقة على الشروط والأحكام")]
        public bool AgreeToTerms { get; set; }
    }
}