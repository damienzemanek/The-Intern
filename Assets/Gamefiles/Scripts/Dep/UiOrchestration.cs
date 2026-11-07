using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UiOrchestration<TDisplaysEnum> : MonoBehaviour
{
    public string currentDisplayName;
    public string currentDisplayStateName;
    
    [Serializable]
    public class Display<TDisplayStateEnum> : UIContainer
        where TDisplayStateEnum : Enum
    {
        public TDisplayStateEnum currentUiScreenState;
        [ReadOnly] [DrawWithUnity] public SerializedDictionary<TDisplayStateEnum, DisplayData> stateData = new();
        [ReadOnly] public List<GameObject> allUIs;
    
        public override void HideAllUIs() => allUIs.ForEach(x => x.SetActive(false));
            
        [Button]
        public void AddUIState(TDisplayStateEnum state, DisplayData data)
        {
            if (stateData.ContainsKey(state)) {
                Debug.LogError($"{state} is already in the state data");
                return; }
            stateData.Add(state, data);
            allUIs.AddRange(data.uiObjects);
        }

        [Button]
        public void RemoveUIState(TDisplayStateEnum state)
        {
            foreach (var uiObject in stateData[state].uiObjects)
                allUIs.Remove(uiObject);
            stateData.Remove(state);
        }
    }
    
    [Serializable]
    public class DisplayData
    {
        public List<GameObject> uiObjects;
        public UnityEvent onDisplayed;
    }
    
    [Required] public GameObject playerGO;

    public SerializedDictionary<TDisplaysEnum, object> DisplaysDict;
    public List<UIContainer> allUIs;

    [Button]
    public void AddDisplay<TDisplayStateEnum>(TDisplaysEnum displayEnum, TDisplayStateEnum stateEnum)
        where TDisplayStateEnum : Enum
    {
        if (DisplaysDict.ContainsKey(displayEnum)) {
            Debug.LogError($"{stateEnum} is already in the state data");
            return; }
        DisplaysDict.TryAdd(displayEnum, new Display<TDisplayStateEnum>());
    }
    
    [Button]
    public void RemoveDisplay(TDisplaysEnum displayEnum)
    {
        if (!DisplaysDict.ContainsKey(displayEnum)) {
            Debug.LogError($"{displayEnum} is not in the state data");
            return; }
        DisplaysDict.Remove(displayEnum);
    }

    public void HideAllUIs() => allUIs.ForEach(u => u.HideAllUIs());
    
    public void ShowUIScreen<TScreenState>(TDisplaysEnum displayEnum, TScreenState stateEnum) where TScreenState : Enum
    {
        var uiScreenNotTyped = DisplaysDict[displayEnum];
        if (uiScreenNotTyped is not Display<TScreenState> displayState)
        {
            Debug.LogError($"{stateEnum} is not the correct state enum for display {displayEnum}");
            return;
        }
        
        HideAllUIs();
        displayState.currentUiScreenState = stateEnum;
        displayState.stateData[stateEnum].uiObjects.ForEach(go => go.SetActive(true));
        displayState.stateData[stateEnum].onDisplayed?.Invoke();
        currentDisplayStateName = displayEnum.ToString();
        currentDisplayName = displayEnum.ToString();
    }
    

}
