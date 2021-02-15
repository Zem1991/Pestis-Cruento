public static class Helper
{
    public static bool IsAngleWithinArc(float angle, float arcStart, float arcEnd)
    {
        float startToEnd = arcEnd - arcStart;
        float startToAngle = angle - arcStart;
        arcEnd = startToEnd < 0 ? startToEnd + 360 : startToEnd;
        angle = startToAngle < 0 ? startToAngle + 360 : startToAngle;
        return angle <= arcEnd;
    }
}
