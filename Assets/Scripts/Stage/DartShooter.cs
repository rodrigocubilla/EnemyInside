using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartShooter : MonoBehaviour
{
    private bool canShoot = true;

    [SerializeField] private GameObject dart;
    [SerializeField] private Transform shootPoint;

    public void Shoot()
    {
        if (canShoot)
        {
            canShoot = false;
            Instantiate(dart, shootPoint.position, shootPoint.rotation);
            StartCoroutine(WaitToShoot(1f));
        }
    }

    IEnumerator WaitToShoot(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        canShoot = true;
    }
}
