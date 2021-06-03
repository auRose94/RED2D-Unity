using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class REDCharacterController : MonoBehaviour
{
    public float gravity = 9.8f;
    public float walkingSpeed = 6.0f;
    public float torqueSpeed = 0.5f;
    public float rotSpeed = 0.5f;
    public float standingRayLength = 1.0f;
    public Transform rayStart;
    Collider capsule;
    private Vector3 down;
    private Vector3 point;
    private bool touching;
    private Rigidbody body;

    void Start()
    {
        capsule = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        down = -transform.up;
    }
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
            return; // No gamepad connected.
        var xInput = gamepad.leftStick.x.ReadValue();
        var mask = LayerMask.GetMask("World");
        RaycastHit hitInfo;
        var standing = Physics.Raycast(rayStart.position, -transform.up, standingRayLength, mask);
        /*
        if (standing)
            body.constraints |= RigidbodyConstraints.FreezeRotationZ;
        else
            body.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
        */
        if (touching || standing)
        {
            //Quaternion rot = Quaternion.FromToRotation(transform.up, hitInfo.normal);
            Quaternion rot = Quaternion.FromToRotation(-transform.up, down);
            //Quaternion rot = Quaternion.FromToRotation(Vector3.up, transform.InverseTransformDirection(-down));
            body.AddTorque(new Vector3(rot.x, rot.y, rot.z) * torqueSpeed, ForceMode.Force);
            body.AddTorque(0, 0, xInput * rotSpeed, ForceMode.Impulse);

            //body.MoveRotation(rot);
            body.AddRelativeForce(Vector3.right * walkingSpeed * xInput, ForceMode.VelocityChange);
            Debug.DrawRay(point, down * gravity, Color.red);
        }
        else
        {
            Debug.DrawRay(transform.position, down * gravity, Color.white);
        }
        body.AddForceAtPosition(-transform.up * gravity, point);
    }
    void OnCollisionStay(Collision collision)
    {
        Vector3 avg = Vector3.zero;
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.thisCollider == capsule)
            {
                //down = -contact.normal;
                point = contact.point;
                touching = true;
                avg += contact.normal;
            }
        }
        down = -(avg / collision.contacts.Length);
    }
    void OnCollisionExit(Collision collision)
    {
        touching = false;
    }
}
