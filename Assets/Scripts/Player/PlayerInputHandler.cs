using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerInputHandler : MonoBehaviour
{
    private bool isToggleCrouching = false;
    private bool isToggleSprinting = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(Keys.KToggleCrouch))
        {
            isToggleCrouching = !isToggleCrouching;
            //enable crouchsprint thats why commented
            /*if (isToggleSprinting && isToggleCrouching) // if sprinting and now toggle crouching then no sprinting
            {
                isToggleSprinting = false;
            }*/
        }
        if (Input.GetKeyDown(Keys.KToggleSprint)) 
        {
            isToggleSprinting = !isToggleSprinting;
            /*if (isToggleCrouching && isToggleSprinting)// if crouching and now sprinting then no 
            {
                isToggleCrouching = false;
            }*/
        }
    }

    public bool IsJumping()
    {
        if (Input.GetKey(Keys.KJump))
        {
            isToggleCrouching = false;
            return true;
        }
        return false;
    }
    
    public bool IsCrouching()
    {
        return (Input.GetKey(Keys.KCrouch) || isToggleCrouching);
    }
    
    public bool IsSprinting()
    {
        if (Input.GetKey(Keys.KSprint))
        {
            isToggleCrouching = false;
            return true;
        }
        return isToggleSprinting;
    }
/// <summary>
/// adjusted for:
/// deltaTime;
/// length: normalized;
/// rotation of player;
/// keys in settings;
/// </summary>
/// <returns>The Movement Direction in worldSpace</returns>
    public Vector3 GetMovementInput()
    {
        Vector3 ret = Vector3.zero;
        if (Input.GetKey(Keys.KForward))
        {
            ret += transform.forward;
        }
        if (Input.GetKey(Keys.KBackward))
        {
            ret += -transform.forward;
        }
        if (Input.GetKey(Keys.KRight))
        {
            ret += transform.right;
        }
        if (Input.GetKey(Keys.KLeft))
        {
            ret += -transform.right;
        }
        ret.Normalize();
        ret *= Time.deltaTime;
        return ret;
    }
}
