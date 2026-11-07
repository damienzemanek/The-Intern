using System;
using DesignPatterns.CreationalPatterns;
using ProSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{ 
    public enum FloorStates
    {
        Elevator,
        CreatedNameComplete,
        Tasks,
        Dead,
        TasksComplete,
    }

    public FloorStates floorState;
    [Required] public Animator elevatorDoorAnimator;
    [Required] public PlayerController playerController;
    
    public void StartLevel()
    {
        floorState = FloorStates.Elevator;
        
    }

    public void E_CreatedNameComplete()
    {
        floorState = FloorStates.CreatedNameComplete;
        elevatorDoorAnimator.Play("Open");
    }

    public void E_ExitElevator()
    {
        elevatorDoorAnimator.Play("Close");
    }
    
    
}
