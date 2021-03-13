using UnityEngine;

public static class Helper
{
    public static float RoundToMultiple(float value, float multipleOf)
    {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    public static Vector3 RoundToMultiple(Vector3 value, float multipleOf)
    {
        Vector3 result = new Vector3
        {
            x = RoundToMultiple(value.x, multipleOf),
            y = RoundToMultiple(value.y, multipleOf),
            z = RoundToMultiple(value.z, multipleOf)
        };
        return result;
    }
}
