using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class MenuManager : MonoBehaviour
{
    #region declarations
    [Header("Settings objects")]
    public Dropdown MoveDrop;
    public Dropdown ViewDrop;
    public Slider Sensetivity, Effects, Sound;
    [Header("Async load")]
    public GameObject Background;
    public Slider ProgressBar;
    [Header("Main menu objects")]
    public GameObject LevelsEmpty;
    public Slider LevelsSlider;
    [SerializeField] public LevelCost[] LevelButtonsPlay;
    [Header("UI objects")]
    [SerializeField] public Stick[] Sticks;
    public Text TextSouls, TextHealth, TextPatrons;
    
    #endregion

    private void Start()
    {
        // check settings prefs
        if (PlayerPrefs.HasKey("Lvl_1"))
        {
            LoadSettings();
            ProgressManager.LoadProgress();
        }
        else
        {
            DefoultSettings();
            ProgressManager.DefoultProgress(LevelButtonsPlay);
        }

        // set buttons to play
        foreach (var item in LevelButtonsPlay) item.text = item.button.GetComponentInChildren<Text>();

        ProgressManager.LoadProgress(LevelButtonsPlay);
        LoadPlayButtons();
        ChangeSoulsSumUI();

        if (Advertisement.isSupported) Advertisement.Initialize("3861673", false);
    }

    // scroll to view levels
    public void LevelSlider()
    {
        LevelsEmpty.transform.localPosition = new Vector3(LevelsSlider.value, LevelsEmpty.transform.localPosition.y, 0);
    }

    #region settings voids
    public void DefoultSettings()
    {
        Debug.LogWarning("Defoult settings");

        PlayerPrefs.SetInt("Sets_Move", 0);
        PlayerPrefs.SetInt("Sets_View", 0);
        PlayerPrefs.SetFloat("Sets_InterfaceSize", 1f);
        PlayerPrefs.SetFloat("Sets_Effects", 1f);
        PlayerPrefs.SetFloat("Sets_Sound", 1f);

        LoadSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("Sets_Move", MoveDrop.value);
        PlayerPrefs.SetInt("Sets_View", ViewDrop.value);
        PlayerPrefs.SetFloat("Sets_Sensetivity", Sensetivity.value);
        PlayerPrefs.SetFloat("Sets_Effects", Effects.value);
        PlayerPrefs.SetFloat("Sets_Sound", Sound.value);
        Debug.Log("settings saved");

        LoadSettings();
    }

    public void LoadSettings()
    {
        // prefs
        MoveDrop.value = PlayerPrefs.GetInt("Sets_Move");
        ViewDrop.value = PlayerPrefs.GetInt("Sets_View");
        Sensetivity.value = PlayerPrefs.GetFloat("Sets_Sensetivity");
        Effects.value = PlayerPrefs.GetFloat("Sets_Effects");
        Sound.value = PlayerPrefs.GetFloat("Sets_Sound");

        // audio mixer
        FindObjectOfType<AudioManager>().SetVolume(Effects.value, Sound.value);

        // sensetivity
        HeroController hero = FindObjectOfType<HeroController>();
        if (hero != null) hero.Sensitivity = Sensetivity.value;

        // Sticks
        if (Sticks.Length > 0)
        {
            // move stick
            switch (MoveDrop.value)
            {
                case 0:
                    Sticks[0].JoystickScript.SetMode(JoystickType.Fixed);
                    break;
                case 1:
                    Sticks[0].JoystickScript.SetMode(JoystickType.Floating);
                    break;
                case 2:
                    Sticks[0].JoystickScript.SetMode(JoystickType.Dynamic);
                    break;
                case 3:
                    Sticks[0].JoystickScript.gameObject.SetActive(false);
                    break;
                default:
                    Sticks[0].JoystickScript.SetMode(JoystickType.Dynamic);
                    break;
            }

            // view stick
            Sticks[1].JoystickScript.gameObject.SetActive(true);
            switch (ViewDrop.value)
            {
                case 0:
                    Sticks[1].JoystickScript.SetMode(JoystickType.Fixed);
                    break;
                case 1:
                    Sticks[1].JoystickScript.SetMode(JoystickType.Floating);
                    break;
                case 2:
                    Sticks[1].JoystickScript.SetMode(JoystickType.Dynamic);
                    break;
                case 3:
                    Sticks[1].JoystickScript.gameObject.SetActive(false);
                    break;
                default:
                    Sticks[1].JoystickScript.SetMode(JoystickType.Dynamic);
                    break;
            }

            // blyat positon
            foreach (var item in Sticks)
            {
                item.JoyRect.sizeDelta = new Vector2(Screen.width / 2, Screen.height);
                item.StickImage.anchoredPosition = item.LocalPosition;
            }
        }

        Debug.Log("settings loaded");
    }
    #endregion

    #region progress voids
    public void ResetProgress()
    {
        ProgressManager.DefoultProgress(LevelButtonsPlay);
    }

    public void SaveProgress()
    {
        ProgressManager.SaveProgress();
    }
    #endregion

    #region load scenes
    public void OnLevelClick(Button button)
    {
        string text = button.GetComponentInChildren<Text>().text;

        foreach (var item in LevelButtonsPlay)
        {
            if (button == item.button)
            {
                if (text == "Play") StartCoroutine(LoadSceneAsync(item.ArenaName));
                else if (ProgressManager.Souls >= item.Cost)
                {
                    ProgressManager.Souls -= item.Cost;
                    button.GetComponentInChildren<Text>().text = "Play";
                    item.IsUnlocked = true;
                    ProgressManager.SaveProgress(LevelButtonsPlay);
                    ChangeSoulsSumUI();
                }
                else Debug.Log("no money, no honey");
            }
        }
        
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    IEnumerator LoadSceneAsync(string SceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        if (Background != null)
        {
            Background.SetActive(true);
            while (!operation.isDone)
            {
                float per = Mathf.Clamp01(operation.progress / .9f);
                ProgressBar.value = per;
                yield return null;
            }
        }
        yield return null;
    }
    #endregion

    public void Ads(int souls)
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show("video");
            ProgressManager.Souls += souls;
            ProgressManager.SaveProgress();
            Debug.Log(souls + " collected");
            ChangeSoulsSumUI();
        }
    }

    public void LoadPlayButtons()
    {
        if (LevelButtonsPlay.Length != 0)
        {
            foreach (var item in LevelButtonsPlay)
            {
                if (item.IsUnlocked) item.text.text = "Play";
                else item.text.text = item.Cost.ToString() + " souls";
            }
        }
    }

    #region UI text
    public void ChangeSoulsSumUI()
    {
        TextSouls.text = "souls " + ProgressManager.Souls.ToString();
    }

    public void ChangePatrons(int patrons)
    {
        TextPatrons.text = "P " + patrons.ToString();
    }

    public void ChangeHealth(float current_health)
    {
        TextHealth.text = "HP " + current_health.ToString();
    }
    #endregion

    public void ExitApp()
    {
        Application.Quit();
    }
}

[Serializable]
public class LevelCost
{
    public Button button;
    public string ArenaName;
    [HideInInspector]
    public Text text;
    [HideInInspector]
    public bool IsUnlocked;
    public int Cost = 100;
}

[Serializable]
public class Stick
{
    public VariableJoystick JoystickScript;
    public Vector2 LocalPosition;
    public RectTransform JoyRect, StickImage;
}
