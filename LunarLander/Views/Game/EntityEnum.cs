using System;

[Flags]
public enum EntityEnum 
{
    Lander,
    Terrian
}

[Flags]
public enum GameStateEnum 
{
    Waiting,
    Gameplay,
    Won,
    Lost
}
