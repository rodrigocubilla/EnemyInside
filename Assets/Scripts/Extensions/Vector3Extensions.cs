using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 adjusted(this Vector3 value)
    {
        return new Vector3(value.x, 0f, value.y);
    }

    public static Vector3Int adjusted(this Vector3Int value)
    {
        return new Vector3Int(value.x, 0, value.y);
    }

    public static Vector2Int deajust(this Vector3 value)
    {
        return new Vector2Int((int)value.x, (int)value.z);
    }

    public static Vector2Int deajust(this Vector3Int value)
    {
        return new Vector2Int(value.x, value.z);
    }

    public static Vector3Int ToInt(this Vector3 value)
    {
        return new Vector3Int(Mathf.RoundToInt(value.x), 0, Mathf.RoundToInt(value.z));
    }
}