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
        float moveY = 0f;
        bool doJump = false;
        bool doFire = false;
        if( thisBrain != null )
        {
            thisBrain.DecideInput(out moveX, out doJump);
        }
        else
        {
            moveX = Input.GetAxis("Horizontal");
            moveY = Input.GetAxis("Vertical");
            doJump = Input.GetButtonDown("Jump");
            doFire = Input.GetButtonDown("Fire1");
        }
        
        thisNavAgent.Walk(moveX);

        if( thisNavAgent.isGrounded && doJump )
        {
            if( moveY < 0f && thisNavAgent.CanSlide )
                thisNavAgent.Slide();
            else
                thisNavAgent.Jump();
        }

        if( !thisNavAgent.isGrounded && doJump && thisNavAgent.CanFlap )
            thisNavAgent.Flap();

        if( thisWeapon != null && doFire )
            thisWeapon.Fire(moveX);
    }
}
