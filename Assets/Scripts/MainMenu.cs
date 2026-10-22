using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer, soundMixer;
    [SerializeField] private Sprite buttonGray, buttonGreen, buttonBlue, buttonDarkBlue;
    [SerializeField] private GameObject contributorsButton;
    [SerializeField] private Sprite s0, s1, s2, s3;
    [SerializeField] private InputAction escapeAction;
    private GameObject canvas;
    private GameObject cars;
    private GameObject level;

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
        Application.targetFrameRate = 60;
        musicMixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume", 1)) * 20);
        soundMixer.SetFloat("Volume", Mathf.Log10(PlayerPrefs.GetFloat("SoundVolume", 1)) * 20);
        DontDestroyOnLoad(GameObject.Find("MusicMainMenu"));
        DontDestroyOnLoad(GameObject.Find("MusicWorld"));
        GameObject.Find("MusicWorld").GetComponent<AudioSource>().Stop();

        canvas = GameObject.Find("Canvas");
        cars = GameObject.Find("Cars");
        level = GameObject.Find("Level");

        if (!GameObject.Find("MusicMainMenu").GetComponent<AudioSource>().isPlaying)
        {
            GameObject.Find("MusicMainMenu").GetComponent<AudioSource>().Play();
        }
        RefreshCars();
        RefreshLevels();
        if (PlayerPrefs.GetInt("Language") == 0 || PlayerPrefs.GetString("PlayerName") == "")
        {
            ChangeMenu(6);
            GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            ChangeMenu(0);
        }

    }
    public void RefreshCars()
    {
        for (int i = 0; i < cars.transform.childCount; i++)
        {
            if (PlayerPrefs.GetInt("CarUnlocked_" + i) == 1 || i == 0)
            {
                cars.transform.GetChild(i).Find("Buy").gameObject.SetActive(false);
                if (PlayerPrefs.GetInt("SelectedCar") == i)
                {
                    GameObject.Find("B2").GetComponent<Image>().sprite = cars.transform.GetChild(i).GetComponent<Image>().sprite;

                    cars.transform.GetChild(i).Find("Select").GetComponent<Image>().sprite = buttonDarkBlue;
                    if (PlayerPrefs.GetInt("Language") == 1)
                    {
                        cars.transform.GetChild(i).Find("Select").GetComponentInChildren<TextMeshProUGUI>().text = "Ausgewählt";
                    }
                    else
                    {
                        cars.transform.GetChild(i).Find("Select").GetComponentInChildren<TextMeshProUGUI>().text = "Selected";
                    }
                }
                else
                {
                    cars.transform.GetChild(i).Find("Select").GetComponent<Image>().sprite = buttonGreen;
                    if (PlayerPrefs.GetInt("Language") == 1)
                    {
                        cars.transform.GetChild(i).Find("Select").GetComponentInChildren<TextMeshProUGUI>().text = "Auswählen";
                    }
                    else
                    {
                        cars.transform.GetChild(i).Find("Select").GetComponentInChildren<TextMeshProUGUI>().text = "Select";
                    }
                }
            }
            else
            {
                if (PlayerPrefs.GetInt("Coins") >= int.Parse(cars.transform.GetChild(i).Find("Buy").Find("Text").GetComponent<TextMeshProUGUI>().text))
                {
                    cars.transform.GetChild(i).Find("Buy").GetComponent<Image>().sprite = buttonBlue;
                }
                else
                {
                    cars.transform.GetChild(i).Find("Buy").GetComponent<Image>().sprite = buttonGray;
                }
            }
        }
    }
    public void RefreshLevels()
    {
        Sprite[] sterne = new Sprite[] { s0, s1, s2, s3 };
        for (int i = 0; i < level.transform.childCount; i++)
        {

            if (i == 0 || PlayerPrefs.GetInt("LevelStars_" + (i - 1)) > 0)
            {
                level.transform.GetChild(i).GetComponent<Button>().interactable = true;
            }
            else
            {
                level.transform.GetChild(i).GetComponent<Button>().interactable = false;
            }
            level.transform.GetChild(i).Find("Image").GetComponent<Image>().sprite = sterne[PlayerPrefs.GetInt("LevelStars_" + i)];

        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void Update()
    {
        if (canvas.transform.GetChild(7).gameObject.activeSelf)
        {
            Button confirmButton = GameObject.Find("ConfirmButton").GetComponent<Button>();
            if (GameObject.Find("NameInputfield").GetComponent<TMP_InputField>().text.Trim() != "")
            {
                confirmButton.GetComponent<Image>().sprite = buttonGreen;
                confirmButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                confirmButton.GetComponent<Image>().sprite = buttonGray;
                confirmButton.GetComponent<Button>().interactable = false;
            }
        }
        if (!canvas.transform.GetChild(6).gameObject.activeSelf && !canvas.transform.GetChild(7).gameObject.activeSelf && !canvas.transform.GetChild(8).gameObject.activeSelf)
        {
            GameObject.Find("VersionUI").GetComponent<TextMeshProUGUI>().text = "V." + Application.version;
            GameObject.Find("NameUI").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetString("PlayerName");
            GameObject.Find("CoinsUI").GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Coins").ToString();

            if (PlayerPrefs.GetInt("Language") == 1)
            {
                GameObject.Find("CarsUI").GetComponent<TextMeshProUGUI>().text = "Autos: " + (PlayerPrefs.GetInt("TotalCars") + 1) + "/4";
                GameObject.Find("StarsUI").GetComponent<TextMeshProUGUI>().text = "Sterne: " + PlayerPrefs.GetInt("TotalStars") + "/18";
            }
            else
            {
                GameObject.Find("CarsUI").GetComponent<TextMeshProUGUI>().text = "Cars: " + (PlayerPrefs.GetInt("TotalCars") + 1) + "/4";
                GameObject.Find("StarsUI").GetComponent<TextMeshProUGUI>().text = "Stars: " + PlayerPrefs.GetInt("TotalStars") + "/18";
            }

        }
        if (escapeAction.WasPerformedThisFrame())
        {
            if (canvas.transform.GetChild(0).gameObject.activeSelf)
            {
                if (canvas.transform.GetChild(5).gameObject.activeSelf)
                {
                    ChangeMenu(0);
                }
                else
                {
                    ChangeMenu(5);
                }
            }
            else if (!canvas.transform.GetChild(6).gameObject.activeSelf && !canvas.transform.GetChild(7).gameObject.activeSelf && !canvas.transform.GetChild(8).gameObject.activeSelf)
            {
                ChangeMenu(0);
            }
        }
    }
    public void ChangeMenu(int menuNumber)
    {
        for (int i = 0; i < canvas.transform.childCount; i++)
        {
            if (menuNumber == i)
            {
                canvas.transform.GetChild(i).gameObject.SetActive(true);
            }
            else if (i != 0)
            {
                canvas.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if (menuNumber == 0)
        {
            contributorsButton.SetActive(true);
        }
        else
        {
            contributorsButton.SetActive(false);
        }
    }
    public void SetLanguage(int languageNumber)
    {
        PlayerPrefs.SetInt("Language", languageNumber);
    }
    public void SetName()
    {
        PlayerPrefs.SetString("PlayerName", GameObject.Find("NameInputfield").GetComponent<TMP_InputField>().text);
        GameObject.Find("NameInputfield").GetComponent<TMP_InputField>().text = "";
        ChangeMenu(0);
    }
    public void BuyCar(int carNumber)
    {
        if (PlayerPrefs.GetInt("Coins") >= int.Parse(GameObject.Find("Cars").transform.GetChild(carNumber).Find("Buy").Find("Text").GetComponent<TextMeshProUGUI>().text))
        {
            PlayerPrefs.SetInt("CarUnlocked_" + carNumber, 1);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - int.Parse(GameObject.Find("Cars").transform.GetChild(carNumber).Find("Buy").Find("Text").GetComponent<TextMeshProUGUI>().text));
            PlayerPrefs.SetInt("TotalCars", PlayerPrefs.GetInt("TotalCars") + 1);
            RefreshCars();
        }
    }
    public void SelectCar(int carNumber)
    {
        PlayerPrefs.SetInt("SelectedCar", carNumber);
        RefreshCars();
    }
    public void StartGame(int level)
    {
        SceneManager.LoadScene("World");
        PlayerPrefs.SetInt("CurrentLevel", level);
    }
}