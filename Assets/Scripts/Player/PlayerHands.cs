using System.Collections;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public GameObject WeaponInHand;
    [Space]
    public Transform RightHand;
    public bool canTake = true;

    private void Start()
    {
        canTake = true;
    }

    public IEnumerator TakeWeapon(GameObject Weapon)
    {
        canTake = false;

        if (WeaponInHand != null) WeaponInHand.transform.parent = null;

        WeaponInHand = Weapon;
        Weapon.transform.parent = RightHand;

        Weapon.transform.position = RightHand.position;
        Weapon.transform.rotation = RightHand.rotation;

        yield return new WaitForSeconds(1f);
        canTake = true;
    }
}
