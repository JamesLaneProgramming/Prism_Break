///using System.Collections;
///using System.Collections.Generic;
///using UnityEngine;
///[RequireComponent(typeof(Rigidbody))]
///public class Red_Boss : Boss_Base_Class {
///
///    public float attack_Charge_Time;
///    public float attack_Charge_Force;
///    private Rigidbody my_Rigidbody;
///    private Vector3 targetDirection;
///    private Vector3 target_Position;
///
///    private void Awake()
///    {
///        my_Rigidbody = GetComponent<Rigidbody>();
///    }
///
///    public override void continue_Moving()
///    {
///        transform.LookAt(player.transform);
///        transform.position = Vector3.MoveTowards(transform.position, ///calculate_Target_Position(player), 1);
///        base.continue_Moving();
///    }
///
///    public override void start_Attack()
///    {
///        targetDirection = (player.transform.position - ///transform.position);
///        base.start_Attack();
///    }
///    public override void continue_Attack()
///    {
///        transform.LookAt(player.transform);
///        if ((Time.time - time_Targeted) > attack_Charge_Time)
///        {
///            transform.position = Vector3.MoveTowards//(transform.position, /transform.position + targetDirection, //attack_Charge_Force);
///        }
///        base.continue_Attack();
///    }
///
///    ///public override void start_Recover()
///    ///{
///    ///    target_Position = transform.position - targetDirection;
///    ///    base.start_Recover();
///    ///}
///    ///public override void continue_Recover()
///    ///{
///    ///    transform.position = Vector3.MoveTowards///(transform.position, ///target_Position, 0.1f);
///    ///    if((transform.position - target_Position).magnitude <= 0.2f)
///    ///    {
///    ///        has_Recovered = true;
///    ///    }
///    ///    base.continue_Recover();
///    ///}
///
///    public void OnCollisionEnter(Collision collided_Object)
///    {
///        is_Attacking = false;
///    }
///}
///