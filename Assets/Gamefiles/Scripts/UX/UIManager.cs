using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using DesignPatterns.CreationalPatterns;
using Sirenix.OdinInspector;
using TMPro;
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
        Slack,
        DeathCRT,
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

    [Required] public GameObject go_speakAndDesc;
    [Required] public TextMeshProUGUI txt_speakAndDesc;
    
    
    public SerializedDictionary<FloorDisplays, UIDisplay> displays;

    protected override void Awake() => OnCompleteUITransition = CompleteUITransition;

    private void Start()
    {
        go_speakAndDesc.gameObject.SetActive(false);
        Display(currentDisplay = defaultDisplay);
    }
    
    public void Display(int enumindx) => Display((FloorDisplays)enumindx);

    public void PlayPhoneBtnSound()
    {
        AudioManager.UseSourceWithClip(AudioManager.SourceTrack.Player, AudioManager.AudioClipType.PhoneClick, 
            out var clip).PlayOneShot(clip);
    }
     
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

    public void SpeakOrCommentate(string text, float forTime, Action onFinish = null)
    {
        go_speakAndDesc.gameObject.SetActive(true);
        txt_speakAndDesc.text = text;
        CoroutineRunner.Instance.RunMethodDelayed(() =>
        {
            go_speakAndDesc.gameObject.SetActive(false);
            txt_speakAndDesc.text = text;
            onFinish?.Invoke();
        }, forTime);
    }


}
