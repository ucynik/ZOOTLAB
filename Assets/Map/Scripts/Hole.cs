using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private List<GameObject> enemy_list = new List<GameObject>();
    private float fall_speed = 0f;


    private void OnTriggerEnter(Collider enemy) {
        if (enemy == null) return;
        if (!enemy_list.Contains(enemy.gameObject) && enemy.GetComponentInChildren<Enemy>()) {
            enemy_list.Add(enemy.gameObject);
            Debug.Log("Added " + gameObject.name);


            Enemy enemy_properties = enemy.GetComponentInChildren<Enemy>();
            StartCoroutine(enemy_properties.fadeOut(.25f));
            StartCoroutine(fall(enemy.gameObject));
        }
    }

    private void Update() {
        foreach (GameObject enemy in enemy_list) {
            if (enemy != null) {
                Enemy enemy_properties = enemy.GetComponentInChildren<Enemy>();
                enemy_properties.move_speed = 0f;
                enemy.transform.Translate(Vector3.down * Time.deltaTime * fall_speed);
            }
        }
    }

    IEnumerator fall(GameObject enemy) {
        if (enemy == null) yield break;
        Enemy enemy_properties = enemy.GetComponentInChildren<Enemy>();

        float duration = 10f;
        float t = 0f; // Elapsed Time
        while (t < duration) {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            if (enemy != null) {
                enemy.transform.localScale = Vector3.Lerp(enemy.transform.localScale, Vector3.zero, normalizedTime);
            }
            yield return null;
        }
        if (enemy != null) {
            enemy_list.Remove(enemy);
            Destroy(enemy);
        }
    }
}
