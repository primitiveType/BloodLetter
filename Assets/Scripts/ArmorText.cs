using UnityEngine;
using UnityEngine.UI;

public class ArmorText : MonoBehaviour
{
    [SerializeField] private ActorArmor Armor;

    [SerializeField] private PlayerEvents Events;

    [SerializeField] private Text Text;
    [SerializeField] private Image Image;
    [SerializeField] private Image OverhealImage;

    private bool IsDirty { get; set; }

    // Start is called before the first frame update
    private void Start()
    {
        Events.OnArmorChangedEvent += ArmorChanged;
        IsDirty = true;
    }

    private void ArmorChanged(object sender, OnArmorChangedEventArgs onArmorChangedEventArgs)
    {
        IsDirty = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsDirty)
        {
            Text.text = $"{Armor.CurrentArmor} / {Armor.MaxArmor}";
            Image.fillAmount = Armor.CurrentArmor / Armor.MaxArmor;
            float overhealMax = Armor.OverhealMaxArmor - Armor.MaxArmor;
            float overhealAmount = Armor.CurrentArmor - Armor.MaxArmor; 
            OverhealImage.fillAmount = overhealAmount / overhealMax;
        }
    }

    private void OnDestroy()
    {
        Events.OnArmorChangedEvent -= ArmorChanged;
    }
}