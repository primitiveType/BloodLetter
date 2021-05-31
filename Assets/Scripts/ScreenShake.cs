using UnityEngine;
using UnityEngine.AddressableAssets;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] private Vector3 m_Trauma;

    private Vector3 Trauma
    {
        get => m_Trauma;
        set => m_Trauma = value;
    }

    public void AddTrauma(Vector3 trauma)
    {
        Trauma = new Vector3(Mathf.Clamp01(trauma.x + Trauma.x), Mathf.Clamp01(trauma.y + Trauma.y),
            Mathf.Clamp01(trauma.z + Trauma.z));
    }

    public void ResetTrauma()
    {
        Trauma = new Vector3();
    }

    public void AddTrauma(float trauma)
    {
        Trauma = new Vector3(Mathf.Clamp01(trauma + Trauma.x), Mathf.Clamp01(trauma + Trauma.y),
            Mathf.Clamp01(trauma + Trauma.z));
    }

    private Vector3 Shake => new Vector3(Trauma.x * Trauma.x, Trauma.y * Trauma.y, Trauma.z * Trauma.z);
    
    public float MaxAngle
    {
        get => SettingsManager.Instance.Settings.ScreenShake * 50;
    }

    [SerializeField] private float m_RecoveryPerSecond = 1f;

    [SerializeField] private Transform m_Camera;
    private Transform Camera => m_Camera;

    private float seed;
    [SerializeField] private float speed = .01f;
    [SerializeField] private float axisOffset = 1f;

    private float totalPitch;
    private float totalYaw;
    private float totalRoll;
    private float totalNoise;
    private int iterations;

    private void Update()
    {
        iterations++;
        seed += speed;
        float yaw = MaxYaw * Shake.x * GetNoise(seed);
        float pitch = MaxPitch * Shake.y * GetNoise(seed + 1);
        float roll = MaxRoll * Shake.z * GetNoise(seed + 2);
        // Debug.Log($"Shake {yaw} , {pitch}, {roll}");

        totalYaw += yaw;
        totalPitch += pitch;
        totalRoll += roll;
        totalNoise += GetNoise(seed);
        // Debug.Log(
        //     $"Averages {totalNoise / (float) iterations}  ----  {totalYaw / (float) iterations},{totalPitch / (float) iterations},{totalRoll / (float) iterations}");
        Camera.localRotation = Quaternion.Euler(yaw, pitch, roll);

        ReduceTrauma();
    }

    private void ReduceTrauma()
    {
        float x = -RecoveryPerSecond * Time.deltaTime;
        float y = -RecoveryPerSecond * Time.deltaTime;
        float z = -RecoveryPerSecond * Time.deltaTime;

        AddTrauma(new Vector3(x, y, z));
    }

    private float GetNoise(float seed)
    {
        return Mathf.PerlinNoise(0, seed) * 2 - 1;
    }

    private float MaxRoll => MaxAngle;

    private float MaxPitch => MaxAngle;

    private float MaxYaw => MaxAngle;

    private float RecoveryPerSecond
    {
        get => m_RecoveryPerSecond;
        set => m_RecoveryPerSecond = value;
    }
}