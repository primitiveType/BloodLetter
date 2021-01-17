using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LevelReferenceComponent))]
public class SelectLevelOnClick : MonoBehaviour, IPointerDownHandler
{
    private LevelInfo Level { get; set; }

    private void Start()
    {
        Level = GetComponent<LevelReferenceComponent>().Level;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSelectManager.Instance.CurrentSelectedLevel == Level)
        {
            LevelSelectManager.Instance.SelectLevel(null);
        }
        else
        {
            LevelSelectManager.Instance.SelectLevel(Level);
        }
    }
}