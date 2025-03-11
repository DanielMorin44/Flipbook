using UnityEngine;

public class SwitchBlockController : MonoBehaviour
{
    public GameObject[] blocks;
    public void Activate(bool enabled)
    {

        foreach (GameObject obj in blocks)
        {
            obj.SetActive(enabled);
            
        }
    }
}
