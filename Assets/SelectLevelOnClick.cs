using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LevelReferenceComponent))]
public class SelectLevelOnClick : MonoBehaviour, IPointerDownHandler
{
    private string LevelName { get; set; }

    private void Start()
    {
        LevelName = GetComponent<LevelReferenceComponent>().LevelName;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelSelectManager.Instance.CurrentSelectedLevel == LevelName)
        {
            LevelSelectManager.Instance.SelectLevel("");
        }
        else
        {
            LevelSelectManager.Instance.SelectLevel(LevelName);
        }
    }
}