using System;
using DesignPatterns.CreationalPatterns;
using EMILtools.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using static AudioManager;

public class SlackManager : Singleton<SlackManager>
{
    [Required] public RectTransform slackRect;
    [Required] public Sprite bossPic;
    [Required] public GameObject pr_slackMessage;
    [Required] public Transform parent_messageParent;
    [Required] public Phone phone;

    public string firstDeathSlackMsg =
        "Hey new Intern! So we had some trouble with our last intern... something about a printer. So you're going to take his place!" +
        "I need you to finish what they started. Thanks!";
    
    public string coffeeDeathSlackMsg =
        "Hey new Intern! Looks like one of our other interns had a little trouble with the espresso machine..." +
        "Clarence still wants his coffee though! Get to it.";
    
    
    public void OpenSlack()
    {
        slackRect.RefreshLayoutGroupsImmediateAndRecursive();
        phone.RemoveNotification();
    }

    public void SendSlackMsg(string msg)
    {
        var newMsg = Instantiate(pr_slackMessage, parent_messageParent);
        newMsg.Get<SlackMsg>().InitMessage(msg, bossPic);
        slackRect.RefreshLayoutGroupsImmediateAndRecursive();
        phone.NewNotification();
    }
    
}