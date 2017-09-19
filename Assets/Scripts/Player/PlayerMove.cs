using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState
{
    NONATTACK,
    RIGHT,
    LEFT,
    UPPER
}

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    int health;
    [SerializeField]
    float moveSpeed, punchWindow, rollSpeed, rollTime, jumpInitial;
    [SerializeField]
    int boomerangCount;
    [SerializeField]
    Animator anim;
    [SerializeField]
    AnimationClip[] animClips;

    bool canMove, jump;
    AttackState aState;
    float jumpTime, yOffset;
    Vector3 moveTarget;
    Coroutine punchTimer;
    GameObject punchBox, grapple;
    GameObject[] boomerangs;

    void Start()
    {
        canMove = true;
        aState = AttackState.NONATTACK;
        moveTarget = transform.position;
        punchBox = transform.GetChild(0).gameObject;
        boomerangs = new GameObject[boomerangCount];
        GameObject grappleObj = Resources.Load<GameObject>("Prefabs/Grapple");
        grapple = Instantiate(grappleObj);
        grapple.GetComponent<Grapple>().SetPlayer(gameObject);

        //for (int i = 0; i < boomerangCount; i++)
        //{
        //    boomerangs[i] = Instantiate(grappleObj);
        //    boomerangs[i].GetComponent<Boomerang>().Init(gameObject);
        //}
    }

    void Update()
    {
        CalcMove();
        CalcJump();
        Inputs();
    }

    void FixedUpdate()
    {
        UpdateMove();
        UpdateJump();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            jump = false;
            anim.SetTrigger("Land");
        }

        if (col.gameObject.GetComponent<Boss_Base_Class>())
        {
            health--;
            if(health == 0)
            {
                //GameOver
                GameObject.Find("Canvas").GetComponent<UIScript>().show_Credits();
            }
        }
    }

    void CalcMove()
    {
        if (canMove)
        {
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");

            anim.SetFloat("Speed", Mathf.Abs(xMove) > 0.2f || Mathf.Abs(zMove) > 0.2f ? Mathf.Max(Mathf.Abs(xMove), Mathf.Abs(zMove)) : 1);
            anim.SetBool("Run", Mathf.Abs(xMove) > 0.2f || Mathf.Abs(zMove) > 0.2f ? true : false);

            Vector3 moveDir = transform.TransformDirection(xMove, 0, zMove);

            if (xMove != 0)
            {
                transform.GetChild(1).LookAt(transform.position + moveDir);
            }

            else
            {
                transform.GetChild(1).LookAt(transform.position + transform.forward);
            }

            moveTarget = Vector3.MoveTowards(transform.position, transform.position + moveDir, moveSpeed * Time.deltaTime);
        }
    }

    void UpdateMove()
    {
        transform.position = moveTarget;
    }

    void Inputs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Roll();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitJump();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ThrowBoomerang();
            canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Grapple();
            Grapple g = grapple.GetComponent<Grapple>();

            if (!g.GetLaunched())
            {
                g.Launch();
            }
            canMove = false;
        }
    }

    void Attack()
    {
        if (aState != AttackState.UPPER)
        {
            punchBox.SetActive(true);

            if (punchTimer != null)
            {
                StopCoroutine(punchTimer);
            }

            punchTimer = StartCoroutine(PunchTimer());
            aState++;
            anim.SetBool("Idle", false);

            switch (aState)
            {
                case AttackState.NONATTACK:

                    break;

                case AttackState.RIGHT:
                    anim.SetTrigger("Right Punch");

                    break;

                case AttackState.LEFT:
                    anim.SetTrigger("Left Punch");

                    break;

                case AttackState.UPPER:
                    anim.SetTrigger("Uppercut");

                    break;
            }
        }
    }

    void Roll()
    {
        StartCoroutine(RollTimer());
        aState = AttackState.NONATTACK;
    }

    void InitJump()
    {
        if (!jump)
        {
            aState = AttackState.NONATTACK;
            anim.SetTrigger("Jump");
            jumpTime = Time.time;
            jump = true;
        }
    }

    void CalcJump()
    {
        if (jump)
        {
            float currentTime = Time.time - jumpTime;
            yOffset = (jumpInitial * currentTime) + ((Physics.gravity.y * (currentTime * currentTime) / 2));
            anim.SetFloat("Fall", yOffset);
        }
    }

    void UpdateJump()
    {
        if (jump)
        {
            transform.position = transform.position + new Vector3(0, yOffset, 0);
        }
    }

    void ThrowBoomerang()
    {
        for (int i = 0; i < boomerangCount; i++)
        {
            if (!boomerangs[i].GetComponent<Boomerang>().GetCasted())
            {
                boomerangs[i].GetComponent<Boomerang>().Cast();

                break;
            }
        }
    }
    
    void Grapple()
    {
        
    }

    void Block()
    {
        // Can't Damage. Timer?
    }

    void Invis()
    {
        // Invis Material. Timer?
    }

    void FallCheck()
    {
        if (GetComponent<Rigidbody>().velocity.y < 0)
        {
            anim.SetFloat("Fall", -1);
        }
    }

    IEnumerator PunchTimer()
    {
        yield return new WaitForSeconds(0.1f);
        punchBox.SetActive(false);
        yield return new WaitForSeconds(animClips[(int)aState - 1].length * punchWindow);
        aState = AttackState.NONATTACK;
        anim.SetBool("Idle", true);
    }

    IEnumerator RollTimer()
    {
        moveSpeed += rollSpeed;
        yield return new WaitForSeconds(rollTime);
        moveSpeed -= rollSpeed;
    }
}