using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GuardianMovement : MonoBehaviour
{
    public float walkSpeed = 5;

    // References to Components
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Inpput
        float v = Input.GetAxis("Vertical");

        // Movement
        transform.position += transform.forward * v * Time.deltaTime * walkSpeed;

        // Animate
        animator.SetFloat("speed", Mathf.Abs(v * walkSpeed));
    }
}
