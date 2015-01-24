using UnityEngine;
using System.Collections;

public class InputAgent : MonoBehaviour
{
    CharacterController2D thisController;
    NavAgent thisNavAgent;
    
    void Awake()
    {
        thisController = GetComponent<CharacterController2D>();
        if( thisController == null )
            Debug.LogWarning("InputAgent: Missing CharacterController2D");

        thisNavAgent = GetComponent<NavAgent>();
        if( thisNavAgent == null )
            Debug.LogWarning("InputAgent: Missing NavAgent");
    }

    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        thisNavAgent.Walk(moveX);

        if( thisController.IsGrounded && Input.GetButtonDown("Jump") )
            thisNavAgent.Jump();
    }
}
