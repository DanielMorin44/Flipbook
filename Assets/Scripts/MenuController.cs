using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{

    InputController input;

    // Start is called before the first frame update
    void Start()
    {
        input = GameObject.FindObjectOfType<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResumePressed()
    {
        input.LeaveMenuState();
    }

    public void ReturnToMenuPressed()
    {
        Debug.Log("Return To Menu Not Implemented");
    }

    public void SavePressed()
    {
        Debug.Log("Save Not Implemented");
    }

    public void QuitPressed()
    {
        Application.Quit();
    }
}
