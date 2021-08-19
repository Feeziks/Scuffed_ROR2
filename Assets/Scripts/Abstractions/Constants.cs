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
  [Flags] public enum StatItemModifies
  {
    none              = 0,
    moveSpeed         = 1,
    sprintMultiplier  = 2,
    jumpHeight        = 4,
    numJumps          = 8,
    maxHealth         = 16,
    healthRegen       = 32,
    armor             = 64,
    fireResist        = 128,
    waterResist       = 256,
    earthResist       = 512,
    airResist         = 1024,
    electricResist    = 2048,
    attackSpeed       = 4096,
    damage            = 8192,
    critChance        = 16384,
    dodgeChance       = 32768
  }  //If this is edited must ALSO edit modified settings cs to match!!!!!!

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
}
