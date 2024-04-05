using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{


    public static event Action OnCollected;

    public static int total;


    void Awake()
    {
        total++;
    }



    void Update()
    {
        transform.Rotate(0, 0.5f, 0.5f);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected?.Invoke();
            Destroy(gameObject);
        }
    }


}
