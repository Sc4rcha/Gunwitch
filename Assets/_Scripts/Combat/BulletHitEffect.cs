using UnityEngine;

public class BulletHitEffect : MonoBehaviour
{
    public bool IsObject;
    [Space]
    public Color ColorBlood;
    public ParticleSystem ParticlesHitHole;
    public ParticleSystem ParticlesHit;
    public SpriteMask DeathMask;
    public ParticleSystem ParticlesDeath;
    public ParticleSystem ParticlesDeathSub;
    public ParticleSystem ParticlesDeathFront;

    public int HitHoleOrderInLayer { get; private set; }

    public void Setup(int enemyOrderInLayer) 
    {
        DeathMask.gameObject.SetActive(false);

        DeathMask.frontSortingOrder = enemyOrderInLayer + 99;
        DeathMask.backSortingOrder = enemyOrderInLayer - 99;

        HitHoleOrderInLayer = enemyOrderInLayer + 50;

        ParticlesHitHole.GetComponent<ParticleSystemRenderer>().sortingOrder = HitHoleOrderInLayer;
        ParticlesHit.GetComponent<ParticleSystemRenderer>().sortingOrder = enemyOrderInLayer + 11;
        ParticlesDeathFront.GetComponent<ParticleSystemRenderer>().sortingOrder = enemyOrderInLayer + 12;

        ParticlesDeath.GetComponent<ParticleSystemRenderer>().sortingOrder = enemyOrderInLayer - 10;
        ParticlesDeathSub.GetComponent<ParticleSystemRenderer>().sortingOrder = enemyOrderInLayer - 11;

        var main = ParticlesHitHole.main;
        main.startColor = ColorBlood;
        main = ParticlesHit.main;
        main.startColor = ColorBlood;
        main = ParticlesDeath.main;
        main.startColor = ColorBlood;
        main = ParticlesDeathSub.main;
        main.startColor = ColorBlood;
        main = ParticlesDeathFront.main;
        main.startColor = ColorBlood;
    }

    public void HitPlace(Vector2 position) 
    {
        // move partice system to mouse position
        ParticlesHitHole.transform.position = new Vector3(position.x, position.y, transform.position.z);
        ParticlesHit.transform.position = new Vector3(position.x, position.y, transform.position.z);
        ParticlesDeath.transform.position = new Vector3(position.x, position.y, transform.position.z);
        ParticlesDeathFront.transform.position = new Vector3(position.x, position.y, transform.position.z) + (Vector3)Random.insideUnitCircle * 0.5f;
        // move mask to mouse position
        DeathMask.transform.position = new Vector3(position.x, position.y, transform.position.z);

    }

    public void Damage() 
    {
        // play particles
        ParticlesHitHole.Play();

        if (!IsObject)
            ParticlesHit.Play();
    }

    public void Die()
    {
        // hide damage particles
        ParticlesHitHole.Clear();
        ParticlesHit.Clear();

        // play death particles
        if (!IsObject)
        {
            ParticlesDeath.Play();
            ParticlesDeathFront.Play();
        }

        // show hole
        DeathMask.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        // disable mask when enemy is deactivated
        DeathMask.gameObject.SetActive(false);
    }
}
