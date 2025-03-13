using UnityEngine;

public class SwitchBlockController : MonoBehaviour
{
    public GameObject[] blocks;
    public void Flip()
    {

        foreach (GameObject obj in blocks)
        {
            obj.SetActive(!obj.activeSelf);
            
        }
    }
}
