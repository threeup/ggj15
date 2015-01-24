using UnityEngine;
using System.Collections;

public static class Utilities
{
    public static Vector2 Vector3ToVector2(Vector3 vec)
    {
        return new Vector2(vec.x, vec.y);
    }

    public static Vector3 Vector2ToVector3(Vector2 vec)
    {
        return new Vector3(vec.x, vec.y, 0);
    }
}
