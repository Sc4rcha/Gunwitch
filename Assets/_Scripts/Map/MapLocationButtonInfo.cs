using UnityEngine;

[RequireComponent (typeof (MapLocationButton))]
public class MapLocationButtonInfo : MonoBehaviour
{
    public GameObject LocationNameHolder;
    public TMPro.TMP_Text LocationNameText;
    [Space]
    public SOLocation LocationInfo;


    private ManagerMap manager;

    public void Setup (ManagerMap manager)
    {
        this.manager = manager;
    }


    public void LocationNameShow(bool isShow) 
    {
        LocationNameHolder.SetActive (isShow);
        LocationNameText.text = LocationInfo.Name;
    }
    public void LocationInteract() 
    {
        manager.ButtonLocation(LocationInfo);
    }
}
