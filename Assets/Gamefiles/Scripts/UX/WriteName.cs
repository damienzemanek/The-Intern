using System;
using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static AudioManager;
using static UIManager;

public class WriteName : MonoBehaviour
{
    [Required] public GameObject go_PickDiffName;
    [Required] public TMP_InputField ip_txtName;
    public bool interactable = true;

    private void Awake()
    {
        go_PickDiffName.gameObject.SetActive(false);
    }

    public void InteractWithNameTag()
    {
        if (!interactable) return;
        Singleton<UIManager>.Instance.Display(FloorDisplays.WriteName);
        FloorManager.Instance.playerController.lookLocked = true;
        FloorManager.Instance.playerController.moveLocked = true;
        CursorEX.Set(true, CursorLockMode.None);
        interactable = false;
        OnCompleteUITransition.Invoke();
        Debug.Log("Showing INteract");

    }
    
    public void OnWriteNameComplete()
    {
        if (FloorManager.Instance.HasAlreadyUsedName(ip_txtName.text))
        {
            go_PickDiffName.gameObject.SetActive(true);
            return;
        }

        FloorManager.Instance.E_CreatedNameComplete(ip_txtName.text);
        FloorManager.Instance.playerController.lookLocked = false;
        FloorManager.Instance.playerController.moveLocked = false;
        Singleton<UIManager>.Instance.Display(FloorDisplays.HUD);
        CursorEX.Set(false, CursorLockMode.Locked);
        UseSourceWithClip(SourceTrack.Player, AudioClipType.Nametag,
            out var clip).PlayOneShot(clip);
    }

    public void AlreadyUsedName()
    {
        
    }
}

