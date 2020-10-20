using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Weapon : MonoBehaviour
{
    #region declarations

    public bool ColdWeapon = false;
    public bool AutoFire = true;
    public float Damage = 10f;
    public float MaxDistance = 20f;
    public int Rate = 150;

    [Header("Firing gun")]
    public ParticleSystem FireEffect;
    public ParticleSystem ImpactEffectPrefab;
    public int BulletsCurrent = 10;

    [Header("Cold weapon")]
    public float Cone = 90f;
    public float TimeToPrepairing = .3f;

    Transform cameraMain;
    float TimeBetShorts = 0f, TimeReadyShoot;
    bool BladeFlag = false;
    bool isGround = true;
    SphereCollider trigger;
    PlayerHands hands;
    MenuManager menu;
    PlayerAnimation animPlayer;

    #endregion

    private void Start()
    {
        TimeReadyShoot = 1 / (Rate / 60f); // sec to next short
        cameraMain = Camera.main.transform;

        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;

        menu = FindObjectOfType<MenuManager>();
        animPlayer = FindObjectOfType<PlayerAnimation>();
    }

    private void Update()
    {
        if (hands != null && hands.WeaponInHand == gameObject)
        {
            if (!ColdWeapon) // firing gun
            {
                if (TimeBetShorts < TimeReadyShoot) TimeBetShorts += Time.deltaTime;
                if (BulletsCurrent > 0)
                {
                    if (AutoFire && TimeBetShorts >= TimeReadyShoot && Aiming()) Shoot();
                }
                else Destroy(gameObject); // delete weapon
            }
            else // cold weapon
            {
                if (AutoFire && Aiming() && !BladeFlag) StartCoroutine(Blade());
            }
        }
        else // rotation then on ground
        {
            if (!isGround)
            {
                transform.rotation = Quaternion.identity;
                isGround = true;
            }
            transform.Rotate(0f, 100f * Time.deltaTime, 0f);
        }
    }

    bool Aiming()
    {
        RaycastHit hit;
        if (Physics.Raycast(cameraMain.position, cameraMain.forward, out hit, MaxDistance))
        {
            EnemyBaheviour enemy = hit.transform.GetComponent<EnemyBaheviour>();
            if (enemy != null) return true;
            else return false;
        }
        else return false;
    }

    void Shoot()
    {
        // Debug.Log("Short");
        RaycastHit hit;
        if (Physics.Raycast(cameraMain.position, cameraMain.forward, out hit, MaxDistance))
        {
            // impact
            // Instantiate(ImpactEffect, hit.transform.position, hit.transform.rotation);
            FireEffect.Play();
            animPlayer.CameraAnimator.SetTrigger("Shoot");

            // damage
            EnemyBaheviour enemy = hit.transform.GetComponent<EnemyBaheviour>();
            if (enemy != null) enemy.TakeDamage(Damage);
        }
        BulletsCurrent--;
        menu.ChangePatrons(BulletsCurrent);
        TimeBetShorts = 0f;
    }

    IEnumerator Blade()
    {
        BladeFlag = true;
        yield return new WaitForSeconds(TimeToPrepairing);

        // check cone and damage
        // Debug.Log("KaYaaaa!");
        Collider[] checkSphere = Physics.OverlapSphere(cameraMain.position, MaxDistance);
        foreach (var item in checkSphere)
        {
            Vector3 toTarget = item.transform.position - cameraMain.position;
            EnemyBaheviour enemy = item.GetComponent<EnemyBaheviour>();
            if (Vector3.Angle(cameraMain.forward, toTarget) < Cone && enemy != null) enemy.TakeDamage(Damage);
        }

        // animation
        animPlayer.HandsAnimator.SetTrigger("BladeTrigger");
        animPlayer.CameraAnimator.SetTrigger("Blade");

        yield return new WaitForSeconds(TimeReadyShoot - TimeToPrepairing);
        BladeFlag = false;
    }

    public void ShootButton()
    {
        if (BulletsCurrent > 0 && !AutoFire && TimeBetShorts >= TimeReadyShoot) Shoot();
    }

    private void OnTriggerEnter(Collider other)
    {
        hands = other.GetComponent<PlayerHands>();
        if (hands != null && hands.WeaponInHand != gameObject && hands.canTake)
        {
            StartCoroutine(hands.TakeWeapon(gameObject));
            menu.ChangePatrons(BulletsCurrent);
            isGround = true;
        }
    }
}
