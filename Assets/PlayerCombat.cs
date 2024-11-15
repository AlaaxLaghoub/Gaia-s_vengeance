using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator animator;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S)){
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {

            Shield();
        }
    }
    void Attack(){
        animator.SetTrigger("Attack");
    }
    void Shield()
    {
        animator.SetBool("Shield",true);
    }
}
