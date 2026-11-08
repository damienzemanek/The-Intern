using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class Ask : MonoBehaviour
{
    public Animator clarenceAnimator;
    public string shootAnimName = "Shoot";
    public string defaultAnimName = "Default";
    public Transform forcePosition;
    public Vector3 headRotation = new Vector3(24, 0, 0);

    public float askTime = 3f;
    public float shootTime = 7f;

    public string askMsg = "Hey Clarence, Is your coffee good?";
    public string clarenceResponse = "Clarence: ...";

    [Button]
    public void TalkToClarence() => StartCoroutine(C_TalkToClarence());
    
    public IEnumerator C_TalkToClarence()
    {
        FloorManager.Instance.playerController.moveLocked = true;
        FloorManager.Instance.playerController.lookLocked = true;
        FloorManager.Instance.playerController.transform.DOMove(forcePosition.position, 1f);
        FloorManager.Instance.playerController.MoveHeadTo(headRotation);

        yield return new WaitForSeconds(1f);

        bool finishedTalking = false;
        UIManager.Instance.SpeakOrCommentate(askMsg, 2f, () => finishedTalking = true);
        yield return new WaitUntil(() => finishedTalking);
        
        finishedTalking = false;
        UIManager.Instance.SpeakOrCommentate(clarenceResponse, 2f, () => finishedTalking = true);
        yield return new WaitUntil(() => finishedTalking);
        
        clarenceAnimator.Play(shootAnimName);

        yield return new WaitForSeconds(shootTime);
        
        FloorManager.Instance.playerController.Die();
        
        yield return new WaitForSeconds(2);

        clarenceAnimator.Play(defaultAnimName);

    }
    
    
    
}
