using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manages all player input and routes to needed controllers

public class InputController : MonoBehaviour
{
    public PlayerController player;
    public CameraController cam;
    public LevelManager levelManager;
    public GUIController gui;

    private int indexCounter;

    private enum InputStateTypes
    {
        PlayerControl,
        FlipDecisionPending,
        CameraPan,
        Menu
    }
    private InputStateTypes inputState;
    private InputStateTypes returnStateAfterPause;

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
                HandlePlayerControlInput();
                break;
            case InputStateTypes.FlipDecisionPending:
                HandleFlipDecisionPendingInput();
                break;
            case InputStateTypes.CameraPan:
                HandleCameraPanInput();
                break;
            case InputStateTypes.Menu:
                HandleMenuInput();
                break;
            default:
                break;
        }
    }

    private void HandlePlayerControlInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateChange(InputStateTypes.Menu);
            returnStateAfterPause = InputStateTypes.PlayerControl;
            levelManager.PauseGame();
            gui.SetMenuActive(true);
            return;
        }
        if (Input.GetKeyDown("r"))
        {
            player.SetShouldReset(true);
        }
        if (Input.GetKeyDown("l"))
        {
            EnterPanMode();
            StateChange(InputStateTypes.CameraPan);
            return;
        }
        player.SetHorizontalMove(Input.GetAxisRaw("Horizontal"));
        // We should jump if user is pressing jump button and the player is allowed to jump
        if ((Input.GetKeyDown("space")) && player.GetCanJump() && !player.GetShouldJump())
        {
            player.SetShouldJump(true);
        }
        if (Input.GetMouseButtonDown(1) && player.GetCanFlip())
        {
            indexCounter = levelManager.GetCurrentPage();
            if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
            {
                gui.OpenSelector();
            }
            player.SetShouldFlip(true);
        }
    }

    private void HandleFlipDecisionPendingInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateChange(InputStateTypes.Menu);
            returnStateAfterPause = InputStateTypes.FlipDecisionPending;
            gui.SetMenuActive(true);
            return;
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (PlayerData.selectionType == PlayerData.SelectionType.Radial)
            {
                gui.CloseSelector();
            }
            if (PlayerData.selectionType == PlayerData.SelectionType.Sequential)
            {
                CompleteFlip(indexCounter);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerData.selectionType == PlayerData.SelectionType.Sequential)
            {
                levelManager.Close(indexCounter);
                indexCounter++; indexCounter %= levelManager.GetTotalPages();
                levelManager.Open(indexCounter);
            }
        }
    }
    
    private void HandleCameraPanInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateChange(InputStateTypes.Menu);
            returnStateAfterPause = InputStateTypes.CameraPan;
            gui.SetMenuActive(true);
            return;
        }
        if (Input.GetKeyDown("l"))
        {
            ExitPanMode();
            StateChange(InputStateTypes.PlayerControl);
            return;
        }
        cam.SetHorizontalMove(Input.GetAxisRaw("Horizontal"));
        cam.SetVerticalMove(Input.GetAxisRaw("Vertical"));
    }

    private void HandleMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveMenuState();
        }
    }

    private void StateChange(InputStateTypes newState)
    {
        inputState = newState;
    }

    private void EnterPanMode()
    {
        levelManager.PauseGame();
        cam.SetPanMode(true);
    }

    private void ExitPanMode()
    {
        cam.SetPanMode(false);
        cam.SetHorizontalMove(0);
        cam.SetVerticalMove(0);
        levelManager.ResumeGame();
    }

    public void InitiateFlip()
    {
        levelManager.PauseGame();
        StateChange(InputStateTypes.FlipDecisionPending);
    }

    public void LeaveMenuState()
    {
        StateChange(returnStateAfterPause);
        if (returnStateAfterPause != InputStateTypes.CameraPan &&
            returnStateAfterPause != InputStateTypes.FlipDecisionPending)
        {
            levelManager.ResumeGame();
        }
        gui.SetMenuActive(false);
    }

    public void CompleteFlip(int index)
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

    public bool IsPlayerControl()
    {
        return inputState == InputStateTypes.PlayerControl;
    }
}
