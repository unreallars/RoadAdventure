using UnityEngine;
using TMPro;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private int[] seconds = new int[2];
    [SerializeField] private int[] coins = new int[3];

    private int stars = 1;
    private float time;
    private Rigidbody2D carRb;
    private TextMeshProUGUI timeStarUI;
    private TextMeshProUGUI timeUI;

    private void Start()
    {
        int autoIndex = PlayerPrefs.GetInt("SelectedCar");
        if (autoIndex >= 0 && autoIndex < GameObject.Find("Cars").transform.childCount)
        {
            carRb = GameObject.Find("Cars").transform.GetChild(autoIndex).GetComponent<Rigidbody2D>();
        }
        timeStarUI = GameObject.Find("StarUI").GetComponent<TextMeshProUGUI>();
        timeUI = GameObject.Find("TimeUI").GetComponent<TextMeshProUGUI>();
    }
    public int GetSterne()
    {
        return stars;
    }

    private void Update()
    {
        if (carRb.bodyType == RigidbodyType2D.Dynamic)
        {
            time += Time.deltaTime;
        }

        ShowTime();
    }

    public void Reward()
    {
        if (time < seconds[0])
        {
            stars = 3;
        }
        else if (time < seconds[1])
        {
            stars = 2;
        }
        else
        {
            stars = 1;
        }

        int currentCoins = PlayerPrefs.GetInt("Coins");
        PlayerPrefs.SetInt("Coins", currentCoins + coins[3 - stars]);

        int level = PlayerPrefs.GetInt("CurrentLevel");
        string starKey = "LevelStars_" + level;

        int oldStars = PlayerPrefs.GetInt(starKey);
        if (oldStars < stars)
        {
            int diff = stars - oldStars;
            int total = PlayerPrefs.GetInt("TotalStars");
            PlayerPrefs.SetInt("TotalStars", total + diff);
            PlayerPrefs.SetInt(starKey, stars);
        }
    }

    public void ShowTime()
    {
        bool german = PlayerPrefs.GetInt("Language") == 1;
        if (time <= seconds[0])
        {
            timeStarUI.text = german
                ? "3 Sterne bei " + seconds[0] + "s"
                : "3 stars at " + seconds[0] + "s";
        }
        else if (time <= seconds[1])
        {
            timeStarUI.text = german
                ? "2 Sterne bei " + seconds[1] + "s"
                : "2 stars at " + seconds[1] + "s";
        }
        else
        {
            timeStarUI.text = "";
        }
        float roundedTime = Mathf.Round(time * 10f) / 10f;
        timeUI.text = roundedTime.ToString("0.0") + "s";
    }
}
