using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public Color ColorBlood;
    public ParticleSystem HitEffect;
    public SpriteMask DeadEffect;

    public void Setup(int enemyOrderInLayer) 
    {
        DeadEffect.gameObject.SetActive(false);

        DeadEffect.frontSortingOrder = enemyOrderInLayer + 1;
        DeadEffect.backSortingOrder = enemyOrderInLayer - 1;

        var main = HitEffect.main;
        main.startColor = ColorBlood;
    }

    public void HitPlace(Vector2 position) 
    {
        // move partice system to mouse position
        HitEffect.transform.position = new Vector3(position.x, position.y, transform.position.z);
        // move mask to mouse position
        DeadEffect.transform.position = new Vector3(position.x, position.y, transform.position.z);

    }

    public void Damage() 
    {
        // play particles
        HitEffect.Play();
    }

    public void Die() 
    {
        HitEffect.Clear();
        DeadEffect.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // disable mask when enemy is deactivated
        DeadEffect.gameObject.SetActive(false);
    }
}
