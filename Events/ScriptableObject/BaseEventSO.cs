using System.Globalization;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventSO<T> : ScriptableObject
{
    [TextArea]
    public string description;    
    
    //最后一个启动事件的
    public string lastSender;

    public UnityAction<T> OnEventRaised;

    public void RaisEvent(T value, object sender)
    {
        OnEventRaised?.Invoke(value);
        lastSender = sender.ToString();
    }
}
