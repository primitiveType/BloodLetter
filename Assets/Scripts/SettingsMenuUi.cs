using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUi : MonoBehaviour
{
    [SerializeField] private Slider m_MusicVolumeSlider;

    [SerializeField] private Slider m_SfxVolumeSlider;
    [SerializeField] private Slider m_ScreenShakeSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_MusicVolumeSlider.value = SettingsManager.Instance.Settings.MusicVolume;
        m_SfxVolumeSlider.value = SettingsManager.Instance.Settings.SfxVolume;
        m_ScreenShakeSlider.value = SettingsManager.Instance.Settings.ScreenShake;
        m_MusicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        m_SfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
        m_ScreenShakeSlider.onValueChanged.AddListener(ScreenShakeChanged);
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