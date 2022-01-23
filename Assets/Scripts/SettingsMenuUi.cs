using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUi : MonoBehaviour
{
    [SerializeField] private Slider m_MusicVolumeSlider;

    [SerializeField] private Slider m_SfxVolumeSlider;
    [SerializeField] private Slider m_ScreenShakeSlider;
    [SerializeField] private Slider m_SensitivitySlider;
    [SerializeField] private Toggle m_PaintballToggle;

    // Start is called before the first frame update
    void Start()
    {
        m_MusicVolumeSlider.value = SettingsManager.Instance.Settings.MusicVolume;
        m_SfxVolumeSlider.value = SettingsManager.Instance.Settings.SfxVolume;
        m_PaintballToggle.isOn = SettingsManager.Instance.Settings.PaintballMode;
        m_ScreenShakeSlider.value = SettingsManager.Instance.Settings.ScreenShake;
        m_SensitivitySlider.value = SettingsManager.Instance.Settings.Sensitivity;
        m_MusicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        m_SfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
        m_ScreenShakeSlider.onValueChanged.AddListener(ScreenShakeChanged);
        m_SensitivitySlider.onValueChanged.AddListener(SensitivityChanged);
        m_PaintballToggle.onValueChanged.AddListener(PaintballModeChanged);
    }

    private void SensitivityChanged(float arg0)
    {
        SettingsManager.Instance.SetSensitivity(arg0);
    }

    private void PaintballModeChanged(bool arg0)
    {
        SettingsManager.Instance.SetPaintballMode(arg0);
    }

    private void ScreenShakeChanged(float arg0)
    {
        SettingsManager.Instance.SetScreenShake(arg0);
    }

    private void SfxVolumeChanged(float volume)
    {
        SettingsManager.Instance.SetSfxVolume(volume);
    }

    private void MusicVolumeChanged(float volume)
    {
        SettingsManager.Instance.SetMusicVolume(volume);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }
    }
}