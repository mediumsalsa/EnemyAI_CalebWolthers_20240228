using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public static Dictionary<Teleporter, Transform> teleporterDestinations = new Dictionary<Teleporter, Transform>();

    void Start()
    {
        if (nextLocation != null)
        {
            teleporterDestinations[this] = nextLocation;
        }
    }

    public Transform nextLocation;
}