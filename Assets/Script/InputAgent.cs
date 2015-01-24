using UnityEngine;
using System.Collections;

public class InputAgent : MonoBehaviour
{
    public AnimationCurve walkSpeed = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

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
        if( moveX != 0f )
            thisController.Move(Vector3.right * Mathf.Sign(moveX) * walkSpeed.Evaluate(Mathf.Abs(moveX)) * 0.1f);

        if( thisController.IsGrounded && Input.GetButtonDown("Jump") )
            thisNavAgent.Jump();
    }
}
