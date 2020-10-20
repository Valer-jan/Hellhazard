using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Soul : MonoBehaviour
{
    [HideInInspector] public int SoulCost;

    private void Start()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        HeroController hero = other.GetComponent<HeroController>();
        if (hero != null)
        {
            ProgressManager.Souls += SoulCost;
            Debug.Log(ProgressManager.Souls + " souls you have");
            FindObjectOfType<AudioManager>().PlayEffect("TakeSoul");
            FindObjectOfType<MenuManager>().ChangeSoulsSumUI();
            Destroy(gameObject);
        }
    }
}
