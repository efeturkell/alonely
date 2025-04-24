using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Reflection;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [HideInInspector] public float currentHealth;
    Animator anim;
    public float maxHealth = 100f;
    [SerializeField] private Image EnemyHealthBar;
    private SphereCollider targetCollider;
    public int ExpAmount = 10;
    public static event Action<int> onDeath;
    

    private EnemyWaypointTracker enemyWaypointTracker;
    private LevelManager levelManager;
    private Canvas canvas;
    private NavMeshAgent agent;

    
    
    private void Awake()
    {
        targetCollider = GetComponentInChildren<SphereCollider>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        
        levelManager = FindObjectOfType<LevelManager>();
        enemyWaypointTracker = FindObjectOfType<EnemyWaypointTracker>();
        canvas = EnemyHealthBar.gameObject.GetComponentInParent<Canvas>();
        

        if (this.gameObject.tag == "Boss")
        {
            maxHealth = 1000;
        }
        else if (this.gameObject.tag == "Enemy")
        {
            maxHealth = 100;
        }
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        EnemyHealthBar.fillAmount = currentHealth / maxHealth;

        if (currentHealth > 0)
        {
            if (this.gameObject.tag == "Boss")
            {
                AudioManager.instance.PlaySfx(6);
            }
            else if (this.gameObject.tag == "Enemy")
            {
                AudioManager.instance.PlaySfx(3);
            }
            anim.SetTrigger("Hit");
        }
        else if (currentHealth <= 0)
        {
            HandleDeath();

     
        }
    }

    private void HandleDeath()
    {
        onDeath?.Invoke(ExpAmount);

        if (targetCollider.gameObject.activeInHierarchy)
        {
            targetCollider.gameObject.SetActive(false);
        }
        if (canvas.gameObject.activeInHierarchy)
        {
            canvas.gameObject.SetActive(false);
        }

    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5f);
        RespawnEnemy();
    }

    private void RespawnEnemy()
    {
        currentHealth = maxHealth;
        EnemyHealthBar.fillAmount = currentHealth / maxHealth;
       


        if (!targetCollider.gameObject.activeInHierarchy)
        {
            targetCollider.gameObject.SetActive(true);
        }
        if (!canvas.gameObject.activeInHierarchy)
        {
            canvas.gameObject.SetActive(true);
        }
        
    }

    public void DecreaseHealthOnLevelUp()
    {
        if (gameObject.tag == "Enemy")
        {
            currentHealth -= 5f;
            if (currentHealth < 0) currentHealth = 0;

        }
        
 
    }
}
