public enum Allegiance
{
    NONE,
    PLAYER,
    ENEMY
}

public static class AllegianceEnum
{
    public static bool CheckOpponent(this Allegiance allegiance, Allegiance other)
    {
        if (allegiance == Allegiance.NONE) return true;
        return allegiance != other;
    }
}