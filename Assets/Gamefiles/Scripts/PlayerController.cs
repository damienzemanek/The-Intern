
 using System;
 using System.Collections;
 using UnityEngine;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using static UnityEngine.CursorLockMode;

public class PlayerController : MonoBehaviour
{
    public bool moveLocked, lookLocked, stopInputReading;
    public float speed;
    public MouseLookEX.MouseLookSettings mouseLookSettings;
    [Required] public CharacterController charController;
    [Required] public Transform camTransform;

    public string defaultAnimName = "Default";
    [Required]  public Animator phoneAnimator;
    public string usePhoneAnimName = "Use";


    public KeyCode phoneKey;

    [Required] public GameObject go_blackScreenDie;

    public float deathFrameTime = 0.5f;
    
    void Awake() => CursorEX.Set(false, Locked);

    public void Die()
    {
        moveLocked = true;
        lookLocked = true;
        stopInputReading = true;
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            go_blackScreenDie.SetActive(true);
            CoroutineRunner.Instance.RunMethodDelayed(() =>
            {
                UIManager.Instance.Display(UIManager.FloorDisplays.DeathCRT);
                CursorEX.Set(true, None);
            }, 2f);
        }, deathFrameTime);
    }
    
    
    public void Update()
    { 
        if (Input.GetKeyDown(phoneKey))
        {
            phoneAnimator.gameObject.SetActive(true);
            if (UIManager.Instance.currentDisplay == UIManager.FloorDisplays.HUD)
            {
                phoneAnimator.PlayOnEnd(usePhoneAnimName, () =>
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
    }
}
