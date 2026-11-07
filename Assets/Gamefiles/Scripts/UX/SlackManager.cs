using System;
using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

public class SlackManager : Singleton<SlackManager>
{
    [Required] public GameObject go_slackAppNotificationIcon;
    [Required] public RectTransform slackRect;
    [Required] public Sprite bossPic;
    [Required] public GameObject pr_slackMessage;
    [Required] public Transform parent_messageParent;

    private void OnEnable()
    {
        slackRect.RefreshLayoutGroupsImmediateAndRecursive();
        go_slackAppNotificationIcon.gameObject.SetActive(false);
    }

    public void SendSlackMsg(string msg)
    {
        var newMsg = Instantiate(pr_slackMessage, parent_messageParent);
        newMsg.Get<SlackMsg>().InitMessage(msg, bossPic);
        slackRect.RefreshLayoutGroupsImmediateAndRecursive();
        go_slackAppNotificationIcon.gameObject.SetActive(true);

    }
    
}