using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Platform : MonoBehaviour
{

    public Transform player;
    public float moveSpeed = 4.0f;
    public float reversePoint = 10.0f;
    private bool movingRight = true;


    private Vector3 originalPosition;

    private void Start()
    {
        originalPosition = transform.position;
    }


    void Update()
    {
        if (movingRight)
        {
            MovePlatform(Vector3.right);
            if (transform.position.x >= originalPosition.x + reversePoint)
            {
                movingRight = false;
            }
        }
        else
        {
            MovePlatform(Vector3.left);
            if (transform.position.x <= originalPosition.x)
            {
                movingRight = true;
            }
        }
    }


    void MovePlatform(Vector3 dir)
    {


        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (transform.childCount > 0)
        {
            Transform player = transform.GetChild(1);
            CharacterController controller = player.GetComponent<CharacterController>();

            if (controller != null)
            {
                controller.Move(dir * moveSpeed * Time.deltaTime);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player on");
            other.transform.SetParent(transform);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player off");
            other.transform.SetParent(null);

        }
    }


}
