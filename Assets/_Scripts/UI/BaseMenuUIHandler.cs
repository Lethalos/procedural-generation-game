using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseMenuUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Slider distanceSlider;
    [SerializeField] Button nextButton;
    [SerializeField] Button exitButton;

    private string distanceKey = "BaseBuildDistance";

    void Start()
    {
        // Load the saved value if it exists
        if (PlayerPrefs.HasKey(distanceKey))
        {
            distanceSlider.value = PlayerPrefs.GetInt(distanceKey);
        }

        // Add a listener to the slider to save the value when it changes
        distanceSlider.onValueChanged.AddListener(delegate { SaveDistance(); });

        nextButton.onClick.AddListener(GoToNextScene);
        exitButton.onClick.AddListener(ExitApplication);
    }

    void SaveDistance()
    {
        PlayerPrefs.SetInt(distanceKey, (int)distanceSlider.value);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene("Game");
    }

    void ExitApplication()
    {
        // Exit the application
        Application.Quit();
    }
}
