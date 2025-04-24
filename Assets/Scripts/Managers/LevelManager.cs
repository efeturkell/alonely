using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int currentExp;
    private int level;
    private int expToNextLevel;
    public int GetLevel { get { return level + 1; } }
    public static LevelManager instance;
    public Image ExpBar;
    public Text LevelText;
    public Text WelcomeMessage; // WelcomeMessage referans�

    public delegate void LevelUpEvent(int newLevel);
    public static event LevelUpEvent OnLevelUp;

    private EnemyHealth[] enemyHealths;
    public GameObject LevelUpVFX;
    private Transform Player;
    private PlayerSkillDamage playerSkillDamage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        InitializeReferences();
        ResetLevelManager();
        ShowWelcomeMessage(); // Welcome mesaj�n� g�ster
    }

    private void OnEnable()
    {
        EnemyHealth.onDeath += AddExp;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        EnemyHealth.onDeath -= AddExp;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeReferences();
        ResetLevelManager();
        ShowWelcomeMessage(); // Sahne y�klendi�inde welcome mesaj�n� g�ster
    }

    private void InitializeReferences()
    {
        // Ensure ExpBar and LevelText are initialized
        if (ExpBar == null)
        {
            ExpBar = GameObject.Find("ExpBar")?.GetComponent<Image>();
            if (ExpBar == null)
            {
                Debug.LogError("ExpBar could not be found in the scene!");
            }
        }
        if (LevelText == null)
        {
            LevelText = GameObject.Find("LevelText")?.GetComponent<Text>();
            if (LevelText == null)
            {
                Debug.LogError("LevelText could not be found in the scene!");
            }
        }
        if (WelcomeMessage == null)
        {
            WelcomeMessage = GameObject.Find("WelcomeMessage")?.GetComponent<Text>();
            if (WelcomeMessage == null)
            {
                Debug.LogError("WelcomeMessage could not be found in the scene!");
            }
        }

        enemyHealths = FindObjectsOfType<EnemyHealth>();
        Player = GameObject.Find("Player")?.transform;
        playerSkillDamage = FindObjectOfType<PlayerSkillDamage>();

        if (Player == null)
        {
            Debug.LogError("Player could not be found in the scene!");
        }
        ShowWelcomeMessage();
    }

    private void ResetLevelManager()
    {
        level = 0;
        currentExp = 0;
        expToNextLevel = 100;
        if (ExpBar != null)
        {
            ExpBar.fillAmount = 0f;
        }
        UpdateLevelText();
    }

    private void ShowWelcomeMessage()
    {
        if (WelcomeMessage != null)
        {
            WelcomeMessage.text = "K�y�ndeki t�m insanlar �ld�. K�y�n� kurtarmak i�in t�m iskeletleri ve ejderhay� alt etmeyi ba�ar !!";
            Invoke("HideWelcomeMessage", 7f); // Mesaj� 5 saniye sonra gizle
        }
    }

    private void HideWelcomeMessage()
    {
        if (WelcomeMessage != null)
        {
            WelcomeMessage.text = "";
        }
    }

    public void AddExp(int amount)
    {
        currentExp += amount;
        if (ExpBar != null)
        {
            ExpBar.fillAmount = (float)currentExp / expToNextLevel;
        }

        if (currentExp >= expToNextLevel)
        {
            level++;
            AudioManager.instance.PlaySfx(11);

            if (Player != null)
            {
                GameObject LevelUpVFXClone = Instantiate(LevelUpVFX, Player.position, Quaternion.identity);
                LevelUpVFXClone.transform.SetParent(Player);
            }

            UpdateLevelText();
            currentExp -= expToNextLevel;
            if (ExpBar != null)
            {
                ExpBar.fillAmount = (float)currentExp / expToNextLevel;
            }

            if (GetLevel <= 5)
            {
                expToNextLevel = 300 * level;
            }
            else if (GetLevel <= 10)
            {
                expToNextLevel = 370 * level;
            }
            else if (GetLevel <= 20)
            {
                expToNextLevel = 480 * level;
            }
            else
            {
                expToNextLevel = 590 * level;
            }

            if (OnLevelUp != null)
            {
                OnLevelUp(level);
            }

            NotifyEnemiesLevelUp();
        }
    }

    void UpdateLevelText()
    {
        if (LevelText != null)
        {
            LevelText.text = GetLevel.ToString();
        }
    }

    private void NotifyEnemiesLevelUp()
    {
        foreach (EnemyHealth enemy in enemyHealths)
        {
            if (enemy != null)
            {
                enemy.DecreaseHealthOnLevelUp();
            }
        }
    }
}
