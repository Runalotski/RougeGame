using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    Animator anim;
    CharacterController charController;

    Vector3 lastVec = Vector3.zero;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        charController = GetComponent<CharacterController>();
        anim.applyRootMotion = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVec = new Vector3(Input.GetAxisRaw("Horizontal"), 0 , Input.GetAxisRaw("Vertical")).normalized;
        charController.Move(moveVec * Time.deltaTime * speed);

        if (moveVec != Vector3.zero)
        {
            anim.SetBool("IsMoving", true);
            lastVec = moveVec;
        }
        else
        {
            anim.SetBool("IsMoving", false);
        }

        

        transform.forward = lastVec;
    }
}
