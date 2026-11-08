using System;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public int loadIndex = 2;
    public LoadSceneConnector loadAdap;
    public bool leaving = false;

    private void Awake()
    {
        leaving = false;
    }

    public void LeaveFloor()
    {
        if (leaving) return;
        leaving = true;
        FloorManager.Instance.playerController.stopInputReading = true;
        loadAdap.Load(loadIndex);
    }
}
