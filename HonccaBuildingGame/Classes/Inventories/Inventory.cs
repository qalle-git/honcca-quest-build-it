using System;

namespace HonccaBuildingGame.Classes.Inventories
{
	class Inventory
	{
		public Item[] Items;

		public Inventory()
		{
			Items = new Item[4];
		}

		/// <summary>
		/// Add a item to this inventory.
		/// </summary>
		/// <param name="itemToAdd">The item that should be added, count and name specified.</param>
		/// <param name="slotIndex">If you want a certain slot then put this here, leave blank if you want a free slot.</param>
		/// <returns>If the item got added or not.</returns>
		public bool AddItem(Item itemToAdd, int slotIndex = -1)
		{
			Item existingItem = GetItemWithName(itemToAdd.Name);

			if (existingItem.Name != "NONE")
			{
				slotIndex = existingItem.Slot;

				itemToAdd.Count += existingItem.Count;

				//Console.WriteLine("Adding new item, but already existing so adding count.");
			}
			else if (GetEmptySlot() == -1)
			{
				//Console.WriteLine("Inventory full, skip add.");

				return false;
			}

			if (slotIndex == -1)
			{
				//Console.WriteLine("No slot specified, just add in a free one.");

				slotIndex = GetEmptySlot();
			}

			Console.WriteLine($"Added item: {itemToAdd.Name} with count: {itemToAdd.Count} on slot: {slotIndex}");

			itemToAdd.Slot = slotIndex;

			Items[slotIndex] = itemToAdd;

			return true;
		}

		/// <summary>
		/// Remove a item from this inventory.
		/// </summary>
		/// <param name="itemToRemove">The item that should be removed, count and name specified.</param>
		/// <param name="slotIndex">If you want a certain slot then put this here, leave blank if you want a slot where this item exists.</param>
		/// <returns>If the item got removed or not.</returns>
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

					//Console.WriteLine($"Removed {itemToRemove.Name} on already existing one.");

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

				//Console.WriteLine($"Removed already existing {itemToRemove.Name} from count {Items[itemExists.Slot].Count + itemToRemove.Count} to: {Items[itemExists.Slot].Count} on slot: {itemExists.Slot}");
			}

			//Console.WriteLine($"Removed {itemToRemove.Name} with count {itemToRemove.Count}");

			return true;
		}

		/// <summary>
		/// This will give you a empty slot inside the inventory.
		/// </summary>
		/// <returns>A slot if there is any free one, -1 if inventory is full.</returns>
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

		/// <summary>
		/// Get item count from a certain itemName
		/// </summary>
		/// <param name="itemName">The itemName of the item you want.</param>
		/// <returns>A Item object with slot, count etc.</returns>
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

		/// <summary>
		/// If you need the item on a exact slot.
		/// </summary>
		/// <param name="slotIndex">The slot where the item is.</param>
		/// <returns>A item if a item exists on this slot.</returns>
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
