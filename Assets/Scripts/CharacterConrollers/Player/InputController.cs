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
    //TODO: Rip this out
    public DialogueTrigger dialogue;

    private int indexCounter;

    private enum InputStateTypes
    {
        PlayerControl,
        FlipDecisionPending,
        CameraPan,
        Menu,
        Dialogue
    }
    private InputStateTypes inputState;
    private InputStateTypes returnStateAfterPause;

    private KeyCode[] alphaKeyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

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
            case InputStateTypes.Dialogue:
                HandleDialogueInput();
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
            return;
        }
        //TODO: Rip this out
        if (Input.GetKeyDown("z"))
        {
            StateChange(InputStateTypes.Dialogue);
        }
        // End Rip out section

        if (Input.GetKeyDown("r"))
        {
            player.SetShouldReset(true);
        }
        if (Input.GetKeyDown("q"))
        {
            StateChange(InputStateTypes.CameraPan);
            return;
        }
        player.SetHorizontalMove(Input.GetAxisRaw("Horizontal"));
        if (PlayerData.wallSlideToggle)
        {
            //If it's toggle, swap value on a press
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                player.ToggleWallSlide();
            }
        } else
        {
            // Otherwise, just set wall slide allow to value of shift press
            player.SetHoldForWallSlide(Input.GetKey(KeyCode.LeftShift));
        }
        // We should jump if user is pressing jump button and the player is allowed to jump
        if ((Input.GetKeyDown("space")) && player.GetCanJump() && !player.GetShouldJump())
        {
            player.SetShouldJump(true);
        }
        if (Input.GetMouseButton(1) && player.GetCanFlip())
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
                indexCounter++; indexCounter %= levelManager.GetNumberPagesAvailable();
                levelManager.SetOnlyOpen(indexCounter);
            }
        }
    }
    
    private void HandleCameraPanInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateChange(InputStateTypes.Menu);
            return;
        }
        if (Input.GetKeyDown("q"))
        {
            StateChange(InputStateTypes.PlayerControl);
            return;
        }
        cam.SetHorizontalMove(Input.GetAxisRaw("Horizontal"));
        cam.SetVerticalMove(Input.GetAxisRaw("Vertical"));
        for (int i = 0; i < alphaKeyCodes.Length; i++)
        {
            if (Input.GetKeyDown(alphaKeyCodes[i]))
            {
                if(i < levelManager.GetNumberPagesAvailable())
                {
                    levelManager.SetOnlyOpen(i);
                }
                return;
            }
        }
    }

    private void HandleMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StateChange(returnStateAfterPause);
        }
    }

    private void HandleDialogueInput()
    {

    }

    private void StateChange(InputStateTypes newState)
    {
        ExitState(inputState);
        EnterState(newState);
        inputState = newState;
    }

    private void EnterState(InputStateTypes state)
    {
        switch (state)
        {
            case InputStateTypes.PlayerControl:
                EnterPlayerControlMode();
                break;
            case InputStateTypes.FlipDecisionPending:
                EnterFlipDecisionMode();
                break;
            case InputStateTypes.CameraPan:
                EnterPanMode();
                break;
            case InputStateTypes.Menu:
                EnterPauseMode();
                break;
            case InputStateTypes.Dialogue:
                EnterDialogueMode();
                break;
            default:
                break;
        }
    }

    private void ExitState(InputStateTypes state)
    {
        switch (state)
        {
            case InputStateTypes.PlayerControl:
                ExitPlayerControlMode();
                break;
            case InputStateTypes.FlipDecisionPending:
                ExitFlipDecisionMode();
                break;
            case InputStateTypes.CameraPan:
                ExitPanMode();
                break;
            case InputStateTypes.Menu:
                ExitPauseMode();
                break;
            case InputStateTypes.Dialogue:
                ExitDialogueMode();
                break;
            default:
                break;
        }
    }

    private void EnterPlayerControlMode()
    {
        levelManager.ResumeGame();
    }

    private void ExitPlayerControlMode()
    {

    }

    private void EnterFlipDecisionMode()
    {
        levelManager.PauseGame();
    }

    private void ExitFlipDecisionMode()
    {

    }

    private void EnterPanMode()
    {
        levelManager.PauseGame();
        gui.OpenLookthrough();
        cam.SetPanMode(true);
    }

    private void ExitPanMode()
    {
        cam.SetPanMode(false);
        cam.SetHorizontalMove(0);
        cam.SetVerticalMove(0);
        levelManager.SetOnlyOpen(levelManager.GetCurrentPage());
        gui.CloseLookthrough();
    }

    private void EnterPauseMode()
    {
        returnStateAfterPause = inputState;
        levelManager.PauseGame();
        gui.SetMenuActive(true);
    }

    private void ExitPauseMode()
    {
        gui.SetMenuActive(false);
    }

    private void EnterDialogueMode()
    {
        levelManager.PauseGame();
        gui.DialogueOpen();
        dialogue.TriggerDialogue();
    }

    private void ExitDialogueMode()
    {

    }

    public void InitiateFlip()
    {
        //levelManager.PauseGame();
        StateChange(InputStateTypes.FlipDecisionPending);
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

    public void DialogueComplete()
    {
        StateChange(InputStateTypes.PlayerControl);
    }

    public void PauseFinished()
    {
        StateChange(returnStateAfterPause);
    }
}
