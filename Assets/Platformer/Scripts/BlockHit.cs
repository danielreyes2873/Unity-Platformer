using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockHit : MonoBehaviour
{
    // public Camera cam;
    // private CameraScript cs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log(collision.gameObject.name);
        if (collision.gameObject.GetComponent<CharacterController>() && collision.contacts[0].normal.y > 0.5f)
        {
            if (gameObject.name.Equals("Question(Clone)"))
            {
                CameraScript.AddCoins(gameObject);
            }
            else if (gameObject.name.Equals("Brick(Clone)"))
            {
                StartCoroutine(CameraScript.Break(gameObject));
            }
        }
    }
}
