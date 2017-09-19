using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class Red_Boss : Boss_Base_Class {

    public float attack_Charge_Delay;
    [Range(0, 1)]
    public float attack_Charge_Force;
    private Rigidbody my_Rigidbody;
    private Vector3 targetDirection;
    private Vector3 target_Position;

    private bool is_Stunned;
    public float stun_Duration;

    [Range(0, 0.1f)]
    public float rotation_Speed;

    private void Start()
    {
        my_Rigidbody = GetComponent<Rigidbody>();
    }

    public override void continue_Moving()
    {
        Vector3 target_Rotation = Vector3.RotateTowards(transform.forward, new Vector3((player.transform.position.x - transform.position.x), 0, (player.transform.position.z - transform.position.z)).normalized, 1 * rotation_Speed, 1);
        transform.rotation = Quaternion.LookRotation(target_Rotation);
        transform.position = Vector3.MoveTowards(transform.position, calculate_Target_Position(player), 1);
        base.continue_Moving();
    }

    public override void start_Attack()
    {
        targetDirection = new Vector3((player.transform.position.x - transform.position.x), 0, (player.transform.position.z - transform.position.z));
        base.start_Attack();
    }
    public override void continue_Attack()
    {
        //transform.LookAt(player.transform);
        if ((Time.time - time_Targeted) > attack_Charge_Delay)
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + targetDirection, attack_Charge_Force);
        }
        base.continue_Attack();
    }

    public override void start_Recover()
    {
        target_Position = transform.position - targetDirection;
        base.start_Recover();
        StartCoroutine(wait(stun_Duration, reset_Pos));
    }
    public override void continue_Recover()
    {
        if(is_Stunned == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_Position, 0.1f);
            if ((transform.position - target_Position).magnitude <= 0.2f)
            {
                has_Recovered = true;
            }
        }
        base.continue_Recover();
    }

    public void OnCollisionEnter(Collision collided_Object)
    {
        //base.OnCollisionEnter(collided_Object);
        if(collided_Object.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            is_Stunned = true;
        }
        is_Attacking = false;
    }

    public void reset_Pos()
    {
        is_Stunned = false;
    }
}
