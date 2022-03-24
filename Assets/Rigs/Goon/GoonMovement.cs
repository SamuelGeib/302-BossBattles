using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GoonMovement : MonoBehaviour
{
    public FootRaycast footLeft;
    public FootRaycast footRight;

    public float speed = 2;
    public float walkSreadX = 0.2f;
    public float walkSpreadY = .4f;
    public float walkSpreadZ = .4f;
    public float walkFootspeed = 4; // Multiplier

    private CharacterController pawn;

    void Start()
    {
        pawn = GetComponent<CharacterController>();
    }


    void Update()
    {
        // Inpput Variables:
        float v = Input.GetAxis("Vertical");    
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v + transform.right * h;
        if (move.sqrMagnitude > 1) move.Normalize();

        pawn.SimpleMove(move * speed);

        AnimateWalk();
    }

    delegate void MoveFoot(float time, float x, FootRaycast foot);
    void AnimateWalk() {

        MoveFoot moveFoot = (t, x, foot) => {

            float y = Mathf.Cos(t) * walkSpreadY;
            float z = Mathf.Sin(t) * walkSpreadZ;

            if (y < 0) y = 0;

            y += .177f;

            foot.transform.localPosition = new Vector3(x, y, z);
        };

        float t = Time.time * walkFootspeed;

        moveFoot.Invoke(t, -walkSreadX,  footLeft);
        moveFoot.Invoke(t + Mathf.PI, walkSreadX,  footRight);

    }


}
