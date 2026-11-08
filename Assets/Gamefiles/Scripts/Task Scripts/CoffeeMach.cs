using System;
using System.Collections;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CoffeeMach : MonoBehaviour
{
    public bool running = false;
    public bool hasDrowned = false;
    [Required] public GameObject go_coffeMugObj;
    [FormerlySerializedAs("go_coffeeMug")] [Required] public Animator coffeeMachAnimator;
    public string coffeeMugStartFillingAnimName = "Filling";
    public string coffeeMugStopFillingAnimName = "StopFilling";
    public string coffeeMugFloodingAnimName = "Flooding";
    public string coffeeMugDefaultAnimName = "Default";
    public float drownScreenTime = 5f;
    public float drowningTime = 10f;

    [Required] public Door door;
    [Required] public GameObject go_GrabFilledMug;
    [Required] public GameObject drownScreen;
    


    private void Awake()
    {
        go_coffeMugObj.gameObject.SetActive(false);
        go_GrabFilledMug.SetActive(false);
    }

    public void PlaceCoffeeMug()
    {
        go_coffeMugObj.gameObject.SetActive(true);
    }

    public void StartCofeeMaker()
    {
        if (running) return;
        running = true;
        if (!hasDrowned) StartCoroutine(C_DrownEvent());
    }

    public IEnumerator C_DrownEvent()
    {
        door.locked = true;
        bool filled = false;
        coffeeMachAnimator.PlayOnEnd(coffeeMugStartFillingAnimName, () => filled = true);
        yield return new WaitUntil(() => filled);
        filled = false;
        yield return new WaitForSeconds(2f);
        coffeeMachAnimator.CrossFadeOnEnd(coffeeMugFloodingAnimName, 0.5f, () => filled = true);
        yield return new WaitForSeconds(drownScreenTime);
        drownScreen.SetActive(true);
        yield return new WaitForSeconds(drowningTime);
        FloorManager.Instance.playerController.Die();
        drownScreen.SetActive(false);
        go_GrabFilledMug.SetActive(true);
        coffeeMachAnimator.Play(coffeeMugDefaultAnimName);
        yield return new WaitForSeconds(1f);
        running = false;
        hasDrowned = true;
        door.locked = false;
    }
}
