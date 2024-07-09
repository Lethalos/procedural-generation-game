using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static MapPreview;

public class TerrainMenuUIHandler : MonoBehaviour
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
    [SerializeField] TMP_InputField seedInputField;
    [SerializeField] Button nextButton;
    [SerializeField] Button resetButton;
    [SerializeField] Button exitButton;

    [Header("Terrain Settings")]
    [SerializeField] HeightMapSettings heightMapSettings;
    [SerializeField] MapPreview mapPreview;
    private NoiseSettings noiseSettings;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        noiseSettings = heightMapSettings.noiseSettings;

        // Load settings from PlayerPrefs
        LoadSettings();

        // Initialize UI values
        InitializeUI();

        // Add listeners
        scaleSlider.onValueChanged.AddListener(UpdateScale);
        octavesSlider.onValueChanged.AddListener(UpdateOctaves);
        persistenceSlider.onValueChanged.AddListener(UpdatePersistence);
        lacunaritySlider.onValueChanged.AddListener(UpdateLacunarity);
        heightMultiplierSlider.onValueChanged.AddListener(UpdateHeightMultiplier);
        seedInputField.interactable = true;
        seedInputField.onEndEdit.AddListener(UpdateSeed);
        nextButton.onClick.AddListener(GoToNextScene);
        resetButton.onClick.AddListener(ResetValues);
        exitButton.onClick.AddListener(ExitApplication);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventSystem.current.SetSelectedGameObject(seedInputField.gameObject);
        }
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
        SaveSettings();
    }

    void UpdateOctaves(float value)
    {
        noiseSettings.octaves = (int)value;
        octavesText.text = value.ToString();
        mapPreview.DrawMapInEditor();
        SaveSettings();
    }

    void UpdatePersistence(float value)
    {
        noiseSettings.persistence = value;
        persistenceText.text = value.ToString("F2");
        mapPreview.DrawMapInEditor();
        SaveSettings();
    }

    void UpdateLacunarity(float value)
    {
        noiseSettings.lacunarity = value;
        lacunarityText.text = value.ToString();
        mapPreview.DrawMapInEditor();
        SaveSettings();
    }

    void UpdateHeightMultiplier(float value)
    {
        heightMapSettings.heightMultiplier = value;
        heightMultiplierText.text = value.ToString();
        mapPreview.DrawMapInEditor();
        SaveSettings();
    }

    void UpdateSeed(string value)
    {
        if (int.TryParse(value, out int seed))
        {
            noiseSettings.seed = seed;
            mapPreview.DrawMapInEditor();
            SaveSettings();
        }
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene("BaseMenu");
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
        SaveSettings();
    }

    void ExitApplication()
    {
        // Exit the application
        Application.Quit();
    }

    void SaveSettings()
    {
        PlayerPrefs.SetFloat("Scale", noiseSettings.scale);
        PlayerPrefs.SetInt("Octaves", noiseSettings.octaves);
        PlayerPrefs.SetFloat("Persistence", noiseSettings.persistence);
        PlayerPrefs.SetFloat("Lacunarity", noiseSettings.lacunarity);
        PlayerPrefs.SetInt("Seed", noiseSettings.seed);
        PlayerPrefs.SetFloat("HeightMultiplier", heightMapSettings.heightMultiplier);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Scale"))
        {
            noiseSettings.scale = PlayerPrefs.GetFloat("Scale");
            noiseSettings.octaves = PlayerPrefs.GetInt("Octaves");
            noiseSettings.persistence = PlayerPrefs.GetFloat("Persistence");
            noiseSettings.lacunarity = PlayerPrefs.GetFloat("Lacunarity");
            noiseSettings.seed = PlayerPrefs.GetInt("Seed");
            heightMapSettings.heightMultiplier = PlayerPrefs.GetFloat("HeightMultiplier");
            InitializeUI();
        }
    }
}
