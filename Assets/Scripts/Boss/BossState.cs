using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossState : MonoBehaviour
{
    public enum State
    {
        NONE,
        SLEEP,
        PATROL,
        CHASE,
        ATTACK,
        SHOOT,
        DEATH
    }

    private Transform playerTarget;
    private State bossState = State.SLEEP;
    public State state
    {
        get { { return bossState; } }
    }
    private float distanceToTarget;

    private EnemyHealth enemyHealth;

    private void Awake()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        enemyHealth = GetComponent<EnemyHealth>();
        bossState = State.SLEEP;
    }
    private void Update()
    {
        SetState();
    }
    void SetState()
    {
        distanceToTarget = Vector3.Distance(transform.position, playerTarget.position);

        if (bossState == State.SLEEP)
        {
            int enemyCount = FindObjectsOfType<EnemyWaypointTracker>().Length;
            if (enemyHealth.currentHealth < enemyHealth.maxHealth)
            {
                bossState = State.NONE;
            }
            else if (distanceToTarget <= 4f)
            {
                bossState = State.NONE;
            }
            else if (enemyCount <= 0)
            {
                bossState = State.NONE;
            }
            else
            {
                bossState = State.SLEEP;
            }
        }
        else if (bossState != State.DEATH || bossState != State.SLEEP)
        {
            if (distanceToTarget > 4f && distanceToTarget <= 8f)
            {
                bossState = State.CHASE;
            }
            else if (distanceToTarget > 8f && distanceToTarget <= 10f)
            {
                bossState = State.SHOOT;
            }
            else if (distanceToTarget > 10f)
            {
                bossState = State.PATROL;
            }
            else if (distanceToTarget <= 5f)
            {
                bossState = State.ATTACK;
            }
            else 
            {
                bossState = State.NONE;
            }
        }
        if (enemyHealth.currentHealth <= 0f)
        {
            bossState = State.DEATH;
        }
    }


}
