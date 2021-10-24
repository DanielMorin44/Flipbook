using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;

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
    public static int highestLevel = 0;

    // Scene Loading
    public static int pageToLoad;
}