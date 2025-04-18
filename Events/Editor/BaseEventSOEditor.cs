using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.TerrainTools;
using UnityEngine;

[CustomEditor(typeof(BaseEventSO<>))]
public class BaseEventSOEditor<T> : Editor
{
    private BaseEventSO<T> baseEventSO;

    private void OnEnable()
    {
        if (baseEventSO == null)
        {
            baseEventSO=target as BaseEventSO<T>;
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.LabelField("订阅数量" + GetListeners().Count);

        foreach (var listener in GetListeners())
        {
            EditorGUILayout.LabelField(listener.ToString()); //显示监听器的名称
        }
    }

    /// <summary>
    /// 得到所有订阅者
    /// </summary>
    /// <returns></returns>
    private List<MonoBehaviour> GetListeners()
    {
        List<MonoBehaviour> listeners = new List<MonoBehaviour>();
        var subscribers = baseEventSO.OnEventRaised.GetInvocationList();

        if (baseEventSO == null || baseEventSO.OnEventRaised == null)
        {
            return listeners;
        }
        
        foreach ( var subscriber in subscribers )
        {
            var obj=subscriber.Target as MonoBehaviour;
            if (!listeners.Contains(obj))
            {
                listeners.Add(obj);
            }
        }
        return listeners;
    }
}
