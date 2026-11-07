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
    
    [Serializable]
    public class Display<TDisplayStateEnum> where TDisplayStateEnum : Enum
    {
        public TDisplayStateEnum currentUiScreenState;
        [ReadOnly] [DrawWithUnity] public SerializedDictionary<TDisplayStateEnum, DisplayData> explorationUIs = new();
        [ReadOnly] public List<GameObject> allUIs;
    
        [Button]
        public void AddUIState(TDisplayStateEnum state, DisplayData data)
        {
            explorationUIs.Add(state, data);
            allUIs.AddRange(data.uiObjects);
        }

        [Button]
        public void RemoveUIState(TDisplayStateEnum state)
        {
            foreach (var uiObject in explorationUIs[state].uiObjects)
                allUIs.Remove(uiObject);
            explorationUIs.Remove(state);
        }
    }
    
    [Serializable]
    public class DisplayData
    {
        public List<GameObject> uiObjects;
        public UnityEvent onDisplayed;
    }
    
    [Required] public Animator alertAnimator;
    [Required] public TextMeshProUGUI alertText;
    [Required] public GameObject playerGO;

    public SerializedDictionary<TDisplaysEnum, object> DisplaysDict;
    

    void Awake()
    {
        alertAnimator.gameObject.SetActive(false);
    }
    
    public void HideAllUIs() => allUIs.ForEach(x => x.SetActive(false));
    
    public void ShowUIScreen<TScreenState>(TDisplaysEnum display, TScreenState state) where TScreenState : Enum
    {
        var uiScreenNotTyped = DisplaysDict[display];
        if(uiScreenNotTyped is not Display<TScreenState> screenState)
        
        
        currentUIState = UIState.Exploration;
        currentExplorationUIState = explorationUIState;
        HideAllUIs();
        explorationUIs[explorationUIState].uiObjects.ForEach(g => g.SetActive(true));
        explorationUIs[explorationUIState].onDisplay?.Invoke();
        onUseExplorationUI?.Invoke(playerGO);
    }
    
    public void ShowMainUIState(int state) => ShowMainUIState((MainUIState)state);
    public void ShowMainUIState(MainUIState mainUIState)
    {
        currentUIState = UIState.Main;
        currentMainUIState = mainUIState;
        HideAllUIs();
        mainUIs[mainUIState].uiObjects.ForEach(g =>
        {
            g.SetActive(true);
            Debug.Log("Showing Main UI State on Obj " + g.name);
        });
        mainUIs[mainUIState].onDisplay?.Invoke();
        onUseMainUI?.Invoke(playerGO);
        Debug.Log("Showed Main UI State");
    }
    
    public void ShowCharacterUIState(int state) => ShowCharacterUIState((CharacterUIState)state);
    public void ShowCharacterUIState(CharacterUIState characterUIState)
    {
        currentUIState = UIState.Character;
        currentCharacterUIState = characterUIState;
        HideAllUIs();
        characterUIs[characterUIState].uiObjects.ForEach(g => g.SetActive(true));
        characterUIs[characterUIState].onDisplay?.Invoke();
    }

    public void Alert(string text, Action cb = null)
    {
        alertAnimator.gameObject.SetActive(true);
        alertText.text = text;
        alertAnimator.PlayOnEnd("AlertStart", () =>
        {
            alertText.text = string.Empty;
            alertAnimator.gameObject.SetActive(false);
            cb?.Invoke();
        });
    }

    public void Back()
    {
        Debug.Log("Back");
        if (currentExplorationUIState == ExplorationUIState.FirstStart ||
            currentExplorationUIState == ExplorationUIState.None) return;
        switch (currentUIState)
        {
            case UIState.Main: ShowExplorationUIState(ExplorationUIState.Exploration); break;
            case UIState.Character: ShowMainUIState(MainUIState.Characters); break;
            case UIState.Exploration: ShowMainUIState(MainUIState.Pause); Debug.Log("Pausing! "); break;
            default: Debug.Log("Did not move to a different UI State"); break;
        }
    }
}
