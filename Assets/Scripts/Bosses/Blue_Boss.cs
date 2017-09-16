using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Blue_Boss : Boss_Base_Class {

    public Animator my_Animator;
    private bool is_Vulnerable;
    public float vulnerable_Time;
    private float time_Anim_Started;

    public override void continue_Moving()
    {
        transform.LookAt(player.transform);
        transform.position = Vector3.MoveTowards(transform.position, calculate_Target_Position(player), 1);
        base.continue_Moving();
    }

    private void OnCollisionEnter(Collision collided_Object)
    {
        //if(collided_Object.gameObject.GetComponent<Grapple>())
        //{
        grapple();
        //}
    }

    public override void start_Attack()
    {
        time_Anim_Started = Time.time;
        my_Animator.SetBool("Is_Attacking", true);
        base.start_Attack();
    }

    public override void continue_Attack()
    {
        if (my_Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            is_Attacking = false;
        }
        base.continue_Attack();
    }

    ///public override void continue_Recover()
    ///{
    ///    wait(2f, set_Has_Recovered);
    ///    base.continue_Recover();
    ///}
    ///
    ///private void set_Has_Recovered()
    ///{
    ///    has_Recovered = true;
    ///}

    public override void end_Attack()
    {
        my_Animator.SetBool("Is_Attacking", false);
        base.end_Attack();
    }

    public void grapple()
    {
        my_Animator.SetBool("Is_Grappled", true);
        ///StartCoroutine(wait(vulnerable_Time, closeGrapple));
    }

    public void closeGrapple()
    {
        my_Animator.SetBool("Is_Grappled", false);
    }
}
