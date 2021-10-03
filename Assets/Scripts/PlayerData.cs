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

    // Progression
    public static int highestLevel = 0;
}