using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    public enum FloorDisplays
    {
        HUD,
        WriteName
    }
    
    public FloorDisplays defaultDisplay = FloorDisplays.HUD;

    [Serializable]
    public class UIDisplay
    {
        public List<GameObject> objects;
        public UnityEvent onDisplay;
    }
    
    public SerializedDictionary<FloorDisplays, UIDisplay> displays;

    private void Start() => Display(defaultDisplay);

    [Button]
    public void Display(FloorDisplays display)
    {
        foreach (var v in Enum.GetValues(typeof(FloorDisplays)))
        {
            if (v is not FloorDisplays vTyped) return;
            displays[vTyped].objects.ForEach(obj => obj.SetActive(false));
        }
        displays[display].objects.ForEach(obj => obj.SetActive(true));
        displays[display].onDisplay?.Invoke();
    }
    
    
}
