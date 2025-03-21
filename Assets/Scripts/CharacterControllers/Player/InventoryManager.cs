using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    private int coinId = -1;
    private int keys = 0;
    private bool flip = false;
    private List<Collectible> inventory = new List<Collectible>();

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

    public void AddKey(KeyController key)
    {
        Debug.Log(inventory.Count);
        keys++;
        AddItemToInventory(key);
    }

    public bool Unlock()
    {
        Collectible token = inventory.Find(x => { return x is KeyController; });
        if (token == null)
        {
            return false;
        }
        keys--;
        RemoveItemFromInventory(token);
        Destroy(token.gameObject);
        return true;
    }

    public bool GetFlipToken()
    {
        return flip;
    }

    public bool AddFlipToken(FlipTokenController token)
    {
        if (flip)
        {
            return false;
        }
        flip = true;
        AddItemToInventory(token);
        return true;
    }

    public void RemoveFlipToken()
    {
        flip = false;
        Collectible token = inventory.Find(x => { return x is FlipTokenController; });
        if (token == null)
        {
            return;
        }
        RemoveItemFromInventory(token);
        Destroy(token.gameObject);
    }

    public void PickUpCoin(CoinController coin)
    {
        SetCoinId(coin.id);
        AddItemToInventory(coin);
    }

    private void AddItemToInventory(Collectible item)
    {
        if (inventory.Count == 0)
        {
            item.SetFollowTarget(transform);
        } else
        {
            Debug.Log(inventory.Count);
            item.SetFollowTarget(inventory.Last().transform);
        }
        inventory.Add(item);
    }
    
    private void RemoveItemFromInventory(Collectible item)
    {
        Collectible nextItem = inventory.SkipWhile(x => x != item).Skip(1).DefaultIfEmpty(null).FirstOrDefault();
        if(nextItem != null)
        {
            Collectible prevItem = inventory.TakeWhile(x => x != item).DefaultIfEmpty(null).LastOrDefault();
            Transform toFollowTarget = prevItem == null ? transform : prevItem.transform;
            nextItem.SetFollowTarget(toFollowTarget);
        }
        inventory.Remove(item);
    }
}
