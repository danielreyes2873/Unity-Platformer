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
    private Rigidbody body;
    private Collider collider;
    private Animator animComp;
    
    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        animComp = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float castDistance = collider.bounds.extents.y + 0.1f;
        feetOnGround = Physics.Raycast(transform.position, Vector3.down, castDistance);
        
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
    }
}
