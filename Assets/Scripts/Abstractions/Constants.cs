using System.Collections;
using System;
using UnityEngine;

public static class Constants
{
  [Flags] public enum ElementTypes
  {
    Fire      = 1,
    Water     = 2,
    Earth     = 4,
    Air       = 8,
    Electric  = 16,
    None      = 0
  }

  [Flags] public enum Status
  {
    normal  = 0,
    onFire  = 1,
    wet     = 2,
    stunned = 4,
    slowed  = 8
  }

  [Flags] public enum ItemRarity
  {
    common    = 1,
    uncommon  = 2,
    rare      = 4,
    legendary = 8,
    divine    = 16
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
    private uint ID; //uint is a 32-bit value

    public bool active;
    public ItemRarity rarity;
    [Range(0, 256)]
    public uint uniqueID;

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
      active = false;
      rarity = 0;
      uniqueID = 0;
    }

    public ItemID(uint i)
    {
      ID = i;
      active = ((ID & activeMask) >> activeBitShift) == 1 ? true : false;
      rarity = (ItemRarity)((ID & rarityMask) >> rarityBitShift);
      uniqueID = (ID & uniqueID) >> uniqueIDBitShift;
    }

    public ItemID(bool a, ItemRarity r, uint uID)
    {
      active = a;
      rarity = r;
      uniqueID = (uID & uniqueIDMask);
      UpdateId();
    }

    public bool IsEquipment()
    {
      return active;
    }

    public ItemRarity GetRarity()
    {
      return rarity;
    }

    public uint GetUniqueID()
    {
      return uniqueID;
    }

    public uint GetID()
    {
      return ID;
    }
    
    private void UpdateId()
    {
      ID = ((uint)(active == true ? 1 : 0) << activeBitShift) + ((uint)rarity << rarityBitShift) + (uniqueID << uniqueIDBitShift);
    }

    public int CompareTo(object obj)
    {
      if (obj == null)
        return 1;
      UpdateId();
      ItemID otherItem = obj as ItemID;
      if (otherItem != null)
      {
        return this.ID.CompareTo(otherItem.GetID());
      }
      else
        throw new ArgumentException("Object is not an ItemID");
    }
  }


}
