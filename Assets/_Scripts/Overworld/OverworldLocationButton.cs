using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof (OverworldLocationButtonInfo))]
public class OverworldLocationButton : Button
{
    // Location Button Information
    private OverworldLocationButtonInfo info;

    protected override void Awake()
    {
        base.Awake();

        // set information reference
        info = GetComponent<OverworldLocationButtonInfo>();
    }

    // mouse enter and exit button
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        Debug.Log(info.LocationInfo.Name + " Enter");
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        Debug.Log(info.LocationInfo.Name + " Exit");
    }

    // mouse interact with button
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // enter location
        Manager.Instance.ManagerOverworld.ButtonLocation(info.LocationInfo);
    }
}
