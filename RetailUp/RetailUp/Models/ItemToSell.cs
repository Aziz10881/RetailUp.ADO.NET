using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RetailUp.Models
{
    public class ItemToSell
    {
        [DisplayName("Id")]
        public int? ItemToSellId { get; set; }
        [Required]
        [DisplayName("Item Name") ]
        public string ItemName { get; set; }
        [DisplayName("Brand")]
        public string ItemBrand { get; set; }
        [DisplayName("Description")]
        public string ItemDescription { get; set; }
        [DisplayName("Model")]
        public string ItemModel { get; set; }
        [Required]
        [DisplayName("Added Date")]
        public DateTime? ItemAddedDate { get; set; }
        [DisplayName("Image")]
        public string ItemImage { get; set; }
        public bool IsActive { get; set; }
        [DisplayName("Category Id")]
        public int? ItemCategoryId { get; set; }
        [DisplayName("Remained")]
        public int? ItemRemained { get; set; }
        [DisplayName("Left")]
        public int? ItemLeft { get; set; }
    }
}
