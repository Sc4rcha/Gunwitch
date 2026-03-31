using System.Collections.Generic;
using UnityEngine;

public class CombatDrum : MonoBehaviour
{
    public GameObject[] Bullets;

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


    public void LoadBullet(int bulletIndex) 
    {
        Bullets[bulletIndex].gameObject.SetActive(true);
    }
    public void FireBullet(int bulletIndex) 
    {
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
