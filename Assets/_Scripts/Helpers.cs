using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static bool isWIthin(this int value, int min, int max)
    {
        return value >= min && value < max;
    }
}