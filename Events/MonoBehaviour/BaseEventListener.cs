using System;
using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO; //获得事件
    public UnityEvent<T> response; //要启动的事件 在Inspeactor窗口中配置

    private void OnEnable()
    {
        if (eventSO != null)
        {
            eventSO.OnEventRaised += OnEventRaised;                
        }
    }


    private void OnDisable()
    {
        if (eventSO != null) 
        {
            eventSO.OnEventRaised -= OnEventRaised;                
        }
    }

    private void OnEventRaised(T value)
    {
        response?.Invoke(value);
    }
}
