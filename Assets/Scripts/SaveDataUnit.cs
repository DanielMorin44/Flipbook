using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataUnit
{
    //Settings
    public PlayerData.SelectionType selectionType;
    public bool wallSlideToggle;

    //Progress
    public int highestLevel;

    public SaveDataUnit()
    {
        // Settings
        selectionType = PlayerData.selectionType;
        wallSlideToggle = PlayerData.wallSlideToggle;

        // Progression
        highestLevel = PlayerData.highestLevel;
    }
}
