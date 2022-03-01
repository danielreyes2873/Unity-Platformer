using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float runForce = 10f;
    public float runBonus = 3f;
    public float maxRunSpeed = 10f;
    public float jumpForce = 10f;
    public float jumpBonus = 6f;
    public bool feetOnGround;
    public bool hitBlock;
    private Rigidbody body;
    private Collider collider;
    private Animator animComp;
    public CameraScript cs;
    private float castDistance;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animComp = GetComponent<Animator>();
        cs = cs.GetComponent<CameraScript>();
    }

    // Update is called once per frame
    void Update()
    {
        castDistance = collider.bounds.extents.y + 0.1f;
        feetOnGround = Physics.Raycast(transform.position, Vector3.down, castDistance);
        
        float axis = Input.GetAxis("Horizontal");
        body.AddForce(Vector3.right * runForce * axis, ForceMode.Force);
        if (Mathf.Abs(body.velocity.x) > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            maxRunSpeed = 20f;
            body.AddForce(Vector3.right * runBonus, ForceMode.Force);         
        }
        else
        {
            maxRunSpeed = 10f;
        }

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
    
}