using UnityEngine;

[RequireComponent (typeof (MapLocationButton))]
public class MapLocationButtonInfo : MonoBehaviour
{
    [Header ("References Scene")]
    public GameObject LocationNameHolder;
    public TMPro.TMP_Text LocationNameText;
    [Header ("References Project")]
    public SOLocation LocationInfo;

    public SOLocation.LocationVisibilityType LocationVisibility { get; private set; }

    private ManagerMap manager;

    public void Setup (ManagerMap manager)
    {
        this.manager = manager;

        // set normal visibility by default
        ChangeVisibility(LocationInfo.InitialVisibility);
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

    public void ChangeVisibility(SOLocation.LocationVisibilityType newVisiblity) 
    {
        LocationVisibility = newVisiblity;
    }
}
