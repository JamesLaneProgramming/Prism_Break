using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    float moveSpeed, rollSpeed, rollTime, jumpInitial;
    [SerializeField]
    int boomerangCount;

    bool canMove, jump;
    float jumpTime, yOffset;
    Vector3 moveTarget;
    GameObject grapple;
    GameObject[] boomerangs;

    void Start()
    {
        canMove = true;
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
        }
    }

    void CalcMove()
    {
        if (canMove)
        {
            float xMove = Input.GetAxis("Horizontal");
            float zMove = Input.GetAxis("Vertical");
            
            Vector3 moveDir = transform.TransformDirection(xMove, 0, zMove);

            moveTarget = Vector3.MoveTowards(transform.position, transform.position + moveDir, moveSpeed * Time.deltaTime);
        }
    }

    void UpdateMove()
    {
        transform.position = moveTarget;
    }

    void Inputs()
    {
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
            //canMove = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Grapple();
            Grapple g = grapple.GetComponent<Grapple>();

            if (!g.GetLaunched())
            {
                g.Launch();
            }
            //canMove = false;
        }
    }

    void Roll()
    {
        StartCoroutine(RollTimer());
    }

    void InitJump()
    {
        if (!jump)
        {
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

    IEnumerator RollTimer()
    {
        moveSpeed += rollSpeed;
        yield return new WaitForSeconds(rollTime);
        moveSpeed -= rollSpeed;
    }
}