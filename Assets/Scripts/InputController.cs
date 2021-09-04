using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all player input and routes to needed controllers

public class InputController : MonoBehaviour
{
    public PlayerController player;
    public LevelManager levelManager;

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
                if (Input.GetKeyDown("f") && player.GetCanFlip())
                {
                    player.SetShouldFlip(true);
                }
                break;
            case InputStateTypes.FlipDecisionPending:
                if (Input.GetKeyDown("1"))
                {
                    CompleteFlip(1);
                }
                else if (Input.GetKeyDown("2"))
                {
                    CompleteFlip(2);
                }
                else if (Input.GetKeyDown("3"))
                {
                    CompleteFlip(3);
                }
                else if (Input.GetKeyDown("4"))
                {
                    CompleteFlip(4);
                }
                else if (Input.GetKeyDown("5"))
                {
                    CompleteFlip(5);
                }
                else if (Input.GetKeyDown("6"))
                {
                    CompleteFlip(6);
                }
                else if (Input.GetKeyDown("7"))
                {
                    CompleteFlip(7);
                }
                else if (Input.GetKeyDown("8"))
                {
                    CompleteFlip(8);
                }
                else if (Input.GetKeyDown("9"))
                {
                    CompleteFlip(9);
                }
                else if (Input.GetKeyDown("0"))
                {
                    CompleteFlip(0);
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
        if (levelManager.FlipLevel(index))
        {
            player.FlipSuccess();
        }
        levelManager.ResumeGame();
        StateChange(InputStateTypes.PlayerControl);
    }
}
