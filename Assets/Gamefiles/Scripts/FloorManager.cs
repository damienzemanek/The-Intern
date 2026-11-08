using System;
using System.Collections.Generic;
using System.Linq;
using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
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

    [Serializable]
    public class InternData
    {
        [ReadOnly] public string internName;
        public string deathReason;
        public Sprite spr_deathStickyNote;
    }

    
    
    public int currentIntern;
    [BoxGroup("Name Data")] public List<InternData> internData = new();
    
    [BoxGroup("Tasks")] public List<TaskData> tasks = new();
    [BoxGroup("Tasks")] public int currentTask = 0;

    [Required] public Transform startPos;
    [Required] public DetectorMouseClick nameTagDetector;
    [Required] public WriteName writeName;
    [Required] public GameObject trigger_exitElevator;
    public List<Door> doors = new();
    
    public bool HasAlreadyUsedName(string internName) => internData.Any(d => d.internName == internName);

    public void AssignFirstTask()
    {
        if (currentTask != 0) return;
        currentIntern = 0;
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
        internData[currentIntern].internName = internName;
    }
    
    [Button]
    public void E_ExitElevator()
    {
        elevatorDoorAnimator.Play("Close");
    }
    
    [Button]
    public void ResetDeath()
    {

        elevatorDoorAnimator.Play("Close", 0, 1f);
        playerController.go_blackScreenDie.gameObject.SetActive(false);
        UIManager.Instance.Display(UIManager.FloorDisplays.HUD);
        StartLevel();
        nameTagDetector.ManualExit();

        writeName.interactable = true;
        writeName.ip_txtName.text = "";

        playerController.moveLocked = false;
        playerController.lookLocked = false;
        playerController.stopInputReading = false;

        trigger_exitElevator.gameObject.SetActive(true);

        currentIntern++;
        
        CharacterController cc = playerController.Get<CharacterController>();
        cc.enabled = false;
        playerController.transform.SetPositionAndRotation(
            startPos.position,
            startPos.rotation
        );
        
        cc.enabled = true;

        Debug.Log("Reset death");
        
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            doors.ForEach(d =>
            {
                d.Get<DetectorTrigger>().ManualExit();
            });
        }, 0.5f);
        
        if (currentIntern == 1)
        {
            CoroutineRunner.Instance.RunMethodDelayed(() =>
            {
                SlackManager.Instance.SendSlackMsg(SlackManager.Instance.firstDeathSlackMsg);
            }, 1.4f);
        }

        if (currentIntern == 3)
        {
            CoroutineRunner.Instance.RunMethodDelayed(() => CompleteCurrentTask(), 3f);
        }
        

    }
    
    
}
