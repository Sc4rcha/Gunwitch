using UnityEngine;

[RequireComponent (typeof (MapLocationButton))]
public class MapLocationButtonInfo : MonoBehaviour
{
    public GameObject LocationNameHolder;
    public TMPro.TMP_Text LocationNameText;

    public SOLocation LocationInfo;

    public void LocationNameShow(bool isShow) 
    {
        LocationNameHolder.SetActive (isShow);
        LocationNameText.text = LocationInfo.Name;
    }
}
