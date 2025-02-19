using System.Runtime.Serialization;
using System.Linq;

public static class PlayerData
{
    public enum SelectionType
    {
        [EnumMember(Value = "Radial")]
        Radial,
        [EnumMember(Value = "Sequential")]
        Sequential
    }

    // Settings
    public static SelectionType selectionType = SelectionType.Radial;
    public static bool wallSlideToggle = false;

    // Progression
    public static int highestChapter = 0;
    public static int highestLevel = 0;

    // Collectables
    public static int[] coins = Enumerable.Repeat(-1, 10).ToArray(); //new int[10];

    // Not Saved Variables
    // Scene Loading
    public static int pageToLoad;
}