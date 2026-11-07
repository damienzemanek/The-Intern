using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    public enum FloorDisplays
    {
        HUD,
        WriteName
    }

    [Serializable]
    public class UIDisplay
    {
        public List<GameObject> objects;
        public UnityEvent onDisplay;
    }
    
    public SerializedDictionary<FloorDisplays, UIDisplay> displays;
    
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
