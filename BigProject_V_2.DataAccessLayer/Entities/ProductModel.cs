using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BigProject_V_2.DataAccessLayer.Entities
{
    public class ProductModel
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Description { get; set; }

        [Required]
        public bool OutOfStock { get; set; }
        
        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }
    }
}
