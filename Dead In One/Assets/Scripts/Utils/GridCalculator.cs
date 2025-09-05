using UnityEngine;

public static class GridCalculator
{
    public static bool IsLineMovement(Vector2Int dir)
    {
        if (dir == Vector2Int.zero) return false;

        // Horizontal, vertical, or perfect diagonal
        return dir.x == 0 || dir.y == 0 || Mathf.Abs(dir.x) == Mathf.Abs(dir.y);
    }
}
