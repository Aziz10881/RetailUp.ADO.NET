﻿using RetailUp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RetailUp.DAL
{
	public class ItemToSellRepository : IItemToSellRepository
	{

		private const string SELECT = @"Select 
													[ID],
													[ItemName], 
													[ItemBrand],
													[ItemDescription],
													[ItemModel],
													[ItemAddedDate], 
													[ItemImage], 
													[IsActive],
													[ItemCategoryID],
													[ItemRemained],
													[ItemLeft]
													FROM [dbo].[ItemToSell]";
		private const string INSERT = @"INSERT INTO ItemToSell (
													[ItemName], 
													[ItemBrand],
													[ItemDescription],
													[ItemModel],
													[ItemAddedDate], 
													[ItemImage], 
													[IsActive],
													[ItemCategoryID],
													[ItemRemained],
													[ItemLeft]
																		)
													VALUES              (
												   @ItemName,
												   @ItemBrand,
												   @ItemDescription,
												   @ItemModel,
												   @ItemAddedDate, 
												   @ItemImage, 
												   @IsActive,
												   @ItemCategoryID,
												   @ItemRemained,
												   @ItemLeft
																		 )";
		private const string UPDATE = @"UPDATE ItemToSell SET
													[ItemName]        = @ItemName
												   ,[ItemBrand]       = @ItemBrand
												   ,[ItemDescription] = @ItemDescription
												   ,[ItemModel]       = @ItemModel
												   ,[ItemAddedDate]   = @ItemAddedDate
												   ,[ItemImage]       = @ItemImage
												   ,[IsActive]        = @IsActive
												   ,[ItemCategoryID]  = @ItemCategoryID
												   ,[ItemRemained]    = @ItemRemained
												   ,[ItemLeft]        = @ItemLeft
										WHERE ID = @ID";
		private const string SELECT_BY_ID = @"Select    [ID],
														[ItemName], 
														[ItemBrand],
														[ItemDescription],
														[ItemModel],
														[ItemAddedDate], 
														[ItemImage], 
														[IsActive],
														[ItemCategoryID],
														[ItemRemained],
														[ItemLeft]
														FROM [dbo].[ItemToSell]
														WHERE ID = @ItemToSellId";
		private const string DELETE = @"DELETE 
										FROM ItemToSell
										WHERE ID = @ItemToSellId";

		/// <summary>
		/// noo need for this so far
		/// </summary>
		//private const string UNLINK_CATEGORY_ID = @"update itemtosell
		//											set categoryid = null
		//											where categoryid = @ID";


		private readonly string ConnStr;

		public ItemToSellRepository(string connStr)
		{
			ConnStr = connStr;
		}
	 

		public List<ItemToSell> GetItemsToSell()
		{
			var list = new List<ItemToSell>();

			using (var conn = new SqlConnection(ConnStr))
			{
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = SELECT;
					cmd.CommandType = System.Data.CommandType.Text;


					conn.Open();

					using (var rdr = cmd.ExecuteReader())
					{
						while (rdr.Read())
						{
							var item = MapReaderToItemToSell(rdr);

							list.Add(item);

						}
					}
				}

			}

				return list;
		}


		public ItemToSell GetById(int id)
		{
			ItemToSell itemToSell = new ItemToSell();

			using(var conn = new SqlConnection(ConnStr))
			{
				using(var cmd = conn.CreateCommand())
				{
					cmd.CommandText = SELECT_BY_ID;

					cmd.Parameters.AddWithValue(@"ItemToSellID", id);

					conn.Open();
					using (var rdr = cmd.ExecuteReader())
					{
						if (rdr.Read())
						{
							itemToSell = MapReaderToItemToSell(rdr);
						}
					}
				}
			}


			return itemToSell;
		}

		public void Insert(ItemToSell itm) //itm = itemtosell
		{
			using(var conn = new SqlConnection(ConnStr))
			{
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = INSERT;

					//how it is done in anywhere
					var pItemName = cmd.CreateParameter();
					pItemName.ParameterName = "ItemName";
					pItemName.DbType = System.Data.DbType.String;
					pItemName.Value = itm.ItemName;
					pItemName.Direction = System.Data.ParameterDirection.Input;
					cmd.Parameters.Add(pItemName);

					//in ado.net SQL parameter
					cmd.Parameters.AddWithValue("ItemBrand", itm.ItemBrand ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("ItemDescription", itm.ItemDescription ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("ItemModel", itm.ItemModel ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("ItemAddedDate", itm.ItemAddedDate);
					cmd.Parameters.AddWithValue("ItemImage", itm.ItemImage ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("IsActive", itm.IsActive);
					cmd.Parameters.AddWithValue("ItemCategoryID", itm.ItemCategoryId ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("ItemRemained", itm.ItemRemained ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("ItemLeft", itm.ItemLeft ?? (object)DBNull.Value);

					conn.Open();
					cmd.ExecuteNonQuery();

				}
			}
		}

		public void Update(ItemToSell itemToSell)
		{
			using (var conn = new SqlConnection(ConnStr))
			{
				using(var cmd = conn.CreateCommand())
				{
					cmd.CommandText = UPDATE;

					cmd.Parameters.AddWithValue("@ID", itemToSell.ItemToSellId);
					cmd.Parameters.AddWithValue("@ItemName", itemToSell.ItemName);                
					cmd.Parameters.AddWithValue("@ItemBrand", itemToSell.ItemBrand ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ItemDescription", itemToSell.ItemDescription ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ItemModel", itemToSell.ItemModel ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ItemAddedDate", itemToSell.ItemAddedDate);
					cmd.Parameters.AddWithValue("@ItemImage", itemToSell.ItemImage ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@IsActive", itemToSell.IsActive);
					cmd.Parameters.AddWithValue("@ItemCategoryID", itemToSell.ItemCategoryId ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ItemRemained", itemToSell.ItemRemained ?? (object)DBNull.Value);
					cmd.Parameters.AddWithValue("@ItemLeft", itemToSell.ItemLeft ?? (object)DBNull.Value);

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}

		}

	  

		public void Delete(int id)
		{
			using (var conn = new SqlConnection(ConnStr))
			{


				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandText = DELETE;

					cmd.Parameters.Add("@ItemToSellId", SqlDbType.Int).Value = id;

					conn.Open();
					cmd.ExecuteNonQuery();
				}
			}
		}

		private ItemToSell MapReaderToItemToSell(DbDataReader rdr)
		{
			var item = new ItemToSell();

			item.ItemToSellId = rdr.GetInt32(rdr.GetOrdinal("ID"));

			item.ItemName = rdr.GetString(rdr.GetOrdinal("ItemName"));

			item.ItemBrand = rdr.IsDBNull(rdr.GetOrdinal("ItemBrand"))
				? (string)null
				: rdr.GetString(rdr.GetOrdinal("ItemBrand"));

			item.ItemDescription = rdr.IsDBNull(rdr.GetOrdinal("ItemDescription"))
				? null
				: rdr.GetString(rdr.GetOrdinal("ItemDescription"));

			item.ItemModel = rdr.IsDBNull(rdr.GetOrdinal("ItemModel"))
				? null
				: rdr.GetString(rdr.GetOrdinal("ItemModel"));

			item.ItemAddedDate = rdr.GetDateTime(rdr.GetOrdinal("ItemAddedDate"));

			item.ItemImage = rdr.IsDBNull(rdr.GetOrdinal("ItemImage"))
				? null
				: rdr.GetString(rdr.GetOrdinal("ItemImage"));

			item.IsActive = rdr.GetBoolean(rdr.GetOrdinal("IsActive"));

			item.ItemCategoryId = rdr.IsDBNull(rdr.GetOrdinal("ItemCategoryID"))
				? (int?)null
				: rdr.GetInt32(rdr.GetOrdinal("ItemCategoryID"));

			item.ItemRemained = (int)(rdr.IsDBNull(rdr.GetOrdinal("ItemRemained"))
				? (int?)null
				: rdr.GetInt32(rdr.GetOrdinal("ItemRemained")));

			item.ItemLeft = (int)(rdr.IsDBNull(rdr.GetOrdinal("ItemLeft"))
				? (int?)null
				: rdr.GetInt32(rdr.GetOrdinal("ItemLeft")));

			return item;
		}
	}
}