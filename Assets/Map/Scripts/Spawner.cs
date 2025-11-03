using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Transform spawnPoint;
    public GameObject enemy;
    public bool hui = false;
    public bool ming = false;
    public GameObject huiIcon;
    public GameObject mingIcon;

    public float[] spawnTimes;
    public Waypoint[] waypoints;
    // Start is called before the first frame update
    void Start()
    {   
        StartCoroutine(spawn());
    }
    
    private IEnumerator spawn() {

        float startTime = Time.time;

        for (int i = 0; i < spawnTimes.Length; i++) {
            
            float targetTime = startTime + spawnTimes[i];

            while (Time.time < targetTime) 
            {
                yield return null;
            }

            spawnEnemy(enemy, waypoints, spawnPoint);
        }
    }

    public void spawnEnemy(GameObject enemy, Waypoint[] waypoints, Transform spawnPoint) {
            GameObject instance = Instantiate(enemy, spawnPoint.position, enemy.transform.rotation);
            Enemy points = instance.GetComponent<Enemy>();
            points.spawner = this;
            points.active = true;   
            points.Init(instance);

            if (points != null) {
                points.waypoints = waypoints;
                Debug.Log("Waypoints assigned to enemy.");
            }

            Renderer[] renderers = instance.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) {
                renderer.enabled = true;
            }


            if (huiIcon != null && hui) {
                GameObject huiIcn = Instantiate(huiIcon, instance.transform);
                Vector3 localPos = huiIcn.transform.localPosition;
                localPos.y = 5f;
                huiIcn.transform.localPosition = localPos;
                Debug.Log("Hui assigned to enemy.");
            }

            if (mingIcon != null && ming) {
                GameObject mingIcn = Instantiate(mingIcon, instance.transform);
                Vector3 localPos = mingIcn.transform.localPosition;
                localPos.y = 5f;
                mingIcn.transform.localPosition = localPos;
                Debug.Log("Ming assigned to enemy.");
            }
    }
}
