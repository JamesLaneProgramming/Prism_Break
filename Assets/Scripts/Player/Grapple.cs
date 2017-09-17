using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GrappleState
{
    CAST,
    ENVIRO,
    ENEMY
}

public class Grapple : MonoBehaviour
{
    [SerializeField]
    float maxDis, moveSpeed, playerGrapSpeed, enemyGrapSpeed;

    bool launched, maxReached;
    float setSpeed;
    GrappleState gState;
    Vector3 moveTarget, playerMoveTarget, enemyMoveTarget, initPos, maxPos, grapOffset;
    GameObject player, grapHit;

    public bool GetLaunched() { return launched; }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
        transform.position = player.transform.position;
    }
    
    void OnEnable()
    {
        if (player != null)
        {
            transform.position = initPos + player.transform.forward;
        }
    }

    void Update()
    {
        CalcMove();
        CalcHooked();
    }

    void FixedUpdate()
    {
        UpdateMove();
        UpdateHooked();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Environment"))
        {
            gState = GrappleState.ENVIRO;
            grapHit = col.gameObject;
            grapOffset = (col.transform.position - transform.position) * 2;
            maxReached = true;
            setSpeed = playerGrapSpeed;
        }

        if (col.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            gState = GrappleState.ENEMY;
            grapHit = col.gameObject;
            grapOffset = (col.transform.position - transform.position) * 2;
            maxReached = true;
            setSpeed = enemyGrapSpeed;
        }
    }

    public void Launch()
    {
        launched = true;
        maxReached = false;
        gState = GrappleState.CAST;
        setSpeed = moveSpeed;
        initPos = player.transform.position;
        maxPos = initPos + (player.transform.forward * maxDis);
        transform.LookAt(initPos + player.transform.forward);
        gameObject.SetActive(true);
    }

    void CalcMove()
    {
        if (launched)
        {
            Vector3 direction = transform.position - player.transform.position;
            float distance = direction.magnitude;

            if (distance >= maxDis)
            {
                maxReached = true;
            }

            if (distance <= 1.0f)
            {
                launched = false;
                gameObject.SetActive(false);
            }

            if (!maxReached)
            {
                moveTarget = Vector3.MoveTowards(transform.position, maxPos, setSpeed * Time.deltaTime);
            }

            else
            {
                moveTarget = Vector3.MoveTowards(transform.position, player.transform.position, setSpeed * Time.deltaTime);
            }
        }
    }

    void UpdateMove()
    {
        if (launched)
        {
            transform.position = moveTarget;
        }
    }

    void CalcHooked()
    {
        if (launched && maxReached)
        {
            switch (gState)
            {
                case GrappleState.CAST:

                    break;
                case GrappleState.ENVIRO:
                    playerMoveTarget = Vector3.MoveTowards(transform.position, grapHit.transform.position - grapOffset, setSpeed * Time.deltaTime);

                    break;
                case GrappleState.ENEMY:
                    enemyMoveTarget = Vector3.MoveTowards(grapHit.transform.position, player.transform.position - grapOffset, setSpeed * Time.deltaTime);

                    break;
            }
        }
    }

    void UpdateHooked()
    {
        if (launched && maxReached)
        {
            switch (gState)
            {
                case GrappleState.CAST:

                    break;
                case GrappleState.ENVIRO:
                    player.transform.position = playerMoveTarget;

                    break;
                case GrappleState.ENEMY:
                    grapHit.transform.position = enemyMoveTarget;

                    break;
            }
        }
    }
}