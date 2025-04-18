using System;
/// <summary>
///所有的房间类型
/// </summary>
[Flags]
public enum RoomType
{
    MinorEnemy = 1,
    EliteEnemy = 2,
    Shop = 4,
    Treasure = 8,
    RestRoom = 16,
    Boss = 32        
}

/// <summary>
/// 房间可访问状态
/// </summary>
public enum RoomState
{
    Locked,    //上锁
    Visited,   //已访问过
    Attainable //可访问
}

/// <summary>
/// 卡片类型
/// </summary>
public enum CardType
{
    Attack,    //攻击
    Defense,   //防御
    Abilities  //能力
}

/// <summary>
/// 卡片目标生效范围
/// </summary>
public enum EffectTargetType
{
    Self,
    Target,
    ALL
}

