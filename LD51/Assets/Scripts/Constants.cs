using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants 
{
    public const int ATTACK_TYPE_MELEE = 0;
    public const int ATTACK_TYPE_PROJECTILE = 1;
    public const int ATTACK_TYPE_MAGIC = 2; 
    public const int MAX_ATTACKS = 2;

    public const int CONDITION_NORMAL = 0;
    public const int CONDITION_PLUS_MELEE = 1;
    public const int CONDITION_PLUS_RANGED = 2;
    public const int CONDITION_PLUS_MAGIC = 3;
    public const int CONDITION_MINUS_MELEE = 4;
    public const int CONDITION_MINUS_RANGED = 5;
    public const int CONDITION_MINUS_MAGIC = 6;
    public const int CONDITION_SPAWN_ENEMIES = 7;
    public const int CONDITION_REVERSE_CONTROLS = 8;
    public const int CONDITION_NO_SOUND = 9;
    public const int CONDITION_DARK = 10;
    public const int CONDITION_ATTACK_LOCKED = 11;
    public const int CONDITION_MUSIC_CHANGE = 12;
}
