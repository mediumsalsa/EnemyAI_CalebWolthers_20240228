using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeGenerator : MonoBehaviour
{

    public static GameObject PreFab;

    static GameObject newObject;

    [SerializeField]private static Vector3 spawnLocation = new Vector3(-104.5f, 7.07f, -111.85f);

    public static void SpawnCube()
    {
        newObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
        newObject.transform.position = spawnLocation;
        newObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        newObject.AddComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Killbox")
        {
            Destroy(newObject);
        }
    }

}
