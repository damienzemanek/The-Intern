using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    public static Action OnCompleteUITransition;
    
    public enum FloorDisplays
    {
        HUD,
        WriteName,
        Phone,
        Slack
    }
    
    public bool uiTransitioning;
    public FloorDisplays defaultDisplay = FloorDisplays.HUD;
    public FloorDisplays currentDisplay = FloorDisplays.HUD;

    
    
    [Serializable]
    public class UIDisplay
    {
        public List<GameObject> objects;
        public UnityEvent onDisplay;
    }
    
    public SerializedDictionary<FloorDisplays, UIDisplay> displays;

    protected override void Awake() => OnCompleteUITransition = CompleteUITransition;

    private void Start() => Display(currentDisplay = defaultDisplay);
    
    public void Display(int enumindx) => Display((FloorDisplays)enumindx);
     
    [Button]
    public void Display(FloorDisplays display, bool autoCompleteTransition = true)
    {
        if (uiTransitioning) return;
        uiTransitioning = true;
        foreach (var v in Enum.GetValues(typeof(FloorDisplays)))
        {
            if (v is not FloorDisplays vTyped) { Debug.LogError($"Enum is not a FloorDisplays: {v}"); return; }
            displays[vTyped].objects.ForEach(obj => obj.SetActive(false));
        }
        displays[display].objects.ForEach(obj => obj.SetActive(true));
        displays[display].onDisplay?.Invoke();
        currentDisplay = display;
        if(autoCompleteTransition) OnCompleteUITransition.Invoke();
    }

    public void CompleteUITransition() => uiTransitioning = false;


}
