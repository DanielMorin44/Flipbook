using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private int coinId = -1;
    private int keys = 0;
    private bool flip = false;

    public int GetCoinId()
    {
        return coinId;
    }

    public bool HasCoin()
    {
        return coinId != -1;
    }

    public bool SetCoinId(int id)
    {
        if (coinId == -1)
        {
            coinId = id;
            return true;
        }
        return false;
    }

    public int GetKeys()
    {
        return keys;
    }

    public void AddKey()
    {
        keys++;
    }

    public void SetKeys(int newKeys)
    {
        keys = newKeys;
    }

    public bool Unlock()
    {
        if (keys <= 0)
        {
            return false;
        }
        keys--;
        return true;
    }

    public bool GetFlipToken()
    {
        return flip;
    }

    public bool AddFlipToken()
    {
        if (flip)
        {
            return false;
        }
        flip = true;
        return true;
    }

    public void RemoveFlipToken()
    {
        flip = false;
    }
}
