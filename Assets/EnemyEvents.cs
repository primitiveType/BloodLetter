using UnityEngine;

public class EnemyEvents : MonoBehaviour
{
    [SerializeField] private EnemySounds Sounds;
    [SerializeField] private AudioSource Source;

    public void OnShot()
    {//TODO: this should probably set an animator bool that fires an event
        Source.PlayOneShot(Sounds.HurtClip);
    }

    public void OnStep()
    {
        Source.PlayOneShot(Sounds.StepClip);
    }

    public void OnAttack()
    {
        Source.PlayOneShot(Sounds.AttackClip);
    }
}