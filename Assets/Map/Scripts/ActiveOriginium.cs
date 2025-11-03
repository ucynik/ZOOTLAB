using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActiveOriginium : MonoBehaviour {
    private float bleed_duration = 9999f;
    public float bleedDamage;

    private void OnTriggerEnter(Collider enemy) {
        if (enemy.GetComponentInChildren<HealthBar>() && !enemy.GetComponent<Enemy>().bleeding) {
            StartCoroutine(bleed(enemy.gameObject));
        }

    }

    IEnumerator bleed(GameObject enemy) {
        HealthBar enemy_properties = enemy.GetComponentInChildren<HealthBar>();
        enemy.GetComponentInChildren<Enemy>().bleeding = true;
        for (int i = 0; i < bleed_duration; i++) {
            enemy_properties.health -= bleedDamage;
            yield return new WaitForSeconds(1f);
        }
        enemy.GetComponentInChildren<Enemy>().bleeding = false;
    }
}
