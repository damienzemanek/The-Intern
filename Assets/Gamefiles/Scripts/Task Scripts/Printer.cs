using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Printer : MonoBehaviour
{
    [Required] public Transform playerLockedLocation;
    public string state;

    [Serializable]
    public struct PrinterEvent
    {
        public float timeUntil;
        public int errorsNeededToClear;
    }

    public bool clearedErrorTrigger;
    public bool blowedUp;

    public int currSelfDesCntdown;
    public int errorTriggersCleared = 0;
    public PrinterEvent firstError;
    public PrinterEvent secondError;
    [FormerlySerializedAs("selfDestruct")] public PrinterEvent selfDestructStart;
    public int selfDestructTime = 15;    
    public float waitToClearRUSure = 2.5f;    

    
    [Required]  public GameObject go_printingDisp;
    [Required]  public GameObject go_errorDisp;
    [Required]  public GameObject go_selfDestructDisp;
    [Required]  public GameObject go_rusure;
    [Required]  public GameObject selfDestResetBtn;

    [Required]  public GameObject go_startPrintBtn;
    [Required] public TextMeshPro txt_selfDestruct;
    
    public List<GameObject> disps = new();

    private void Awake()
    {
        disps.Add(go_printingDisp);
        disps.Add(go_errorDisp);
        disps.Add(go_selfDestructDisp);
        disps.Add(go_rusure);
        go_startPrintBtn.gameObject.SetActive(false);
        foreach (var disp in disps) disp.SetActive(false);
        selfDestResetBtn.gameObject.SetActive(false);
    }

    public void StartPrinting()
    {
        go_startPrintBtn.gameObject.SetActive(false);
        StartCoroutine(C_Print());
        FloorManager.Instance.playerController.moveLocked = true;
        FloorManager.Instance.playerController.transform.DOMove(playerLockedLocation.position, 1f);
    }

    public IEnumerator C_Print(float time)
    {
        disps.ForEach(d => d.SetActive(false));
        go_printingDisp.SetActive(true);
        state = "printing";
        yield return new WaitForSeconds(time);
    }

    public IEnumerator C_Error()
    {
        disps.ForEach(d => d.SetActive(false));
        go_errorDisp.SetActive(true);
        state = "error";
        yield return new WaitUntil(() => clearedErrorTrigger);
        clearedErrorTrigger = false;
    }

    public void StartSelfDestruct()
    {
        disps.ForEach(d => d.SetActive(false));
        go_selfDestructDisp.SetActive(true);
        state = "selfDestruct";
    }

    public IEnumerator C_SelfDestructCountdown()
    {
         int secondsPassed = 0;
         currSelfDesCntdown = selfDestructTime;

        while (currSelfDesCntdown > 0)
        {
            txt_selfDestruct.text = $"SELF DESTRUCTING IN... {currSelfDesCntdown}";
            yield return new WaitForSeconds(1);
            currSelfDesCntdown--;
            secondsPassed++;

            if (secondsPassed == 2) selfDestResetBtn.gameObject.SetActive(true);
        }
    }

    IEnumerator C_Print()
    {
        yield return C_Print(firstError.timeUntil);
        yield return C_Error();
        
        yield return C_Print(secondError.timeUntil);
        yield return C_Error();
        
        yield return C_Print(selfDestructStart.timeUntil);
        StartSelfDestruct();
        yield return C_SelfDestructCountdown();


    }

    public void AttemptReset()
    {
        clearedErrorTrigger = true;
    }

    private bool rUSuring = false;
    public void AttemptResetSelfDestruct()
    {
        go_rusure.gameObject.SetActive(true); 
        selfDestResetBtn.gameObject.SetActive(false);
    }

    public void AttemptClearRUSure()
    {
        if (rUSuring) return;
        rUSuring = true;
        go_rusure.SetActive(false);
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            rUSuring = false;
            selfDestResetBtn.gameObject.SetActive(true);
        }, waitToClearRUSure);
    }
    
    
    
    
}
