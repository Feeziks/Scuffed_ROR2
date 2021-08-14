using System.Collections;

public static class Constants
{
  public enum ElementTypes
  {
    Fire,
    Water,
    Earth,
    Air,
    Electric,
    None
  }

  public enum Status
  {
    normal  = 0b0000000000000000,
    onFire  = 0b0000000000000001,
    wet     = 0b0000000000000010,
    stunned = 0b0000000000000100,
    slowed  = 0b0000000000001000
  }
}
