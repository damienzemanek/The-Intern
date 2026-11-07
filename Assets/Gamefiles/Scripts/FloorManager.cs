using System;
using System.Collections.Generic;
using DesignPatterns.CreationalPatterns;
using ProSM;
using Sirenix.OdinInspector;
using UnityEngine;

public class FloorManager : Singleton<FloorManager>
{
    public int deaths;
    public string internName;

    public List<string> usedNames = new();
    
    
    public bool HasAlreadyUsedName(string internName) => usedNames.Contains(internName);
    
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

    private void Start() => StartLevel();

    [Button]
    public void StartLevel()
    {
        floorState = FloorStates.Elevator;
    }

    [Button]
    public void E_CreatedNameComplete(string internName)
    {
        floorState = FloorStates.CreatedNameComplete;
        elevatorDoorAnimator.Play("Open");
        this.internName = internName;
        usedNames.Add(internName);
    }
    
    [Button]
    public void E_ExitElevator()
    {
        elevatorDoorAnimator.Play("Close");
    }
    
    
}
