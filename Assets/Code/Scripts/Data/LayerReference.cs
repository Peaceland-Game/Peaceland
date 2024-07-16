using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as a static reference for Unity layers in code instead of having to remember the int layer number
/// </summary>
public class LayerReference : MonoBehaviour
{
    public static int INTERACTABLE = 3;
    public static int PLAYER = 15;
    public static int GROUND = 6;
    public static int OBSTACLES = 7;
    public static int ROADS = 14;
    public static int PLAYABLE_AREA_CONTROLLER = 15;
    public static int CELL = 19;
    public static int NPC = 18;
    public static int TRIGGER_IGNORE_RAYCAST = 17;
    public static int PLAYABLE_AREA = 16;
}
