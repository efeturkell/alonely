
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;




public class PlayerSkillCast : MonoBehaviour
{
    
    [Header("Mana Settings")]
    public float totalMana = 100f;
    public float manaRegenSpeed = 2f;
    public Image manaBar;


    [Header("CooldownIcons")]
    public Image[] CooldownIcons;
    [Header("Out of Mana Icons")]
    public Image[] OutofManaIcons;
    [Header("CooldownTimes")]
  
    public float CooldownTime1 = 1f;
    public float CooldownTime2 = 1f;
    public float CooldownTime3 = 1f;
    public float CooldownTime4 = 1f;
    public float CooldownTime5 = 1f;
    public float CooldownTime6 = 1f;
    [Header ("Mana Amounts")]
    public float Skill1ManaAmount = 20f;
    public float Skill2ManaAmount = 20f;
    public float Skill3ManaAmount = 20f;
    public float Skill4ManaAmount = 20f;
    public float Skill5ManaAmount = 20f;
    public float Skill6ManaAmount = 20f;
    [Header("Required Level")]
    public int Skill5 = 1;
    public int Skill1 = 2;
    public int Skill2 = 3;
    public int Skill3 = 4;
    public int Skill4 = 5;
    
    public int Skill6 = 6;










    private bool faded = false;

    private int[] fadeImages = new int[] { 0, 0, 0, 0, 0, 0 };
    private List<float> CooldownTimesList = new List<float>();
    private List<float> ManaAmountList = new List<float>();
    private List<int> LevelList = new List<int>();

    private Animator anim;

    private bool canAttack = true;

    private bool isManaDepleted = false;
    private KeyCode manaDepleteKey = KeyCode.Alpha4;

    private PlayerOnClick playerOnClick;
    private LevelManager levelManager;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerOnClick = GetComponent<PlayerOnClick>();
        manaBar = GameObject.Find("ManaOrb").GetComponent<Image>();
        levelManager = FindObjectOfType<LevelManager>();
    }







    void Start()
    {
       AddList();
    }

    void AddList()
    {
        //CooldownListAdd
        CooldownTimesList.Add(CooldownTime1);
        CooldownTimesList.Add(CooldownTime2);
        CooldownTimesList.Add(CooldownTime3);
        CooldownTimesList.Add(CooldownTime4);
        CooldownTimesList.Add(CooldownTime5);
        CooldownTimesList.Add(CooldownTime6);
        //ManaAmountList
        ManaAmountList.Add(Skill1ManaAmount);
        ManaAmountList.Add(Skill2ManaAmount);
        ManaAmountList.Add(Skill3ManaAmount);
        ManaAmountList.Add(Skill4ManaAmount);
        ManaAmountList.Add(Skill5ManaAmount);
        ManaAmountList.Add(Skill6ManaAmount);
        //Level
        LevelList.Add(Skill1);
        LevelList.Add(Skill2);
        LevelList.Add(Skill3);
        LevelList.Add(Skill4);
        LevelList.Add(Skill5);
        LevelList.Add(Skill6);


    }


    void Update()
    {
        if (Input.GetKeyDown(manaDepleteKey))
        {
            StartCoroutine(DepleteManaForSeconds(0f, 5f));
        }
        if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            canAttack = true;
        }
        else
        {
            canAttack = false;
        }
        if (anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            TurnThePlayer();
        }
        if (totalMana < 100f)
        {
            totalMana += Time.deltaTime * manaRegenSpeed;
            manaBar.fillAmount = totalMana / 100f;
        }
        

        CheckMana();
        CheckToFade();
        CheckInput();
        CheckLevel();




    }
    IEnumerator DepleteManaForSeconds(float targetMana, float duration)
    {
        if (!isManaDepleted)
        {
            isManaDepleted = true;
            float originalMana = Skill1ManaAmount;
            float originalManaRegenSpeed = manaRegenSpeed;

            Skill1ManaAmount = targetMana;
            manaRegenSpeed = 5;

            yield return new WaitForSeconds(duration);

            manaRegenSpeed = originalManaRegenSpeed;
            Skill1ManaAmount = originalMana;
            isManaDepleted = false;
        }


    }



    void CheckInput()
    {
        if (anim.GetInteger("Attack") == 0)
        {
            playerOnClick.FinishedMovement = false;
            if (!anim.IsInTransition(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                playerOnClick.FinishedMovement = true;
            }
        }
        //Skill Input

        if (Input.GetKeyDown(KeyCode.Alpha1) && totalMana >= Skill1ManaAmount && levelManager.GetLevel >= Skill1)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[0] != 1 && canAttack)
            {
                totalMana -= Skill1ManaAmount;
                fadeImages[0] = 1;
                anim.SetInteger("Attack", 1);
                
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && totalMana >= Skill2ManaAmount && levelManager.GetLevel >= Skill2)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[1] != 1 && canAttack)
            {
                totalMana -= Skill2ManaAmount;
                fadeImages[1] = 1;
                anim.SetInteger("Attack", 2);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && totalMana >= Skill3ManaAmount && levelManager.GetLevel >= Skill3)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[2] != 1 && canAttack)
            {
                totalMana -= Skill3ManaAmount;
                fadeImages[2] = 1;
                anim.SetInteger("Attack", 3);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && totalMana >= Skill4ManaAmount && levelManager.GetLevel >= Skill4)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[3] != 1 && canAttack)
            {
                
                totalMana -= Skill4ManaAmount;
                fadeImages[3] = 1;
                anim.SetInteger("Attack", 4);
                


            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && totalMana >= Skill5ManaAmount && levelManager.GetLevel >= Skill5)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[4] != 1 && canAttack)
            {
                totalMana -= Skill5ManaAmount;
                totalMana += 25f;
                fadeImages[4] = 1;
                anim.SetInteger("Attack", 5);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && totalMana >= Skill6ManaAmount && levelManager.GetLevel >= Skill6)
        {
            playerOnClick.TargetPosition = transform.position;
            if (playerOnClick.FinishedMovement && fadeImages[5] != 1 && canAttack)
            {
                totalMana -= Skill6ManaAmount;
                fadeImages[5] = 1;
                anim.SetInteger("Attack", 6);
            }
        }
        else
        {
            anim.SetInteger("Attack", 0);
        }



    }

    void CheckToFade () 
    
    {
        for (int i = 0; i < CooldownIcons.Length; i++)
        {
            if (fadeImages[i] == 1) 
            {
 
                if (FadeAndWait(CooldownIcons[i], CooldownTimesList[i]))
                {
                fadeImages[i] = 0;
                }
            
            }



        }
    
    }
    void CheckMana()
    {
        for (int i = 0; i < OutofManaIcons.Length; i++)
        {
            if (levelManager.GetLevel >= LevelList[i])
            {
                if (totalMana < ManaAmountList[i])
                {
                    OutofManaIcons[i].gameObject.SetActive(true);
                }
                else
                {
                    OutofManaIcons[i].gameObject.SetActive(false);
                }
            }
            
        }
        
        
    }
    void CheckLevel()
    {
        for (int i = 0; i < OutofManaIcons.Length; i++)
        {
            if (levelManager.GetLevel < LevelList[i])
            {
                OutofManaIcons[i].gameObject.SetActive(true);
            }
        }
    }

    bool FadeAndWait (Image fadeImage, float fadeTime)
    {
        faded = false;
        if (fadeImage == null)
        {
            return faded;
        }
        if (!fadeImage.gameObject.activeInHierarchy)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.fillAmount = 1f;
        }
        fadeImage.fillAmount -= fadeTime * Time.deltaTime;

        if (fadeImage.fillAmount <= 0f )
        {
            fadeImage.gameObject.SetActive(false);
            faded = true;
        }
        return faded;
    }
    void TurnThePlayer()
    {
        Vector3 targetPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit))
        {
            targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(targetPos - transform.position),
            playerOnClick.turnSpeed * Time.deltaTime);
    }
    
}
