using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all player input and routes to needed controllers

public class InputController : MonoBehaviour
{
    public PlayerController player;
    public LevelManager levelManager;

    private int indexCounter;

    private enum InputStateTypes
    {
        PlayerControl,
        FlipDecisionPending
    }
    private InputStateTypes inputState;

    // Start is called before the first frame update
    void Start()
    {
        inputState = InputStateTypes.PlayerControl;
    }

    // Update is called once per frame
    void Update()
    {
        switch (inputState)
        {
            case InputStateTypes.PlayerControl:
                if (Input.GetKeyDown("r"))
                {
                    player.SetShouldReset(true);
                }
                player.SetHorizontalMove(Input.GetAxis("Horizontal"));
                // We should jump if user is pressing jump button and the player is allowed to jump
                if((Input.GetKeyDown("space")) && player.GetCanJump() && ! player.GetShouldJump())
                {
                    player.SetShouldJump(true);
                }
                if (Input.GetMouseButtonDown(1) && player.GetCanFlip())
                {
                    indexCounter = levelManager.GetCurrentPage();
                    Debug.Log("Cur index: " + indexCounter);
                    player.SetShouldFlip(true);
                }
                break;
            case InputStateTypes.FlipDecisionPending:
                if (Input.GetMouseButtonUp(1))
                {
                    CompleteFlip(indexCounter);
                }
                if (Input.GetMouseButtonDown(0))
                {
                    levelManager.Close(indexCounter);
                    indexCounter++; indexCounter %= levelManager.GetTotalPages();
                    Debug.Log("indexCounter:" + indexCounter);
                    levelManager.Open(indexCounter);
                }
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        
    }

    private void StateChange(InputStateTypes newState)
    {
        inputState = newState;
    }

    public void InitiateFlip()
    {
        levelManager.PauseGame();
        StateChange(InputStateTypes.FlipDecisionPending);
    }

    private void CompleteFlip(int index)
    {
        if( index >= 0) // -1 index means cancel flip
        {
            if (levelManager.FlipLevel(index))
            {
                player.FlipSuccess();
            } else
            {
                levelManager.Open(levelManager.GetCurrentPage());
            }
        }
        levelManager.ResumeGame();
        StateChange(InputStateTypes.PlayerControl);
    }
}
