using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController2D), typeof(NavAgent))]
public class InputAgent : MonoBehaviour
{
    NavAgent thisNavAgent;
    Brain thisBrain;
    Weapon thisWeapon;

    public bool JumpDown { get { return Input.GetButton("Jump"); } }
    
    void Awake()
    {
        thisBrain = GetComponent<Brain>();
        thisNavAgent = GetComponent<NavAgent>();
        thisWeapon = GetComponent<Weapon>();
    }

    void Update()
    {
        float moveX = 0f;
        bool doJump = false;
        bool doFire = false;
        if( thisBrain != null )
        {
            thisBrain.DecideInput(out moveX, out doJump);
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            doJump = Input.GetButtonDown("Jump");
            doFire = Input.GetButtonDown("Fire1");
        }
        
        thisNavAgent.Walk(moveX);

        if( thisNavAgent.isGrounded && doJump )
            thisNavAgent.Jump();

        if( thisWeapon != null && doFire )
            thisWeapon.Fire(moveX);
    }
}
