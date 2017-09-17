using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Orange_Boss : Boss_Base_Class
{
    public float height;
    public float attack_Charge_Time;
    public float attack_Charge_Force;
    public float recover_Speed;
    public float time_Before_Recover;
    private Rigidbody my_Rigidbody;
    Vector3 target;

    private void Awake()
    {
        my_Rigidbody = GetComponent<Rigidbody>();
    }

    public override void start_Recover()
    {
        target = new Vector3(transform.position.x, height, transform.position.z);
        base.start_Recover();
    }
    public override void continue_Recover()
    {
        print(target);
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, height, transform.position.z), recover_Speed);
        if (transform.position == target)
        {
            has_Recovered = true;
        }
    }

    public override void continue_Moving()
    {
        Vector3 target_Position = Vector3.MoveTowards(transform.position, calculate_Target_Position(player), 1);
        transform.position = new Vector3(target_Position.x, height, target_Position.z);
        base.continue_Moving();
    }

    public override void continue_Attack()
    {
        if ((Time.time - time_Targeted) > attack_Charge_Time)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position - (Vector3.up), attack_Charge_Force);
        }
        base.continue_Attack();
    }

    public override void end_Attack()
    {
        StartCoroutine(wait(time_Before_Recover, base.end_Attack));
    }
    private void OnCollisionEnter(Collision collided_Object)
    {
        is_Attacking = false;
        
    }
}
