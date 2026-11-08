
 using System;
 using System.Collections;
 using DG.Tweening;
 using UnityEngine;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using static AudioManager;
using static UnityEngine.CursorLockMode;

public class PlayerController : MonoBehaviour
{
    public bool cantUsePhone = false;
    public bool moveLocked, lookLocked, stopInputReading;
    public float speed;
    public MouseLookEX.MouseLookSettings mouseLookSettings;
    [Required] public CharacterController charController;
    [Required] public Transform camTransform;

    public string defaultAnimName = "Default";
    [Required]  public Animator phoneAnimator;
    public string usePhoneAnimName = "Use";
    
    [Header("Footsteps")]
    public float footstepInterval = 0.4f;
    private float footstepTimer;

    public KeyCode phoneKey;

    [Required] public GameObject go_blackScreenDie;
    [Required] public GameObject pr_skeletonCorpse;

    public float deathFrameTime = 0.5f;
    
    void Awake() => CursorEX.Set(false, Locked);

    public void SetStopInputReading(bool v) => stopInputReading = v;

    [Button]
    public void Die()
    {
        moveLocked = true;
        lookLocked = true;
        stopInputReading = true;
        Source(SourceTrack.Long2).Stop();
        Source(SourceTrack.Footsteps).Stop();
        Source(SourceTrack.Long1).Stop();
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            go_blackScreenDie.SetActive(true);
            var corpse = Instantiate(pr_skeletonCorpse, camTransform.position, Quaternion.identity);
            corpse.transform.parent = null;
            CoroutineRunner.Instance.RunMethodDelayed(() =>
            {
                UIManager.Instance.Display(UIManager.FloorDisplays.DeathCRT);
                CursorEX.Set(true, None);
            }, 2f);
        }, deathFrameTime);
    }

    private void LateUpdate()
    {
        transform.position = transform.position.With(y: 0);
        if(moveLocked || stopInputReading)
            Source(SourceTrack.Footsteps).Stop();
    }

    public void MoveHeadTo(Vector3 rot)
    {
        mouseLookSettings.head.transform.DOLocalRotate(rot, 1f);
    }


    public void Update()
    {
        if (Input.GetKeyDown(phoneKey) && !cantUsePhone)
        {
            phoneAnimator.gameObject.SetActive(true);
            if (UIManager.Instance.currentDisplay == UIManager.FloorDisplays.HUD)
            {
                phoneAnimator.CrossFadeOnEnd(usePhoneAnimName, 0.2f, () =>
                {
                    UIManager.Instance.Display(UIManager.FloorDisplays.Phone);
                });
            }
            else if(UIManager.Instance.currentDisplay == UIManager.FloorDisplays.Phone 
                    || UIManager.Instance.currentDisplay == UIManager.FloorDisplays.Slack)
            {
                UIManager.Instance.Display(UIManager.FloorDisplays.HUD);
                phoneAnimator.CrossFade(defaultAnimName, 0.6f);
            }
        }
        
        
        if (stopInputReading) return;
        if(!lookLocked) mouseLookSettings.UpdateMouseLook(Input.mousePositionDelta);
        Move();
    }
    
    void Move()
    {
        if(moveLocked) return;
        
        var yInputScalar = Input.GetAxis("Vertical");
        var xInputScalar = Input.GetAxis("Horizontal");
        var fwd = camTransform.forward * yInputScalar;
        var right = camTransform.right * xInputScalar;
        var moveDir = fwd + right;
        moveDir.y = 0;
        charController.Move(moveDir * speed * Time.deltaTime);
        //transform.Translate(moveDir * speed * Time.deltaTime);

        var source = UseSourceWithClip(SourceTrack.Footsteps, AudioClipType.FootSteps1, out var clip);
        // Continuous footsteps
        if (moveDir.sqrMagnitude > 0.01f)
        {
            if (source.isPlaying) return;
            source.loop = true;
            source.clip = clip;
            source.volume = 0.2f;
            source.pitch = 1.2f;
            source.Play();
        }
        else
            source.Stop();
    }
}
