using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Translation : MonoBehaviour
{
    [SerializeField] private bool text;
    [SerializeField][TextArea(3, 10)] private string englishText;
    [SerializeField] private Sprite englishSprite;
    private Sprite germanSprite;
    private string germanText;

    private void Start()
    {
        if (text)
        {
            germanText = gameObject.GetComponent<TextMeshProUGUI>().text;
        }
        else
        {
            germanSprite = gameObject.GetComponent<Image>().sprite;
        }
    }
    private void Update()
    {
        if (text)
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("Language") == 1 ? germanText : englishText;
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = PlayerPrefs.GetInt("Language") == 1 ? germanSprite : englishSprite;
        }
    }
}