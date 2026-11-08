using System;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool locked = false;
    public Transform doorTransform;
    public float openTime = 1f;
    public Vector3 origRot;
    public Vector3 openRot;

    private void Start() => ResetRot();
    
    public void ResetRot() => doorTransform.localRotation = Quaternion.Euler(openRot);

    public void OpenDoor()
    {
        if (locked) return;
        doorTransform.DORotate(openRot, openTime);
    }

    public void CloseDoor()
    {
        doorTransform.DORotate(origRot, openTime);
    }
    
}
