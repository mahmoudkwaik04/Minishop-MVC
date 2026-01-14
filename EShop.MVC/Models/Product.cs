using System;
using System.ComponentModel.DataAnnotations;

namespace EShop.MVC.Models
{
    public class Product
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, 999999.99)]
        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
        
        public int Stock { get; set; } = 0;
        
        public string? Category { get; set; }
        
        public bool IsAvailable { get; set; } = true;
        
        public bool IsFeatured { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
