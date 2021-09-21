using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveDataUnit
{
    public int highestLevel;

    public SaveDataUnit()
    {
        highestLevel = PlayerData.highestLevel;
    }
}
