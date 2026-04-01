using UnityEngine;
using UnityEngine.UI;

public class CombatDrum : MonoBehaviour
{
    public GameObject[] Bullets;
    [Space]
    public GameObject DrumButtonsHolder;
    public Button Unload;
    public Button LoadDefault;

    private int magazineSize;
    private RectTransform rectTransform;

    private const float magazineRadius = 80f;

    public void Setup(int magazineSize) 
    {
        this.magazineSize = magazineSize;

        rectTransform = GetComponent<RectTransform>();

        // hide all bullets
        foreach (var bullet in Bullets)
            bullet.SetActive(false);

        // arrange and show bullets
        var bulletPositions = GetBulletPositions();
        for (int i = 0; i < bulletPositions.Length; i++)
            Bullets[i].GetComponent<RectTransform>().anchoredPosition = bulletPositions[i];
    }


    public void ReloadStart() 
    {
        // activate drum buttons
        DrumButtonsHolder.SetActive(true);

        // set drum buttons to interactable
        Unload.interactable = true;
        LoadDefault.interactable = true;
    }
    public void ReloadFinish() 
    {
        // some cool effect or something
    }


    public void LoadBullet(int bulletIndex) 
    {
        // unload button interactable when a bullet is loaded
        Unload.interactable = true;
        // load default button interactable when bullet is not last bullet on magazine
        LoadDefault.interactable = bulletIndex < magazineSize - 1;

        // activate loaded bullet
        Bullets[bulletIndex].gameObject.SetActive(true);
    }
    public void UnloadBullet(int bulletIndex) 
    {
        // load default button interactable since there is now a new space
        LoadDefault.interactable = true;
        // deactivate bullet unloaded
        Bullets[bulletIndex].gameObject.SetActive(false);
    }
    public void FireBullet(int bulletIndex) 
    {
        // once player has fired deactivate drum buttons, cannot unload anymore
        DrumButtonsHolder.SetActive(false);

        // deactivate bullet fired
        Bullets[bulletIndex].gameObject.SetActive(false);
    }
    public void RotateDrum(int bulletIndex) 
    {
        rectTransform.localRotation = Quaternion.Euler(0, 0, GetDrumRotation(bulletIndex));
    }

    private Vector2[] GetBulletPositions()
    {
        var positions = new Vector2[magazineSize];

        // if magazine size 1 return centre of drum.
        if (magazineSize == 1)
        {
            positions[0] = Vector2.zero;
            return positions;
        }

        // get angle step and starting angle
        float angleStep = (float)(2 * Mathf.PI / magazineSize);
        float startAngle = (float)(Mathf.PI / 2);

        // get positions
        for (int i = 0; i < magazineSize; i++)
        {
            // get angle
            float angle = startAngle - i * angleStep;
            // get vector2
            positions[i] = new Vector2(magazineRadius * (float)Mathf.Cos(angle), magazineRadius * (float)Mathf.Sin(angle));
        }

        return positions;
    }

    float GetDrumRotation(int holeIndex)
    {
        return holeIndex * 360f / magazineSize;
    }
}
