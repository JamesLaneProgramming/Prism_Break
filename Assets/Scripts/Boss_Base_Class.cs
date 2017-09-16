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
    public float attack_Recover_Time;
    private bool is_Recovering = false;

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
        switch (current_Boss_State)
        {
            case BOSS_STATE.idle:
                current_Boss_State = get_Next_Action();
                break;
            case BOSS_STATE.moving:
                move();
                break;
            case BOSS_STATE.attacking:
                if(!is_Recovering)
                    attack();
                break;
            case BOSS_STATE.recovering:
                break;
            default:
                break;
        }
    }

    private BOSS_STATE get_Next_Action()
    {
        float distance_To_Player = Vector3.Distance(player.transform.position, this.transform.position);
        if(distance_To_Player <= attack_Radius)
        {
            return BOSS_STATE.attacking;
        }
        else
        {
            return BOSS_STATE.moving;
        }
    }
    private void attack()
    {
        print("Attacking");
        StartCoroutine(recover());
    }
    private void move()
    {
        transform.position = Vector3.MoveTowards(transform.position, calculate_Target_Position(player), 1);
        print("Moving");
        current_Boss_State = get_Next_Action();
    }

    private Vector3 calculate_Target_Position(GameObject target)
    {
        Vector3 target_Direction = (target.transform.position - transform.position).normalized;
        return transform.position + (target_Direction * movement_Speed);
    }

    private IEnumerator recover()
    {
        print("Recovering");
        is_Recovering = true;
        yield return new WaitForSeconds(attack_Recover_Time);
        is_Recovering = false;
        current_Boss_State = get_Next_Action();
    }
}
