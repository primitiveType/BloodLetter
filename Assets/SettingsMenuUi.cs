using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUi : MonoBehaviour
{
    [SerializeField] private Slider m_MusicVolumeSlider;

    [SerializeField] private Slider m_SfxVolumeSlider;

    // Start is called before the first frame update
    void Start()
    {
        m_MusicVolumeSlider.value = SettingsManager.Instance.Settings.MusicVolume;
        m_SfxVolumeSlider.value = SettingsManager.Instance.Settings.SfxVolume;
        m_MusicVolumeSlider.onValueChanged.AddListener(MusicVolumeChanged);
        m_SfxVolumeSlider.onValueChanged.AddListener(SfxVolumeChanged);
    }

    private void SfxVolumeChanged(float volume)
    {
        SettingsManager.Instance.SetSfxVolume(volume);
    }

    private void MusicVolumeChanged(float volume)
    {
        SettingsManager.Instance.SetMusicVolume(volume);
    }
}