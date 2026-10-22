using UnityEngine;
using TMPro;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject[] menu, freeplayParts, freeplayBackgrounds;
    [SerializeField] private AudioSource[] sounds;
    [SerializeField] private Image starUI;
    [SerializeField] private Sprite s1, s2, s3;
    [SerializeField] private InputAction escapeAction;

    private bool playing = true;
    private int gas, brake, phase = 0;
    private float endPosX = 20.48f, timerGas;
    private float[] endPosXBackground = new float[4];
    private Transform levelTransform;
    private Transform carsTransform;
    private CinemachineCamera gameCam;
    private TextMeshProUGUI coinUI;
    private Button nextLevelButton;
    private AudioSource musicMainMenu;
    private AudioSource musicWorld;
    private GameObject donutBox;
    private TextMeshProUGUI reasonUI;
    private TimeManager timeManager;

    private void OnEnable()
    {
        escapeAction.Enable();
    }

    private void OnDisable()
    {
        escapeAction.Disable();
    }

    private void Start()
    {
        levelTransform = GameObject.Find("Level").transform;
        carsTransform = GameObject.Find("Cars").transform;
        gameCam = GameObject.Find("GameCam").GetComponent<CinemachineCamera>();
        coinUI = GameObject.Find("CoinUI").GetComponent<TextMeshProUGUI>();
        nextLevelButton = GameObject.Find("NextLevelButton").GetComponent<Button>();
        musicMainMenu = GameObject.Find("MusicMainMenu").GetComponent<AudioSource>();
        musicWorld = GameObject.Find("MusicWorld").GetComponent<AudioSource>();
        donutBox = GameObject.Find("DonutBox");
        reasonUI = GameObject.Find("ReasonUI").GetComponent<TextMeshProUGUI>();
        int index = PlayerPrefs.GetInt("CurrentLevel") + 1;
        timeManager = levelTransform.GetChild(index).GetComponent<TimeManager>();

        endPosXBackground = new float[freeplayBackgrounds.Length];
        for (int i = 0; i < freeplayBackgrounds.Length; i++)
        {
            endPosXBackground[i] = freeplayBackgrounds[i].transform.position.x;
        }
        InitLevelButton();
        InitLevelUI();
        InitCarsAndCamera();
        InitMusic();

        ChangeMenu(0);

        carsTransform.GetChild(PlayerPrefs.GetInt("SelectedCar"))
            .GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        phase = 1;
    }

    private void Update()
    {
        HandleEscapeInput();
        HandleAudio();
        HandleUI();
        HandleFreeplayBackgrounds();
        HandleFreeplayParts();
    }
    public bool IsPlaying()
    {
        return playing;
    }
    private void InitLevelButton()
    {
        int levelIndex = PlayerPrefs.GetInt("CurrentLevel");
        if (levelIndex >= levelTransform.childCount - 2)
        {
            nextLevelButton.interactable = false;
        }
    }

    private void InitLevelUI()
    {

        int currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        int childCount = levelTransform.childCount;

        for (int i = -1; i < childCount - 1; i++)
        {
            int childIndex = i + 1;
            bool active = currentLevel == i;
            levelTransform.GetChild(childIndex).gameObject.SetActive(active);
        }
    }

    private void InitCarsAndCamera()
    {

        int carIndex = PlayerPrefs.GetInt("SelectedCar");
        int childCount = carsTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject car = carsTransform.GetChild(i).gameObject;
            bool active = i == carIndex;
            car.SetActive(active);

            if (active)
            {
                gameCam.Follow = car.transform;
            }
        }
    }

    private void InitMusic()
    {
        musicMainMenu.Stop();
        if (!musicWorld.isPlaying)
        {
            musicWorld.Play();
        }
    }
    public void ChangeMenu(int pMenünummer)
    {

        for (int i = 0; i < menu.Length; i++)
        {
            menu[i].SetActive(pMenünummer == i);
        }

        if (pMenünummer == 1 || pMenünummer == 2)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        playing = pMenünummer == 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene("World");
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    private void HandleEscapeInput()
    {
        if (!escapeAction.WasPerformedThisFrame()) return;

        if (menu[0].activeSelf)
        {
            ChangeMenu(1);
        }
        else if (menu[1].activeSelf)
        {
            ChangeMenu(0);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && menu[0].activeSelf)
        {
#if !UNITY_EDITOR
            ChangeMenu(1);
#endif
        }
    }

    private void HandleAudio()
    {
        int geschwindigkeit = GetGeschwindigkeit();

        if (geschwindigkeit <= 0 && !sounds[0].isPlaying && phase < 2)
        {
            if (sounds[1].isPlaying)
            {
                sounds[1].Stop();
                if (timerGas > 1.2f)
                {
                    sounds[2].Play();
                }
                else
                {
                    sounds[4].Play();
                }
            }
            else
            {
                sounds[3].Play();
            }

            sounds[0].Play();
            timerGas = 0;
        }
        else if (geschwindigkeit > 0 && !sounds[1].isPlaying && phase == 1)
        {
            sounds[0].Stop();
            sounds[1].Play();
        }
        else if (geschwindigkeit == 0 && phase == 2)
        {
            sounds[0].Stop();
        }

        if (geschwindigkeit > 0)
        {
            timerGas += Time.deltaTime;
        }
    }

    public int GetGeschwindigkeit()
    {
        if (menu[0].activeSelf)
        {
            return gas - brake;
        }
        return 0;
    }

    public void SetGas(int pGas)
    {
        gas = pGas;
    }

    public void SetBrake(int pBremse)
    {
        brake = pBremse;
    }

    private void HandleUI()
    {

        if (menu[0].activeSelf)
        {
            coinUI.text = PlayerPrefs.GetInt("Coins").ToString();

            if (PlayerPrefs.GetInt("CurrentLevel") != -1)
            {
                timeManager.ShowTime();
            }
        }
    }

    public void GameOver(string reason)
    {
        ChangeMenu(2);
        reasonUI.text = reason;
    }

    public void Finish()
    {
        carsTransform.GetChild(PlayerPrefs.GetInt("SelectedCar"))
            .GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        timeManager.Reward();

        if (timeManager.GetSterne() == 1)
        {
            starUI.sprite = s1;
        }
        else if (timeManager.GetSterne() == 2)
        {
            starUI.sprite = s2;
        }
        else if (timeManager.GetSterne() == 3)
        {
            starUI.sprite = s3;
        }

        if (PlayerPrefs.GetInt("CurrentLevel") == 5)
        {
            sounds[5].Play();
            donutBox.SetActive(false);
        }

        ChangeMenu(3);
        phase = 2;
    }

    public void NextLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", PlayerPrefs.GetInt("CurrentLevel") + 1);
        Restart();
    }

    private void HandleFreeplayBackgrounds()
    {
        float followX = gameCam.Follow.position.x;

        for (int i = 0; i < freeplayBackgrounds.Length; i++)
        {
            if (endPosXBackground[i] - followX < 100)
            {
                GameObject inst = Instantiate(
                    freeplayBackgrounds[i],
                    new Vector2(endPosXBackground[i] + 28.8f, 0),
                    Quaternion.identity
                );
                endPosXBackground[i] = inst.transform.position.x;
            }
        }
    }

    private void HandleFreeplayParts()
    {
        if (PlayerPrefs.GetInt("CurrentLevel") != -1) return;

        float followX = gameCam.Follow.position.x;

        if (endPosX - followX < 100)
        {
            GameObject inst = Instantiate(
                freeplayParts[Random.Range(0, freeplayParts.Length)],
                new Vector2(endPosX, 0),
                Quaternion.identity
            );
            Transform endPos = inst.transform.Find("EndPos");
            endPosX = endPos.position.x;
        }
    }
}
