using UnityEngine;
using UnityEngine.UI;

public class CombatDrum : MonoBehaviour
{
    public Image[] Bullets;
    [Header("Variables")]
    public float RotationSpeed;

    private int magazineSize;
    private RectTransform rectTransform;

    private const float magazineRadius = 80f;
    private float drumtargetAngle;
    private float drumAngle;

    public void Setup(int magazineSize) 
    {
        this.magazineSize = magazineSize;

        rectTransform = GetComponent<RectTransform>();

        // hide all bullets
        foreach (var bullet in Bullets)
            bullet.gameObject.SetActive(false);

        // arrange and show bullets
        var bulletPositions = GetBulletPositions();
        for (int i = 0; i < bulletPositions.Length; i++)
            Bullets[i].GetComponent<RectTransform>().anchoredPosition = bulletPositions[i];
    }


    private void Update()
    {
        // Smooth interpolation toward target angle
        drumAngle = Mathf.LerpAngle(rectTransform.localEulerAngles.z, drumtargetAngle, RotationSpeed * Time.deltaTime);

        // set drum rotation
        rectTransform.localEulerAngles = new Vector3(rectTransform.localEulerAngles.x, rectTransform.localEulerAngles.y, drumAngle);
    }


    public void ReloadStart() 
    {
        // activate drum buttons
    }
    public void ReloadFinish() 
    {
        // some cool effect or something
    }


    public void LoadBullet(int bulletIndex, GameInfo.Bullet bullet) 
    {
        // activate loaded bullet
        Bullets[bulletIndex].gameObject.SetActive(true);
        Bullets[bulletIndex].color = bullet.BulletColor;
    }
    public void UnloadBullet(int bulletIndex) 
    {
        // deactivate bullet unloaded
        Bullets[bulletIndex].gameObject.SetActive(false);
    }
    public void FireBullet(int bulletIndex) 
    {
        // deactivate bullet fired
        Bullets[bulletIndex].gameObject.SetActive(false);
    }
    public void RotateDrum(int bulletIndex) 
    {
        drumtargetAngle = bulletIndex * 360f / magazineSize;
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
}
