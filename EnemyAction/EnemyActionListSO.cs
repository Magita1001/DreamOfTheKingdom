using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyActionListSO", menuName = "Enemy/EnemyActionListSO")] 
public class EnemyActionListSO : ScriptableObject
{
    public List<EnemyAction> actions;
}

[System.Serializable]
public struct EnemyAction
{
    public Sprite intentSprite;
    public Effect effect;
}
