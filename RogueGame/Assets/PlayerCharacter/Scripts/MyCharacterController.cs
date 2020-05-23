using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCharacterController : MonoBehaviour
{
    Animator anim;
    CharacterController charController;

    Vector3 lastVec = Vector3.forward;

    public float speed;

    bool isGrounded = false;

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

        Vector3 gravVec = charController.isGrounded ? Vector3.zero : new Vector3(0, -10, 0);

        charController.Move((moveVec * Time.deltaTime * speed) + (gravVec * Time.deltaTime));

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
