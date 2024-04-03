using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordGravity : MonoBehaviour
{
    IEnumerator Routine()
    {
        while (true)
        {
            Rigidbody[] Rigidbodies = FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
            for (int x = 0; x < Rigidbodies.Length; x++)
            {
                yield return new WaitForSeconds(0);

                for (int y = 0; y < Rigidbodies.Length; y++)
                {
                    if (x != y)
                    {
                        Rigidbodies[x].AddForce((Rigidbodies[y].gameObject.transform.position - Rigidbodies[x].transform.position) * (Rigidbodies[y].mass / Vector3.Distance(Rigidbodies[x].transform.position, Rigidbodies[y].transform.position)));
                        yield return new WaitForSeconds(0);
                    }
                }
            }
            yield return new WaitForSeconds(0);
        }
    }
    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(Routine());
    }
}
