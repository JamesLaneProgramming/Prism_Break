using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : MonoBehaviour
{
    [SerializeField]
    float dmg, initSpeed, inwardsAccel;

    bool casted, midCast;
    float initTime, setInitSpeed, setInwardsAccel, midDis;
    Vector3 moveTarget, initPos;
    GameObject player;

    public bool GetCasted() { return casted; }

    public void Init(GameObject player)
    {
        casted = false;
        this.player = player;
        transform.parent = player.transform;
        gameObject.SetActive(false);
    }

    void Update()
    {
        CalcMove();
    }

    void FixedUpdate()
    {
        UpdateMove();
    }

    public void Cast()
    {
        gameObject.SetActive(true);
        casted = true;
        midCast = false;
        initTime = Time.time;
        initPos = player.transform.position;
        transform.position = initPos;
        Debug.Log(initPos + " " + player.transform.position + " " + transform.position);
        setInitSpeed = initSpeed;
        setInwardsAccel = inwardsAccel;
    }

    void CalcMove()
    {
        if (casted)
        {
            if (transform.localPosition.z < midDis && midCast)
            {
                moveTarget = Vector3.MoveTowards(transform.position, player.transform.position, initSpeed * Time.deltaTime);
            }

            else
            {
                float currentTime = Time.time - initTime;
                float displacement = (setInitSpeed * currentTime) + (setInwardsAccel * (currentTime * currentTime * currentTime));
                moveTarget = new Vector3(initPos.x + displacement, transform.position.y, transform.position.z + (setInitSpeed * Time.deltaTime));
            }
        }
    }

    void UpdateMove()
    {
        if (casted)
        {
            transform.position = moveTarget;
            transform.LookAt(moveTarget);

            if (transform.localPosition.x < initPos.x && !midCast)
            {
                midCast = true;
                initTime = Time.time;
                midDis = (transform.position.z - initPos.z) / 2;
                initPos = transform.position;
                setInitSpeed = -initSpeed;
                setInwardsAccel = -inwardsAccel;
            }

            if (transform.position == player.transform.position && midCast)
            {
                casted = false;
                gameObject.SetActive(false);
            }
        }
    }
}