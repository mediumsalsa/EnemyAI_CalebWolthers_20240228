using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCount : MonoBehaviour
{

    public TMPro.TMP_Text text;


    int count;

    void Awake()
    {
        text = GetComponent<TMPro.TMP_Text>();
    }


    void Start()
    {
        UpdateCount();
    }


    void OnEnable()
    {
        Collectable.OnCollected += OnCollectibleCollected;
    }
    void OnDisable()
    {
        Collectable.OnCollected -= OnCollectibleCollected;
    }

    void OnCollectibleCollected()
    {
        count++;
        UpdateCount();
    }

    void UpdateCount()
    {
        text.text = $"{count} / {Collectable.total}";
    }



}
