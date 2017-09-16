using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///public class Green_Boss : Boss_Base_Class {
///
///    public Transform target;
///    public float panSpeed = 5f;
///    public float distanceMin = .5f;
///    public float distanceMax = 15f;
///    private float distance = 0f;
///    private float x = 0.0f;
///    public float attack_Charge_Time;
///    public float attack_Charge_Force;
///    private bool is_Moving_Right;
///    //public Grapple grappling_Gun;
///
///    private void Start()
///    {
///        target.transform.SetParent(null);
///        Vector3 angles = transform.eulerAngles;
///        x = angles.x;
///    }
///
///    void Orbit()
///    {
///        if(is_Moving_Right)
///        {
///            x += Random.Range(0, 1 * panSpeed);
///        }
///        else
///        {
///            x += Random.Range(-1 * panSpeed, 0);
///        }
///        
///    }
///
///    public void Movement()
///    {
///        distance = Vector3.Distance(target.position, /transform.position) /-/ movement_Speed;
///
///        if (target != null)
///        {
///            Quaternion rotation = Quaternion.Euler(0, x, 0);
///            float desiredDist = distance;
///
///            distance = Mathf.Clamp(desiredDist, distanceMin, //distanceMax);
///
///            Vector3 invDistanceZ = new Vector3(0, 0, -(distance));
///            invDistanceZ = rotation * invDistanceZ;
///
///            Vector3 position = target.position + invDistanceZ;
///
///            transform.rotation = rotation;
///            transform.position = position;
///        }
///    }
///
///    public override void start_Attack()
///    {
///        is_Moving_Right = !is_Moving_Right;
///        //grappling_Gun.shoot(player);
///        base.start_Attack();
///    }
///
///    public override void continue_Attack()
///    {
///        transform.LookAt(player.transform);
///        if ((Time.time - time_Targeted) > attack_Charge_Time)
///        {
///            is_Attacking = false;
///        }
///        base.continue_Attack();
///    }
///
///    public override void continue_Moving()
///    {
///        Orbit();
///        Movement();
///        base.continue_Moving();
///    }
///
///    private void OnCollisionEnter(Collision collided_Object)
///    {
///        if(collided_Object.gameObject.tag == "Player")
///        {
///            collided_Object.transform.position = new Vector3(0, 0.5f, /0);
///            end_Attack();
///        }
///    }
///}
///