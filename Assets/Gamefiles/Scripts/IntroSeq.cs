using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class IntroSeq : MonoBehaviour
{
    public GameObject go_preIntroUI;
    public Animator IntroSeqAnimator;
    public string startIntroSeqName = "IntroStart";
    public string endIntroSeqName = "IntroEnd";
    public Transform t_documentSpawnPt;
    public float tweenTime = 0.3f;
    
    public Transform tweenFrontPos;
    public Transform tweenOffScreenPos;
    public List<GameObject> pr_DocumentsToSign = new();
    public GameObject pr_lastDoc;
    public int numOfDocs = 100;
    
    public bool signedDoc;

    public List<GameObject> allDocs = new();

    public void StartIntroSeq()
    {
        go_preIntroUI.SetActive(false);
        IntroSeqAnimator.PlayOnEnd(startIntroSeqName, () => StartSigning());   
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)) Sign();
    }

    public void StartSigning() => StartCoroutine(C_StartSigning());

    [Button]
    public void Sign() => signedDoc = true;

    public IEnumerator C_StartSigning()
    {
         for(int i = 0; i <  numOfDocs; i++)
         {
             var newDoc = Instantiate(pr_DocumentsToSign.Rand(),  t_documentSpawnPt.position, Quaternion.identity);
             allDocs.Add(newDoc);
             newDoc.transform.SetParent(null);
             newDoc.transform.DOMove(tweenFrontPos.position, tweenTime);
             yield return new WaitUntil(() => signedDoc);
             signedDoc = false;
             newDoc.transform.DOMove(tweenOffScreenPos.position, tweenTime);
         }
         var lastDoc = Instantiate(pr_lastDoc, tweenFrontPos.position, Quaternion.identity);
         allDocs.Add(lastDoc);
         lastDoc.transform.SetParent(null);
         lastDoc.transform.DOMove(tweenOffScreenPos.position, tweenTime);
         yield return new WaitUntil(() => signedDoc);
         signedDoc = false;
         lastDoc.transform.DOMove(tweenOffScreenPos.position, tweenTime);
         yield return new WaitForSeconds(1f);
         allDocs.ForEach(g => g.SetActive(false));
         
    }



}
