using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    public RectTransform bar;
    public RectTransform healthBar;
    public RectTransform whiteBar;

    private Canvas canvas;
    private Coroutine whiteBarCoroutine;
    private GameObject enemy;
    private Enemy enemy_properties;
    List<GameObject> toKill;
    bool isDead = false;
    private float oldHP;

    [HideInInspector] public float health;
    public float max_health;
    public float width = 0.6f;

    public void Start() {
        foreach (RectTransform rect in gameObject.GetComponentInChildren<RectTransform>()) {
            switch (rect.name) {
                case "Health Bar": bar = rect; break;
                case "Health": healthBar = rect; break;
                case "WhiteBar": whiteBar = rect; break;
            }
        }
        canvas = GetComponentInChildren<Canvas>();
        canvas.enabled = false;
        health = max_health;
        
        setMaxHP(max_health);
        setHP(health);
        Vector3 pos = bar.transform.position;
        pos.x = transform.position.x;
        bar.transform.position = pos;

        enemy = transform.gameObject;
        enemy_properties = enemy.GetComponent<Enemy>();
        Debug.Log("Added " + gameObject.name);
    }

    public void Update() {
        if (health <= max_health - 1 && !isDead) {
            canvas.enabled = true;
        }
        if (health <= 0 && !isDead) {
            enemy_properties.move_speed = 0f;
            StartCoroutine(onZeroHP());
            isDead = true;
        }
        if (health != oldHP) {
            setHP(health);
            oldHP = health;
        }
    }

    public void setMaxHP(float MAX_HP) {
        max_health = MAX_HP;
    }

    public void setHP(float HEALTH) {
        health = HEALTH;
        float bar_size = (health / max_health) * width;
        healthBar.sizeDelta = new Vector2 (bar_size, healthBar.sizeDelta.y);

        if (whiteBarCoroutine != null) {
            StopCoroutine(whiteBarCoroutine);
        }

        whiteBarCoroutine = StartCoroutine(animateWhiteBar(bar_size, 0.1f));
    }

    IEnumerator onZeroHP() {
        canvas.enabled = false;
        StartCoroutine(enemy_properties.forceKill(0.25f));
        yield return null;
    }

    IEnumerator animateWhiteBar(float target, float delay) {
        yield return new WaitForSeconds(delay);
        float current = whiteBar.sizeDelta.x;
        float duration = 0.3f;
        float t = 0f; // Elapsed Time
        while (t < duration) {
            t += Time.deltaTime;
            float normalizedTime = t / duration;
            float animate = Mathf.SmoothStep(current, target, normalizedTime);
            whiteBar.sizeDelta = new Vector2(animate, 0.05f);
            yield return null;
        }
        whiteBar.sizeDelta = new Vector2(target, 0.05f);
    }
}
