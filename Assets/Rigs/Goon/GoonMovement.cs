using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{
    // Goon Movement Mode / State
    enum Mode {
        Idle,
        Walk,
        InAir
    }

    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float footSeparateAmount = 0.2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .4f;
    public float walkFootSpeed = 4; // Multiplier

    private CharacterController pawn;

    private Mode mode = Mode.Idle; // State = IDLE_STATE
    private Vector3 input;

    private float walkTime;

    private Camera cam;

    private Quaternion targetRotation;
    /// <summary> Current vertical velocity in m/s </summary>
    public float velocityY = 0;

    public float gravity = 50;
    public float jumpImpulse = 15;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
        cam = Camera.main;
    }


    void Update()
    {


        // Calculate Player Input
        float v = Input.GetAxis("Vertical");    
        float h = Input.GetAxis("Horizontal");

        Vector3 camForward = cam.transform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = Vector3.Cross(Vector3.up, camForward);



        // Craete and normalize move vector
        input = camForward * v + camRight * h;
        if (input.sqrMagnitude > 1) input.Normalize();



        // Control current Mode
        // This section is a bit confusing
        float threshold = .1f;
        mode = (input.sqrMagnitude > threshold * threshold) ? Mode.Walk : Mode.Idle;  // Ternary Operator: Variable = (boolean condition) ? if (test == true) : if (test == false)

        if(mode == Mode.Walk) targetRotation = Quaternion.LookRotation(input, Vector3.up);

        if(pawn.isGrounded) {
            if (Input.GetButtonDown("Jump")) {
                velocityY = -jumpImpulse;
            }
        }

        velocityY += gravity * Time.deltaTime;

        // MOVE using calculated movement Vector and player speed
        pawn.Move((input * speed + Vector3.down * velocityY) * Time.deltaTime);

        if (pawn.isGrounded)
        {
            velocityY = 0;
        } else {
            mode = Mode.InAir;
        }

        Animate();
    }

    /// <summary> Controls which animation plays </summary>
    void Animate() {

        transform.rotation = AnimMath.Ease(transform.rotation, targetRotation, .01f);

        switch(mode) {
            case Mode.Idle:
                AnimateIdle();
                break;
            case Mode.Walk:
                AnimateWalk();
                break;
            case Mode.InAir:
                AnimateInAir();
                break;

        }
    }

    private void AnimateInAir()
    {
        // TODO Create an animation for air
    }

    /// <summary>
    /// Animate Goon's feet, not necessarily everything
    /// </summary>
    void AnimateIdle() {
        footLeft.SetPositionHome();
        footRight.SetPositionHome();
    }

    delegate void MoveFoot(float time, FootRaycast foot);

    /// <summary>
    /// Walking Animation
    /// </summary>
    void AnimateWalk() {

        MoveFoot moveFoot = (t, foot) => {

            // Calculate foot position with sin/cos
            float y = Mathf.Cos(t) * walkSpreadY; // Vertival Movement

            float lateral = Mathf.Sin(t) * walkSpreadZ; // Forward/Backward Movement

            // foot.transform.parent gets the transform values of the parent of foot 
            Vector3 localDir = foot.transform.parent.InverseTransformDirection(input);


            float x = lateral * localDir.x;
            float z = lateral * localDir.z;


            float alignment = Mathf.Abs(Vector3.Dot(localDir, Vector3.forward));
            //  1 = Forward
            //  1 = Backward
            //  0 = Strafing

            // Calmp y value to 0
            if (y < 0) y = 0; 


            // Move Foot to calculated position
            foot.SetPositionOffset(new Vector3(x, y, z), footSeparateAmount * alignment);
        };

        walkTime += Time.deltaTime * input.sqrMagnitude * walkFootSpeed;

        // What does Invoke do?
        moveFoot.Invoke(walkTime, footLeft);
        moveFoot.Invoke(walkTime + Mathf.PI,  footRight);

    }


}
