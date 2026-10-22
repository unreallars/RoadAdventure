using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    [SerializeField] private Sprite musicOn, musicOff, soundOn, soundOff;
    [SerializeField] private AudioMixer musicMixer, soundMixer;

    private void Start()
    {
        GameObject.Find("MusicSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume", 1);
        GameObject.Find("SoundSlider").GetComponent<Slider>().value = PlayerPrefs.GetFloat("SoundVolume", 1);
    }
    private void Update()
    {
        if (PlayerPrefs.GetFloat("MusicVolume") != 0.0001f)
        {
            GameObject.Find("MusicHandle").GetComponent<Image>().sprite = musicOn;
        }
        else
        {
            GameObject.Find("MusicHandle").GetComponent<Image>().sprite = musicOff;
        }
        if (PlayerPrefs.GetFloat("SoundVolume") != 0.0001f)
        {
            GameObject.Find("SoundHandle").GetComponent<Image>().sprite = soundOn;
        }
        else
        {
            GameObject.Find("SoundHandle").GetComponent<Image>().sprite = soundOff;
        }
    }
    public void ChangeMusicVolume(float value)
    {
        musicMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public void ChangeSoundVolume(float value)
    {
        soundMixer.SetFloat("Volume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SoundVolume", value);
    }
    public void ChangeName()
    {
        GameObject.Find("Canvas").GetComponent<MainMenu>().ChangeMenu(7);
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
    }
    public void ChangeLanguage()
    {
        GameObject.Find("Canvas").GetComponent<MainMenu>().ChangeMenu(8);
        GameObject.Find("Canvas").transform.GetChild(0).gameObject.SetActive(false);
    }
}