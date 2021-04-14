using RetailUp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailUp.DAL
{
    public interface IItemToSellRepository
    {
        public List<ItemToSell> GetItemsToSell();

        List<ItemToSell> Filter(
            string itemName, 
            string itemBrand,
            string itemModel, 
            int? itemCategoryId, 
            DateTime? itemAddedDate, 
            out int totalCount,
            int? page = 1,
            int pageSize = 3);

        void Insert(ItemToSell itm);

        void Update(ItemToSell itemToSell);

        ItemToSell GetById(int id);

        void Delete(int id);

       // List<ItemToSell> FilterUDSP(string itemName, string category, DateTime? kjjh);
    }
}
