using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    private float directionDampTime = 0.25f;

    #region MonoBehaviour Callbacks
    void Start()
    {
        {
            animator = GetComponent<Animator>();
            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }
    }

    void Update()
    {
        if (!animator)
        {
            return;
        }
        
        Jumping();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v < 0)
        {
            v = 0;
        }
        animator.SetFloat("Speed", h * h + v * v);
        animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);
    }

    void Jumping()
    {
        // deal with Jumping
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // only allow jumping if we are running.
        if (stateInfo.IsName("Base Layer.Run"))
        {
            // When using trigger parameter
            if (Input.GetButtonDown("Fire2"))
            {
                animator.SetTrigger("Jump");
            }
        }
    }
    #endregion

}
