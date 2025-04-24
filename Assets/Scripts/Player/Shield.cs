using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{

    PlayerHealth playerHealth;


    public void Awake()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
    }
    public void OnEnable()
    {
        playerHealth.Shielded = true;
      
    }
    public  void OnDisable()
    {
        playerHealth.Shielded = false;
    }

}
