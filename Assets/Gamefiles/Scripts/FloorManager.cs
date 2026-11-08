using System;
using System.Collections.Generic;
using DesignPatterns.CreationalPatterns;
using ProSM;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class FloorManager : Singleton<FloorManager>
{
    [Serializable]
    public class TaskData
    {
        public string msg;
        public bool isComplete;
        [Required] public GameObject go_taskGrouping;
        public UnityEvent onStartTask;
        public UnityEvent onCompleteTask;
    }

    
    
    public int deaths;
    
    [BoxGroup("Name Data")] public string internName;
    [BoxGroup("Name Data")] public List<string> usedNames = new();
    
    [BoxGroup("Tasks")] public List<TaskData> tasks = new();
    [BoxGroup("Tasks")] public int currentTask = 0;

    [Required] public Transform startPos;
    [Required] public DetectorMouseClick nameTagDetector;
    [Required] public WriteName writeName;
    
    public bool HasAlreadyUsedName(string internName) => usedNames.Contains(internName);

    public void AssignFirstTask()
    {
        if (currentTask != 0) return;
        tasks[0].onStartTask.Invoke();
        tasks[0].isComplete = false;
        tasks[0].go_taskGrouping.SetActive(true);
        SlackManager.Instance.SendSlackMsg(tasks[currentTask].msg);
    }
    
    [Button]
    public void CompleteCurrentTask()
    {
        tasks[currentTask].go_taskGrouping.SetActive(false);
        tasks[currentTask].isComplete = true;
        tasks[currentTask].onCompleteTask.Invoke();
        currentTask++;
        if (currentTask >= tasks.Count)
        {
            throw new NotImplementedException("COMPLETED ALL TASKS, NOT IMPLEMETNED YET");
        }
        tasks[currentTask].onStartTask.Invoke();
        tasks[currentTask].go_taskGrouping.SetActive(true);
        SlackManager.Instance.SendSlackMsg(tasks[currentTask].msg);
    }
    
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

    [Button]
    public void ResetDeath()
    {
        playerController.go_blackScreenDie.gameObject.SetActive(false);
        StartLevel();
        playerController.transform.position = startPos.position;
        playerController.transform.localRotation = Quaternion.Euler(0, -180, 0);
        nameTagDetector.ManualExit();
        internName = string.Empty;
        StartLevel();
        writeName.interactable = true;
        playerController.moveLocked = false;
        playerController.lookLocked = false;
        playerController.stopInputReading = false;
    }
    
    
}
