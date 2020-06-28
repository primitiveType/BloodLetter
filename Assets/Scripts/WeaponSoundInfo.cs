using UnityEngine;

public class WeaponSoundInfo : MonoBehaviour
{
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip ReloadSound;

    [SerializeField] private AudioSource Source;
    public void OnShoot()
    {
        Source.PlayOneShot(ShootSound);
    }

    public void OnReload()
    {
        Source.PlayOneShot(ReloadSound);
    }
}