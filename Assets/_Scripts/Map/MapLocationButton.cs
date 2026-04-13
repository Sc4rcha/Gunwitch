using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent (typeof (MapLocationButtonInfo))]
public class MapLocationButton : Button
{
    // Location Button Information
    private MapLocationButtonInfo info;

    protected override void Awake()
    {
        base.Awake();

        // set information reference
        info = GetComponent<MapLocationButtonInfo>();
    }

    // mouse enter and exit button
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        info.LocationNameShow(true);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        info.LocationNameShow(false);
    }

    // mouse interact with button
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        // enter location
        ManagerGameElements.Instance.ManagerOverworld.ButtonLocation(info.LocationInfo);
    }
}
