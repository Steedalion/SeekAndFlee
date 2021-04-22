using UnityEngine;

public sealed class World
{
    //TODO: Use singleton from toolbox.

    private World instance = new World();
    public static GameObject[] HidingSpots { get; }

    static World()
    {
        HidingSpots = GameObject.FindGameObjectsWithTag("hide");
    }

    public World Instance
    {
        get => instance;
    }
}
