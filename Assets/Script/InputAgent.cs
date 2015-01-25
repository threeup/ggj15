using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController2D), typeof(NavAgent))]
public class InputAgent : MonoBehaviour
{
    CharacterController2D thisController;
    NavAgent thisNavAgent;
    Brain thisBrain;
    
    void Awake()
    {
        thisBrain = GetComponent<Brain>();
        thisController = GetComponent<CharacterController2D>();
        thisNavAgent = GetComponent<NavAgent>();
    }

    void Update()
    {
        float moveX = 0f;
        bool doJump = false;
        if( thisBrain != null )
        {
            thisBrain.DecideInput(out moveX, out doJump);
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            doJump = Input.GetButtonDown("Jump");
        }
        
        thisNavAgent.Walk(moveX);

        if( thisController.IsGrounded && doJump )
            thisNavAgent.Jump();
    }
}
