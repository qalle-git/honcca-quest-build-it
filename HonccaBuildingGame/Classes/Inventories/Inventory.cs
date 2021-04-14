using System;
using System.Collections.Generic;
using System.Text;

namespace HonccaBuildingGame.Classes.Inventories
{
	class Inventory
	{
		public Item[] Items;

		public Inventory()
		{
			Items = new Item[4];

			//AddItem(new Item()
			//{
			//	Name = "DIRT_BLOCK",
			//	Count = 2
			//});
			//AddItem(new Item()
			//{
			//	Name = "GRASS_BLOCK",
			//	Count = 2
			//});
		}

		public bool AddItem(Item itemToAdd, int slotIndex = -1)
		{
			Item existingItem = GetItemWithName(itemToAdd.Name);

			if (existingItem.Name != "NONE")
			{
				slotIndex = existingItem.Slot;

				itemToAdd.Count += existingItem.Count;

				Console.WriteLine("Adding new item, but already existing so adding count.");
			}
			else if (GetEmptySlot() == -1)
			{
				Console.WriteLine("Inventory full, skip add.");

				return false;
			}

			if (slotIndex == -1)
			{
				Console.WriteLine("No slot specified, just add in a free one.");

				slotIndex = GetEmptySlot();
			}

			Console.WriteLine($"Added item: {itemToAdd.Name} with count: {itemToAdd.Count} on slot: {slotIndex}");

			itemToAdd.Slot = slotIndex;

			Items[slotIndex] = itemToAdd;

			return true;
		}

		public bool RemoveItem(Item itemToRemove, int slotIndex = -1)
		{
			if (slotIndex != -1)
			{
				Item itemOnSlot = GetItemOnSlot(slotIndex);

				if (itemOnSlot.Slot == -1)
				{
					return false;
				}
				else
				{
					Items[slotIndex].Count -= itemToRemove.Count;

					if (Items[slotIndex].Count <= 0)
					{
						Items[slotIndex] = new Item()
						{
							Name = "NONE",
							Count = 0,
							Slot = -1
						};
					};

					Console.WriteLine($"Removed {itemToRemove.Name} on already existing one.");

					return true;
				}
			}

			Item itemExists = GetItemWithName(itemToRemove.Name);

			if (itemExists.Slot == -1)
			{
				return false;
			} 
			else
			{
				Items[itemExists.Slot].Count -= itemToRemove.Count;

				if (Items[itemExists.Slot].Count <= 0)
				{
					Items[itemExists.Slot] = new Item()
					{
						Name = "NONE",
						Count = 0,
						Slot = -1
					};
				};

				Console.WriteLine($"Removed already existing {itemToRemove.Name} from count {Items[itemExists.Slot].Count + itemToRemove.Count} to: {Items[itemExists.Slot].Count} on slot: {itemExists.Slot}");
			}

			Console.WriteLine($"Removed {itemToRemove.Name} with count {itemToRemove.Count}");

			return true;
		}

		private int GetEmptySlot()
		{
			for (int currentItemIndex = 0; currentItemIndex < Items.Length; currentItemIndex++)
			{
				Item currentItem = Items[currentItemIndex];

				if (currentItem.Name == null || currentItem.Name.Length <= 0 || currentItem.Name == "NONE")
				{
					return currentItemIndex;
				}
			}

			return -1;
		}

		public Item GetItemWithName(string itemName)
		{
			for (int currentItemIndex = 0; currentItemIndex < Items.Length; currentItemIndex++)
			{
				Item currentItem = Items[currentItemIndex];

				if (currentItem.Name == itemName)
				{
					return currentItem;
				}
			}

			return new Item()
			{
				Name = "NONE",
				Count = 0,
				Slot = -1
			};
		}

		public Item GetItemOnSlot(int slotIndex)
		{
			for (int currentItemIndex = 0; currentItemIndex < Items.Length; currentItemIndex++)
			{
				Item currentItem = Items[currentItemIndex];

				if (currentItemIndex == slotIndex)
				{
					return currentItem;
				}
			}

			return new Item()
			{
				Name = "NONE",
				Count = 0,
				Slot = -1
			};
		}
	}
}
