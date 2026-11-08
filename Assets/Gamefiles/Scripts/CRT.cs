using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CRT : MonoBehaviour
{
    public int currentPostItDeath = 0;
    public List<GameObject> postItDeaths = new();
    
    [Required] public GameObject go_EmailNotifAlert;
    [Required] public GameObject go_homeScreen;
    [Required] public GameObject go_emailListScreen;
    [Required] public GameObject go_inspectEmailScreen;
    [Required] public GameObject go_msgSent;
    [Required] public TextMeshProUGUI txt_deathReason;
    public List<GameObject> allScreens = new();
 
    private void Awake()
    {
        allScreens.Add(go_homeScreen);
        allScreens.Add(go_emailListScreen);
        allScreens.Add(go_inspectEmailScreen);
        allScreens.Add(go_msgSent);
        allScreens.Add(go_EmailNotifAlert);
    }

    private void OnEnable()
    {
        foreach (var t in postItDeaths) t.SetActive(false);

        currentPostItDeath++;
        if (currentPostItDeath < postItDeaths.Count)
        {
            for(int i = 0; i < currentPostItDeath; i++)
                postItDeaths[i].SetActive(true);
        }
        else foreach (var t in postItDeaths)
                t.SetActive(true);
        
        allScreens.ForEach(s => s.SetActive(false));
        go_homeScreen.gameObject.SetActive(true);
        CoroutineRunner.Instance.RunMethodDelayed(() => go_EmailNotifAlert.SetActive(false), 2f);
    }

    public void OpenEmailAppFromHome()
    {
        allScreens.ForEach(s => s.SetActive(false));
        go_emailListScreen.SetActive(true);
    }

    public void SelectSpecificEmail()
    {
        allScreens.ForEach(s => s.SetActive(false));
        go_inspectEmailScreen.SetActive(true);
        txt_deathReason.text = FloorManager.Instance.internData[FloorManager.Instance.currentIntern].deathReason;
    }
    
    [Button]
    public void MsgSent()
    {
        go_msgSent.SetActive(true);
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            allScreens.ForEach(s => s.SetActive(false));
            FloorManager.Instance.ResetDeath();
        }, 3f);
        
    }
}
