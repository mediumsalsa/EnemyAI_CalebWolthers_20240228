using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public Vector3 targetPosition;

    public float slidingSpeed = 5f;

    public int maxHeight = 10;

    private Vector3 originalPosition;

    public static bool IsPlayerInside = false;



    void Start()
    {
        originalPosition = transform.position;

        targetPosition = new Vector3(originalPosition.x, maxHeight, originalPosition.z);
    }


    void Update()
    {
        if (IsPlayerInside)
        {
            SlideDoor(targetPosition);
        }
        else
        {
            SlideDoor(originalPosition);
        }
    }


    private void SlideDoor(Vector3 target)
    {
        transform.position = Vector3.Lerp(transform.position, target, slidingSpeed * Time.deltaTime);
    }


}
