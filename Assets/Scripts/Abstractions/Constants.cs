using System.Collections;
using System;

public static class Constants
{
  public enum ElementTypes
  {
    Fire = 0b1,
    Water = 0b10,
    Earth = 0b100,
    Air = 0b1000,
    Electric = 0b10000,
    None = 0b100000,
    NUM_ELEMENTS = 6 //Counter / number of all element types above
  }

  public enum Status
  {
    normal = 0b0000000000000000,
    onFire = 0b0000000000000001,
    wet = 0b0000000000000010,
    stunned = 0b0000000000000100,
    slowed = 0b0000000000001000
  }

  public enum ItemRarity
  {
    common = 0b000,
    uncommon = 0b001,
    rare = 0b010,
    legendary = 0b011,
    divine = 0b100
  }

  [System.Serializable]
  public enum StatItemModifies
  {
    moveSpeed         = 0b0000000000000000,
    sprintMultiplier  = 0b0000000000000001,
    jumpHeight        = 0b0000000000000010,
    numJumps          = 0b0000000000000100,
    maxHealth         = 0b0000000000001000,
    healthRegen       = 0b0000000000010000,
    armor             = 0b0000000000100000,
    fireResist        = 0b0000000001000000,
    waterResist       = 0b0000000010000000,
    earthResist       = 0b0000000100000000,
    airResist         = 0b0000001000000000,
    electricResist    = 0b0000010000000000,
    attackSpeed       = 0b0000100000000000,
    damage            = 0b0001000000000000,
    dodgeChance       = 0b0010000000000000
  }

  [System.Serializable]
  public class ItemID : IComparable
  {
    public uint ID; //uint is a 32-bit value

    private static uint activeBitWidth = 0b1;
    private static int activeBitShift = 11;
    private static uint activeMask = activeBitWidth << activeBitShift;

    private static uint rarityBitWidth = 0b111;
    private static int rarityBitShift = 8;
    private static uint rarityMask = rarityBitWidth << rarityBitShift;

    private static uint uniqueIDBitWidth = 0b11111111;
    private static int uniqueIDBitShift = 0;
    private static uint uniqueIDMask = uniqueIDBitWidth << uniqueIDBitShift; //TODO: Think of a better name for this

    public ItemID()
    {
      ID = uint.MaxValue;
    }

    public ItemID(uint i)
    {
      ID = i;
    }

    public ItemID(bool active, uint rarity, uint uniqueID)
    {
      uint a = (uint)(active ? 1 : 0);
      ID = (a << activeBitShift) + (rarity << rarityBitShift) + (uniqueID << uniqueIDBitShift);
    }

    public bool IsEquipment()
    {
      return (ID & activeMask) == (0b1 << activeBitShift) ? true : false;
    }

    public ItemRarity GetRarity()
    {
      int temp = (int)(ID & rarityMask);
      temp = temp >> rarityBitShift;
      ItemRarity temp2 = (ItemRarity)temp;

      return (ItemRarity)((ID & rarityMask) >> rarityBitShift);
    }

    public uint GetUniqueID()
    {
      return (ID & uniqueIDMask) >> uniqueIDBitShift;
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;

      ItemID otherItemId = obj as ItemID;
      if (otherItemId != null)
      {
        return this.ID.CompareTo(otherItemId.ID);
      }
      else
        throw new ArgumentException("Object is no an ItemID");
    }
  }

  //TODO: There must be a better way for us to do this
  //This could be an X Y problem ?
  //I am storing these values so that we can initialize our sorted dict inventory for each valid ItemID
  //We need someway to get those ItemIDs and know what is valid. This quick and dirty solution solves that by just hardcoding them directly

  public static ItemID firstCommonItem = new ItemID(0);
  public static ItemID lastCommonItem = new ItemID(0);

  public static ItemID firstUncommonItem = new ItemID(256);
  public static ItemID lastUncommonItem = new ItemID(256);

  public static ItemID firstRareItem = new ItemID(512);
  public static ItemID lastRareItem = new ItemID(512);

  public static ItemID firstLegendaryItem = new ItemID(768);
  public static ItemID lastLegendaryItem = new ItemID(768);

  public static ItemID firstDivineItem = new ItemID(1024);
  public static ItemID lastDivineItem = new ItemID(1024);

  public static ItemID firstEquipmentItem = new ItemID(2048);
  public static ItemID lastEquipmentItem = new ItemID(2048);

  public static ItemID[,] startAndEndOfItemIDByRarirty = new ItemID[,]{ { firstCommonItem, firstUncommonItem, firstRareItem, firstLegendaryItem, firstDivineItem, firstEquipmentItem }, 
                                                                       { lastCommonItem, lastUncommonItem, lastRareItem, lastLegendaryItem, lastDivineItem, lastEquipmentItem } };

}
