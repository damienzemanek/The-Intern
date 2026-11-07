using DesignPatterns.CreationalPatterns;
using UnityEngine;
using static UIManager;

public class WriteName : MonoBehaviour
{
    
    public void InteractWithNameTag()
    {
        Singleton<UIManager>.Instance.Display(FloorDisplays.WriteName);
        FloorManager.Instance.playerController.lookLocked = true;
        FloorManager.Instance.playerController.moveLocked = true;
    }
    
    public void OnWriteNameComplete(string txt_internName)
    {
        FloorManager.Instance.E_CreatedNameComplete(txt_internName);
        FloorManager.Instance.playerController.lookLocked = false;
        FloorManager.Instance.playerController.moveLocked = false;
    }
}
