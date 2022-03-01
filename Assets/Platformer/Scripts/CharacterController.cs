using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runForce = 10f;
    public float maxRunSpeed = 6f;
    public float jumpForce = 10f;
    public float jumpBonus = 6f;
    public bool feetOnGround;
    public bool hitBlock;
    private Rigidbody body;
    private Collider collider;
    private Animator animComp;
    public GameObject cam;
    public CameraScript cs;
    private float castDistance;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animComp = GetComponent<Animator>();
        cs = cs.GetComponent<CameraScript>();
        StartCoroutine(BlockHitRaycast());
    }

    // Update is called once per frame
    void Update()
    {
        castDistance = collider.bounds.extents.y + 0.1f;
        feetOnGround = Physics.Raycast(transform.position, Vector3.down, castDistance);
        // hitBlock = Physics.Raycast(transform.position, Vector3.up);
        //
        // if (hitBlock)
        // {
        //     Debug.Break();
        // }
        
        float axis = Input.GetAxis("Horizontal");
        body.AddForce(Vector3.right * runForce * axis, ForceMode.Force);

        var rotationVector = transform.rotation.eulerAngles;
        
        if (axis > 0.1)
        {
            rotationVector.y = 90f;
        } else if (axis < -0.1)
        {
            rotationVector.y = -90f;
        }

        transform.rotation = Quaternion.Euler(rotationVector);

        if (feetOnGround && Input.GetKeyDown(KeyCode.Space))
        {
            body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        else if (body.velocity.y > 0f && Input.GetKey(KeyCode.Space))
        {
            body.AddForce(Vector3.up * jumpBonus, ForceMode.Force);
        }

        if (Mathf.Abs(body.velocity.x) > maxRunSpeed)
        {
            float newX = maxRunSpeed * Mathf.Sign(body.velocity.x);
            body.velocity = new Vector3(newX, body.velocity.y, body.velocity.z);
        }

        if (Mathf.Abs(axis) < 0.1f)
        {
            float newX = body.velocity.x * (1f - Time.deltaTime * 5f);
            body.velocity = new Vector3(newX, body.velocity.y, body.velocity.z);
        }
        
        animComp.SetFloat("Speed", body.velocity.magnitude);
        animComp.SetBool("Jumping", !feetOnGround);
        
        // hitBlock = Physics.Raycast(transform.position, Vector3.up, castDistance);
        //
        // if (hitBlock)
        // {
        //     if 
        // }

        //Ray ray = new Ray(transform.position * castDistance, Vector3.up);
        // if (Physics.Raycast(ray, out RaycastHit hitInfo))
        // {
        //     if (hitInfo.collider.gameObject.name.Equals("Question(Clone)"))
        //     {
        //         cs.addCoins(hitInfo.collider.gameObject);
        //     }
        // }

        
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(other.gameObject.name);
        if (other.gameObject.name.Equals("Water(Clone)"))
        {
            cs.GameOverEvent();
        }
        else if (other.gameObject.name.Equals("Flagpole(Clone)") || other.gameObject.name.Equals("Flag(Clone)"))
        {
            cs.GameWinEvent();
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.name.Equals("Question(Clone)"))
    //     {
    //         if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitInfo, 1.82f))
    //         {
    //             Debug.DrawRay(transform.position, Vector3.up * castDistance, Color.green);
    //             // Debug.Break();
    //             Debug.Log(hitInfo.collider.gameObject.name);
    //             if (hitInfo.collider.gameObject.name.Equals("Question(Clone)"))
    //             {
    //                 cs.addCoins(hitInfo.collider.gameObject);
    //             }
    //         }
    //     }
    // }

    IEnumerator BlockHitRaycast()
    {
        while (true)
        {
            if (Physics.Raycast(transform.position, Vector3.up, out RaycastHit hitInfo, 1.7435f))
            {
                Debug.DrawRay(transform.position, Vector3.up, Color.green);
                // Debug.Break();
                Debug.Log(hitInfo.collider.gameObject.name);
                if (hitInfo.collider.gameObject.name.Equals("Question(Clone)"))
                {
                    cs.addCoins(hitInfo.collider.gameObject);
                }
                
            }

            yield return null;
        }
    }
}

// Use a coroutine similar to updatePickingraycast do not do raycast in update, you fucking donut