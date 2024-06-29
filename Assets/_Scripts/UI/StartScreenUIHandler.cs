using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MapPreview;

public class StartScreenUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Slider scaleSlider;
    [SerializeField] TextMeshProUGUI scaleText;
    [SerializeField] Slider octavesSlider;
    [SerializeField] TextMeshProUGUI octavesText;
    [SerializeField] Slider persistenceSlider;
    [SerializeField] TextMeshProUGUI persistenceText;
    [SerializeField] Slider lacunaritySlider;
    [SerializeField] TextMeshProUGUI lacunarityText;
    [SerializeField] Slider heightMultiplierSlider;
    [SerializeField] TextMeshProUGUI heightMultiplierText;
    [SerializeField] InputField seedInputField;
    [SerializeField] Button nextButton;
    [SerializeField] Button resetButton;
    [SerializeField] Button exitButton;

    [Header("Terrain Settings")]
    [SerializeField] HeightMapSettings heightMapSettings;
    [SerializeField] MapPreview mapPreview;
    private NoiseSettings noiseSettings;

    void Start()
    {
        noiseSettings = heightMapSettings.noiseSettings;

        // Initialize UI values
        InitializeUI();

        // Add listeners
        scaleSlider.onValueChanged.AddListener(UpdateScale);
        octavesSlider.onValueChanged.AddListener(UpdateOctaves);
        persistenceSlider.onValueChanged.AddListener(UpdatePersistence);
        lacunaritySlider.onValueChanged.AddListener(UpdateLacunarity);
        heightMultiplierSlider.onValueChanged.AddListener(UpdateHeightMultiplier);
        seedInputField.onEndEdit.AddListener(UpdateSeed);
        nextButton.onClick.AddListener(GoToNextScene);
        resetButton.onClick.AddListener(ResetValues);
        exitButton.onClick.AddListener(ExitApplication);
    }

    void InitializeUI()
    {
        mapPreview.DrawMapInEditor();
        
        // Set initial values from ScriptableObject
        scaleSlider.value = noiseSettings.scale;
        octavesSlider.value = noiseSettings.octaves;
        persistenceSlider.value = noiseSettings.persistence;
        lacunaritySlider.value = noiseSettings.lacunarity;
        heightMultiplierSlider.value = heightMapSettings.heightMultiplier;
        seedInputField.text = noiseSettings.seed.ToString();

        // Update text labels
        scaleText.text = scaleSlider.value.ToString();
        octavesText.text = octavesSlider.value.ToString();
        persistenceText.text = persistenceSlider.value.ToString("F2");
        lacunarityText.text = lacunaritySlider.value.ToString();
        heightMultiplierText.text = heightMultiplierSlider.value.ToString();
    }

    void UpdateScale(float value)
    {
        noiseSettings.scale = value;
        scaleText.text = value.ToString();
        mapPreview.DrawMapInEditor();
    }

    void UpdateOctaves(float value)
    {
        noiseSettings.octaves = (int)value;
        octavesText.text = value.ToString();
        mapPreview.DrawMapInEditor();
    }

    void UpdatePersistence(float value)
    {
        noiseSettings.persistence = value;
        persistenceText.text = value.ToString("F2");
        mapPreview.DrawMapInEditor();
    }

    void UpdateLacunarity(float value)
    {
        noiseSettings.lacunarity = value;
        lacunarityText.text = value.ToString();
        mapPreview.DrawMapInEditor();
    }

    void UpdateHeightMultiplier(float value)
    {
        heightMapSettings.heightMultiplier = value;
        heightMultiplierText.text = value.ToString();
        mapPreview.DrawMapInEditor();
    }

    void UpdateSeed(string value)
    {
        if (int.TryParse(value, out int seed))
        {
            noiseSettings.seed = seed;
            mapPreview.DrawMapInEditor();
        }
    }

    void GoToNextScene()
    {
        // Load the next scene. Make sure to replace "NextSceneName" with the actual name of the next scene.
        SceneManager.LoadScene("Game");
    }

    void ResetValues()
    {
        // Reset values to default
        noiseSettings.scale = 200f;
        noiseSettings.octaves = 3;
        noiseSettings.persistence = 0.3f;
        noiseSettings.lacunarity = 3.5f;
        noiseSettings.seed = 150;
        heightMapSettings.heightMultiplier = 100f;

        // Update UI elements to reflect default values
        InitializeUI();
        mapPreview.DrawMapInEditor();
    }

    void ExitApplication()
    {
        // Exit the application
        Application.Quit();
    }
}
