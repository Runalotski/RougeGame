using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowClass : MonoBehaviour, IWeaponClass
{

    private byte _handedness = 2;
    public byte handedness { get { return _handedness; } set => throw new System.NotImplementedException(); }

    public Transform owner { get; set; }

    public Transform projectile;

    /// <summary>
    /// How many times a second will the wepon attack
    /// </summary>
    public float _attackSpeed;
    public float attackSpeed { get { return _attackSpeed; } set { _attackSpeed = value; } }

    bool canAttack = true;
    private IEnumerator coroutine;

    public void Attack()
    {
        if (canAttack)
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Vector3 shootDir = Vector3.zero;
            float angle = 0;
            if (Physics.Raycast(mouseRay, out hit, 100))
            {
                shootDir = hit.point - transform.position;
                shootDir.y = 0;
                shootDir.Normalize();
                angle = Mathf.Acos(Vector3.Dot(Vector3.forward, shootDir)) * Mathf.Rad2Deg;

                if (hit.point.x < transform.position.x)
                    angle = 360 - angle;

            }

            Instantiate(projectile, transform.position + (shootDir) - new Vector3(0, 1.5f, 0), Quaternion.Euler(0, angle, 0));
            canAttack = false;
            StartCoroutine(AttackTimer());
        }   
    }

    IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1 / _attackSpeed);
        canAttack = true;
        yield return null;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        coroutine = AttackTimer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
