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
    public int highestChapter;
    public int highestLevel;

    // Collectables
    public int[] coins;

    public SaveDataUnit()
    {
        // Settings
        selectionType = PlayerData.selectionType;
        wallSlideToggle = PlayerData.wallSlideToggle;

        // Progression
        highestChapter = PlayerData.highestChapter;
        highestLevel = PlayerData.highestLevel;

        // Collectables
        coins = PlayerData.coins;
    }
}
