using System;
using UnityEngine;

[Serializable]
public class SerializVector3
{
    public float x, y, z;
    public SerializVector3(Vector3 pos)
    {
        x = pos.x;
        y = pos.y;
        z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
