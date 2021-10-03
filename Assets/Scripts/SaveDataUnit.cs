using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataUnit
{
    public int highestLevel;
    public PlayerData.SelectionType selectionType;

    public SaveDataUnit()
    {
        // Settings
        selectionType = PlayerData.selectionType;

        // Progression
        highestLevel = PlayerData.highestLevel;
    }
}
