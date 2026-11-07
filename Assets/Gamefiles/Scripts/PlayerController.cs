using System;
using UnityEngine;
using EMILtools.Extensions;
using static UnityEngine.CursorLockMode;

public class PlayerController : MonoBehaviour
{
    public float speed;
    
    public MouseLookEX.MouseLookSettings mouseLookSettings;
    public CharacterController charController;
    public Transform camTransform;

    void Awake() => CursorEX.Set(false, Locked);

    public void Update()
    {
        mouseLookSettings.UpdateMouseLook(Input.mousePositionDelta);
        Move();
    }


    void Move()
    {
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
