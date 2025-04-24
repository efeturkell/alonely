using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Transform playerTarget;
    private BossState bossStateChecker;
    private NavMeshAgent agent;
    private Animator anim;

    private bool finishedAttacking = true;
    private bool isTailAttacking = false; // Yeni eklenen bayrak

    public float turnSpeed;
    public float attackRate;
    private float currentAttackTime;
    private SphereCollider targetCollider;
    public static bool bossDeath = false;

    private List<GameObject> allWaypointsList = new List<GameObject>();

    [SerializeField] private GameObject fireball;
    [SerializeField] private Transform firePosition;

    private void Awake()
    {
        bossDeath = false;
        targetCollider = GetComponentInChildren<SphereCollider>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
        bossStateChecker = GetComponent<BossState>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        allWaypointsList.AddRange(GameObject.FindGameObjectsWithTag("Waypoints"));
    }

    void Update()
    {
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            if (!AudioManager.instance.sfx[10].isPlaying)
            {
                AudioManager.instance.PlaySfx(10);
            }
        }
        if (finishedAttacking)
        {
            GetControl();
        }
        else
        {
            anim.SetInteger("Attack", 0);
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                finishedAttacking = true;
                isTailAttacking = false; // TailAttack bittiðinde bayraðý sýfýrlayýn
            }
        }
        
    }

    void GetControl()
    {
        if (bossStateChecker.state == BossState.State.DEATH)
        {
            agent.isStopped = true;
            anim.SetBool("Death", true);
            targetCollider.enabled = true;
            bossDeath = true;
            AudioManager.instance.PlaySfx(7);
        }
        else
        {
            if (bossStateChecker.state == BossState.State.CHASE)
            {
                agent.isStopped = false;
                anim.SetBool("Run", true);
                anim.SetBool("WakeUp", true);
                anim.SetBool("Walk", false);
                agent.speed = 3f;
                agent.SetDestination(playerTarget.position);
            }
            else if (bossStateChecker.state == BossState.State.PATROL)
            {
                agent.isStopped = false;
                anim.ResetTrigger("Shoot");
                anim.SetBool("Run", false);
                anim.SetBool("WakeUp", true);
                anim.SetBool("Walk", true);
                if (agent.remainingDistance < 4f || !agent.hasPath)
                {
                    agent.speed = 2f;
                    PickRandomLocation();
                }
            }
            else if (bossStateChecker.state == BossState.State.SHOOT)
            {
                anim.SetBool("Run", false);
                anim.SetBool("WakeUp", true);
                anim.SetBool("Walk", false);
                LookPlayer();
                if (currentAttackTime >= attackRate)
                {
                    anim.SetTrigger("Shoot");
                    AudioManager.instance.PlaySfx(0);
                    Instantiate(fireball, firePosition.position, Quaternion.identity);
                    currentAttackTime = 0;
                    finishedAttacking = false;
                }
                else
                {
                    currentAttackTime += Time.deltaTime;
                }
            }
            else if (bossStateChecker.state == BossState.State.ATTACK)
            {
                anim.SetBool("Run", false);
                anim.SetBool("WakeUp", true);
                anim.SetBool("Walk", false);
                
                LookPlayer();

                if (currentAttackTime >= attackRate)
                {
                    int index = Random.Range(1, 3);
                    anim.SetInteger("Attack", index);
                    if (index == 2) // TailAttack seçildiðinde bayraðý ayarlayýn
                    {
                        isTailAttacking = true;
                       
                    }
                    AudioManager.instance.PlaySfx(9);
                    currentAttackTime = 0f;
                    finishedAttacking = false;
                }
                else
                {
                    currentAttackTime += Time.deltaTime;
                    anim.SetInteger("Attack", 0);
                }
            }
            else
            {
                anim.SetBool("WakeUp", false);
                anim.SetBool("Walk", false);
                anim.SetBool("Run", false);
                agent.isStopped = true;
            }
        }
    }

    void PickRandomLocation()
    {
        GameObject pos = GetRandomPoint();
        agent.SetDestination(pos.transform.position);
    }

    private GameObject GetRandomPoint()
    {
        int index = Random.Range(0, allWaypointsList.Count);
        return allWaypointsList[index];
    }

    void LookPlayer()
    {
        Vector3 targetPosition = new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetPosition - transform.position),
            turnSpeed * Time.deltaTime);
    }

    public void OnHit() // Hit animasyonu kontrolü
    {
        if (!isTailAttacking)
        {
            anim.SetTrigger("Hit");
        }
    }
}
