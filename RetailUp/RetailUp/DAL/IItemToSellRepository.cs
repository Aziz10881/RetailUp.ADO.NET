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

        void Insert(ItemToSell itm);

        void Update(ItemToSell itemToSell);

        ItemToSell GetById(int id);

        void Delete(int id);
    }
}
