using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RetailUp.DAL;
using RetailUp.Models;
using X.PagedList;

namespace RetailUp.Controllers
{
    public class ItemsToSellController : Controller
    {
        public IItemToSellRepository ItemRep;

       public ItemsToSellController(IItemToSellRepository itemRep)
        {
            ItemRep = itemRep;
        }

        // GET: ItemsToSellController
        public ActionResult Index()
        {
            var listItem = ItemRep.GetItemsToSell();

            return View(listItem);
        }



        public ActionResult Filter(int page, ItemToSellFilterViewModel model)
        {
            int totalCount;
            var list = ItemRep.Filter(model.ItemName, model.ItemBrand, model.ItemModel, 
                model.ItemCategoryId, model.ItemAddedDate, out totalCount, page, 2);


            if(page <= 0)
            {
                page = 1;
            }

            //model.ItemToSells = list;
            model.ItemToSellsPaged = new StaticPagedList<ItemToSell>(list, page, 2, totalCount);

                return View(model);
        }

        public ActionResult ExportJson(ItemToSellFilterViewModel model)
        {
            int totalCount;
            var list = ItemRep.Filter(
                model.ItemName, 
                model.ItemBrand,
                model.ItemModel,
                model.ItemCategoryId,
                model.ItemAddedDate, 
                out totalCount,
                1,
                1000_000);

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var serializer = new JsonSerializer();
            serializer.Serialize(writer, list);
            writer.Flush();

            memory.Position = 0;
            if (memory != Stream.Null)
            {
                return File(memory, "application/json", $"Export_{DateTime.Now}.json");
            }
            return NotFound();
        }

        public ActionResult ExportXML(ItemToSellFilterViewModel model)
        {
            int totalCount;
            var list = ItemRep.Filter(
                model.ItemName,
                model.ItemBrand,
                model.ItemModel,
                model.ItemCategoryId,
                model.ItemAddedDate,
                out totalCount,
                1,
                1000_000);

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var serializer = new XmlSerializer(typeof(List<ItemToSell>));
            serializer.Serialize(writer, list);
            writer.Flush();

            memory.Position = 0;
            if (memory != Stream.Null)
            {
                return File(memory, "application/xml", $"Export_{DateTime.Now}.xml");
            }
            return NotFound();
        }

        public ActionResult ExportCSV(ItemToSellFilterViewModel model)
        {
            int totalCount;
            var list = ItemRep.Filter(
                model.ItemName,
                model.ItemBrand,
                model.ItemModel,
                model.ItemCategoryId,
                model.ItemAddedDate,
                out totalCount,
                1,
                1000_000);

            var memory = new MemoryStream();
            var writer = new StreamWriter(memory);
            var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csvWriter.WriteRecords(list);
            writer.Flush();

            memory.Position = 0;
            if (memory != Stream.Null)
            {
                return File(memory, "text/csv", $"Export_{DateTime.Now}.csv");
            }
            return NotFound();
        }

        public ActionResult ImportJson()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportJson(IFormFile importFile)
        {
            IList<ItemToSell> itemstosell = null;
            if(importFile != null)
            {
                using (var stream = importFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new JsonSerializer();
                    itemstosell = (List<ItemToSell>)serializer.
                        Deserialize(reader, typeof(List<ItemToSell>));
                }
                foreach(var itm in itemstosell)
                {
                    ItemRep.Insert(itm);
                }
            }

            return View(itemstosell);
        }


        public ActionResult ImportCsv()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportCsv(IFormFile importFile)
        {
            var itemsTosell = new List<ItemToSell>();
            if (importFile != null)
            {
                using (var stream = importFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new CsvReader(reader, CultureInfo.InvariantCulture);
                    itemsTosell = serializer.GetRecords<ItemToSell>().ToList<ItemToSell>();
                }

                foreach (var itm in itemsTosell)
                    ItemRep.Insert(itm);
            }
            else
            {
                ModelState.AddModelError("", "Empty file");
            }

            return View(itemsTosell);
        }

        public ActionResult ImportXml()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportXml(IFormFile importFile)
        {
            var itemsTosell = new List<ItemToSell>();
            if (importFile != null)
            {
                using (var stream = importFile.OpenReadStream())
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new XmlSerializer(typeof(List<ItemToSell>));
                    itemsTosell = (List<ItemToSell>)serializer.Deserialize(reader);
                }

                foreach (var itm in itemsTosell)
                    ItemRep.Insert(itm);
            }
            else
            {
                ModelState.AddModelError("", "Empty file");
            }

            return View(itemsTosell);
        }



        // GET: ItemsToSellController/Details/5
        public ActionResult Details(int id) 
        {
            var itemToSell = ItemRep.GetById(id);

            return View(itemToSell);
        }

        // GET: ItemsToSellController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemsToSellController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ItemToSell itemToSell)
        {
            try
            {
                ItemRep.Insert(itemToSell);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }


        // GET: ItemsToSellController/Edit/5
        public ActionResult Edit(int id)
        {
            var item = ItemRep.GetById(id);

            return View(item);
        }

        // POST: ItemsToSellController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( ItemToSell itemToSell)
        {
            try
            {             
                ItemRep.Update(itemToSell);

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }

        // GET: ItemsToSellController/Delete/5
        public ActionResult Delete(int id)
        {
            var itemToSell = ItemRep.GetById(id);
            return View(itemToSell);
        }

        // POST: ItemsToSellController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                ItemRep.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}
