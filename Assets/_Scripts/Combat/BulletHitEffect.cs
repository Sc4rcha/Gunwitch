using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public ParticleSystem HitEffect;
    public SpriteMask DeadEffect;

    private bool isHitPlaced;

    public void SetMaskRange(int enemyOrderInLayer) 
    {
        DeadEffect.frontSortingOrder = enemyOrderInLayer + 1;
        DeadEffect.backSortingOrder = enemyOrderInLayer - 1;
    }


    public void HitPlace(Vector2 position) 
    {
        isHitPlaced = true;

        // move partice system to mouse position
        HitEffect.transform.position = new Vector3(position.x, position.y, transform.position.z);
        // move mask to mouse position
        DeadEffect.transform.position = new Vector3(position.x, position.y, transform.position.z);

    }

    public void Damage(bool isDead) 
    {
        // do nothing if Hit placed was not called
        if (!isHitPlaced)
            return;

        if (!isDead)
            // play particle system
            HitEffect.Play();
        else
            // show mask
            DeadEffect.gameObject.SetActive(true);

        isHitPlaced = false;
    }

    private void OnDisable()
    {
        // disable mask when enemy is deactivated
        DeadEffect.gameObject.SetActive(false);
    }
}
