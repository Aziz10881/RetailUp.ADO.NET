using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace RetailUp.Models
{
    public class ItemToSellFilterViewModel
    {
        public string ItemName { get; set; }
        public string ItemBrand { get; set; }
        public string ItemModel { get; set; }
        public int? ItemCategoryId { get; set; }

        [DataType(DataType.Date)]
        public DateTime? ItemAddedDate { get; set; }

        public List<ItemToSell> ItemToSells { get; set; }

        public IPagedList<ItemToSell> ItemToSellsPaged;
    }
}
