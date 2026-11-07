using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using static UIManager;

public class WriteName : MonoBehaviour
{
    [Required] public TMP_InputField ip_txtName;
    public bool interactable = true;
    
    public void InteractWithNameTag()
    {
        if (!interactable) return;
        Singleton<UIManager>.Instance.Display(FloorDisplays.WriteName);
        FloorManager.Instance.playerController.lookLocked = true;
        FloorManager.Instance.playerController.moveLocked = true;
        CursorEX.Set(true, CursorLockMode.None);
        interactable = false;
    }
    
    public void OnWriteNameComplete()
    {
        FloorManager.Instance.E_CreatedNameComplete(ip_txtName.text);
        FloorManager.Instance.playerController.lookLocked = false;
        FloorManager.Instance.playerController.moveLocked = false;
        Singleton<UIManager>.Instance.Display(FloorDisplays.HUD);
        CursorEX.Set(false, CursorLockMode.Locked);
    }
}
