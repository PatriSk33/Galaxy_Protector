using System.Xml.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterSlider;
    public Slider sfxSlider;
    public Slider musicSlider;

    //Labels
    public Text masterText, musicText, sfxText;
    //Trying sound volume
    public AudioSource sfxLoop;

    private void Awake()
    {
        Load();
    }
    private void Start()
    {
        // Audio Mixer
        audioMixer.SetFloat("Master", PlayerPrefs.GetFloat("masterValue"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetFloat("musicValue"));
        audioMixer.SetFloat("SFx", PlayerPrefs.GetFloat("sfxValue"));
        // Labels
        masterText.text = (masterSlider.value + 80).ToString();
        musicText.text = (musicSlider.value + 80).ToString();
        sfxText.text = (sfxSlider.value + 80).ToString();
    }

    public void SetMasterVol()
    {
        audioMixer.SetFloat("Master", masterSlider.value);
        masterText.text = (masterSlider.value + 80).ToString();
        PlayerPrefs.SetFloat("masterValue", masterSlider.value);
    }
    public void SetMusicVol()
    {
        audioMixer.SetFloat("Music", musicSlider.value);
        musicText.text = (musicSlider.value + 80).ToString();
        PlayerPrefs.SetFloat("musicValue", musicSlider.value);
    }
    public void SetSFXVol()
    {
        audioMixer.SetFloat("SFx", sfxSlider.value);
        sfxText.text = (sfxSlider.value + 80).ToString();
        PlayerPrefs.SetFloat("sfxValue", sfxSlider.value);
    }

    public void StartSFXLoop()
    {
        sfxLoop.Play();
    }

    public void StopSFXLoop()
    {
        sfxLoop.Stop();
    }

    void Load()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterValue");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxValue");
        musicSlider.value = PlayerPrefs.GetFloat("musicValue");
        StopSFXLoop();
    }
}
