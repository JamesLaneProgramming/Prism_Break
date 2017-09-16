using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BOSS_STATE
{
    idle,
    moving,
    attacking,
    recovering
};

public class Boss_Base_Class : MonoBehaviour {

    private float movement_Radius;
    [Range(0, 1)]
    public float movement_Speed;
    [HideInInspector]
    public bool is_Attacking = false;
    [HideInInspector]
    public bool is_Moving = false;

    [HideInInspector]
    public bool can_Attack = true;

    [HideInInspector]
    public float time_Targeted;
    [HideInInspector]
    public bool has_Recovered;

    public float attack_Radius;

    public GameObject player;

    private BOSS_STATE current_Boss_State = BOSS_STATE.idle;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void FixedUpdate()
    {
        print(current_Boss_State);
        switch (current_Boss_State)
        {
            case BOSS_STATE.idle:
                get_Next_Action();
                break;
            case BOSS_STATE.moving:
                if(is_Moving)
                {
                    continue_Moving();
                }
                else
                {
                    end_Moving();
                }
                break;
            case BOSS_STATE.attacking:
                if(is_Attacking)
                {
                    continue_Attack();
                }
                else
                {
                    end_Attack();
                }
                break;
            case BOSS_STATE.recovering:
                if(has_Recovered)
                {
                    end_Recover();
                }
                else
                {
                    continue_Recover();
                }
                break;
            default:
                break;
        }
    }

    private void check_Can_Attack()
    {
        Vector2 difference_In_Distance = new Vector2((player.transform.position.x - transform.position.x), (player.transform.position.z - transform.position.z));
        float distance_To_Player = difference_In_Distance.magnitude;
        if (distance_To_Player <= attack_Radius)
        {
            can_Attack = true;
        }
        else
        {
            can_Attack = false;
        }
    }
    private void get_Next_Action()
    {
        check_Can_Attack();
        if(can_Attack)
        {
            start_Attack();
        }
        else
        {
            start_Moving();
        }
    }

    public virtual void start_Attack()
    {
        current_Boss_State = BOSS_STATE.attacking;
        is_Attacking = true;
        time_Targeted = Time.time;
    }

    public virtual void continue_Attack()
    {

    }

    public virtual void end_Attack()
    {
        start_Recover();
    }

    public virtual void start_Moving()
    {
        current_Boss_State = BOSS_STATE.moving;
        is_Moving = true;
    }

    public virtual void continue_Moving()
    {
        transform.position = Vector3.MoveTowards(transform.position, calculate_Target_Position(player), 1);
        check_Can_Attack();
        if(can_Attack)
        {
            get_Next_Action();
            is_Moving = false;
        }
    }
    
    public virtual void end_Moving()
    {

    }

    public Vector3 calculate_Target_Position(GameObject target)
    {
        Vector3 target_Direction = (target.transform.position - transform.position).normalized;
        return transform.position + (target_Direction * movement_Speed);
    }

    public virtual void start_Recover()
    {
        current_Boss_State = BOSS_STATE.recovering;
        has_Recovered = false;
    }
    public virtual void continue_Recover()
    {

    }
    public virtual void end_Recover()
    {
        current_Boss_State = BOSS_STATE.idle;
    }
    public delegate void MyDelegateType();
    public MyDelegateType functionToRun;

    public IEnumerator wait(float time, MyDelegateType _functionToRun)
    {
        functionToRun += _functionToRun;
        yield return new WaitForSeconds(time);
        functionToRun();
        functionToRun -= _functionToRun;
    }
}
