using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using System.Net;
using System;
using System.Linq;
using UnityEngine.UIElements;
using static UnityEditor.UIElements.ToolbarMenu;



#if UNITY_EDITOR
using UnityEditor;
#endif


[System.Serializable]
public class Waypoint {
    public Transform wpTransform;
    [HideInInspector]
    public Vector3 pos;
    public float stop;
    public bool phase;
    [HideInInspector] public bool shadowDisabled = false;
    public bool stairs;
    public float stairsDuration;

    public string newDeath;
    public string newSpawn;
    public string newIdle;
    public string newMove;
    public string newMoveBegin;
    public string newMoveEnd;
    public string newSkill;
    public string newPhaseStart;
    public string newPhaseIdle;
    public string newPhaseEnd;

    public Waypoint(Waypoint waypoint) {
        wpTransform = waypoint.wpTransform;
        pos = waypoint.pos;
        stop = waypoint.stop;
        phase = waypoint.phase;
        shadowDisabled = waypoint.shadowDisabled;
        stairs = waypoint.stairs;
        stairsDuration = waypoint.stairsDuration;

    }

    public Waypoint(Transform transform) {
        wpTransform = transform;
        pos = transform.position;
    }

    public bool isPhase() {
        return phase;
    }
}
public class Enemy : MonoBehaviour {

    [HideInInspector] public Waypoint[] waypoints;
    [HideInInspector] public float move_speed = 1.0f;
    [HideInInspector] public bool isFast;
    [HideInInspector] public float max_speed;
    [HideInInspector] public bool active = false;
    [HideInInspector] public bool spawnsMinions = false;
    [HideInInspector] public GameObject[] minions;
    [HideInInspector] public float minionSpawnInterval = 1;
    [HideInInspector] public float initialCooldown = 3;
    [HideInInspector] public float spawnDelay = 3;
    [HideInInspector] public Spawner spawner;
    [HideInInspector] public bool bleeding = false;
    
    private int index = 0;
    private bool isMoving = true;
    private Renderer rend;
    private Projector shadow;
    private GameObject headIcon;
    private ParticleSystem[] effect;
    private SkeletonAnimation anim;
    private Canvas healthBar;
    private Canvas[] icons;
    private GameObject enemyObj;
    private Animate enemyAnimation;
    private Flip flip;

    void Start() {
        anim = GetComponent<SkeletonAnimation>();
        shadow = GetComponentInChildren<Projector>();
        rend = GetComponent<Renderer>();
        effect = GetComponentsInChildren<ParticleSystem>();
        flip = GetComponentInChildren<Flip>();

        if (GetComponentInChildren<Canvas>()) {
            healthBar = GetComponentInChildren<Canvas>();
        }

        if (isFast) {
            anim.timeScale = 0.125f;
        }

        if (this.transform.Find("Icon") != false) {
            headIcon = this.transform.Find("Icon").gameObject;
        }

        anim.skeleton.SetColor(Color.black);
        StartCoroutine(fadeIn(1f));

        if (!rend.enabled) {
            shadow.enabled = false;
            if (effect != null) {
                foreach (var system in effect) {
                    var emission = system.emission;
                    emission.enabled = false;
                    system.Stop();
                }
            }
        }
        else {
            shadow.enabled = true;
            if (effect != null) {
                foreach (var system in effect) {
                    var emission = system.emission;
                    emission.enabled = true;
                    system.Play();
                }
            }
        }
        if (waypoints.Length > 0) {
            StartCoroutine(moveTo());
        }
    }

    public void Init(GameObject enemy) {
        enemyObj = enemy;
        enemyAnimation = enemy.GetComponent<Animate>();
    }

    private IEnumerator moveTo() {
        shadow.enabled = true;

        if (isFast) StartCoroutine(accelerate());
        if (spawnsMinions) StartCoroutine(spawnMinion());
        while (isMoving) {
            if (index < waypoints.Length) {
                Waypoint WP = waypoints[index];
                transform.position = Vector3.MoveTowards(transform.position, WP.wpTransform.position, move_speed * Time.deltaTime);

                if (transform.position == WP.wpTransform.position) {

                    if (WP.stairs) {
                        yield return StartCoroutine(processStairs(WP, index));
                    }
                    shadow.enabled = !WP.shadowDisabled;
                    updateAnimations(WP);
                    if (waypoints[index].phase) {
                        if (!string.IsNullOrEmpty(enemyAnimation.phaseStartAnimation)
                            && string.IsNullOrEmpty(enemyAnimation.phaseIdleAnimation)) {
                            if (anim.skeleton.GetColor().a > 0) {
                                enemyAnimation.phaseStart();
                                yield return new WaitForSeconds(enemyAnimation.skeletonAnimation.Skeleton.Data
                                    .FindAnimation(enemyAnimation.phaseStartAnimation).Duration);
                            }
                        }
                        else if (!string.IsNullOrEmpty(enemyAnimation.phaseStartAnimation)
                            && !string.IsNullOrEmpty(enemyAnimation.phaseIdleAnimation)) {
                            if (anim.skeleton.GetColor().a > 0) {
                                enemyAnimation.phaseStart();
                                yield return new WaitForSeconds(enemyAnimation.skeletonAnimation.Skeleton.Data
                                    .FindAnimation(enemyAnimation.phaseStartAnimation).Duration);
                                enemyAnimation.phaseIdle();
                                yield return new WaitForSeconds(enemyAnimation.phaseTransitionDuration);
                                enemyAnimation.phaseEnd();
                                yield return new WaitForSeconds(enemyAnimation.skeletonAnimation.Skeleton.Data
                                    .FindAnimation(enemyAnimation.phaseEndAnimation).Duration);
                            }
                        }
                    }

                    yield return new WaitForSeconds(WP.stop);

                    if (!string.IsNullOrEmpty(enemyAnimation.spawnAnimation) && index > 0 && WP.phase) {
                        enemyAnimation.spawn();
                        yield return new WaitForSeconds(enemyAnimation.skeletonAnimation.Skeleton.Data.FindAnimation(enemyAnimation.spawnAnimation).Duration);
                    }

                    index++;

                    if (!string.IsNullOrEmpty(WP.newDeath)) {
                        enemyAnimation.deathAnimation = WP.newDeath;
                    }
                }
            }
            else if (index >= waypoints.Length) {
                yield return StartCoroutine(fadeOut(0.25f));
                move_speed = 0;
                isMoving = false;
                Destroy(enemyObj);
            }
            yield return null;
        }

    }

    public IEnumerator processStairs(Waypoint WP, int index) {
        bool wasActive = active;
        yield return StartCoroutine(fadeOut(0.25f));
        transform.position = waypoints[index + 1].wpTransform.position;
        flip.pause = true;
        yield return new WaitForSeconds(WP.stairsDuration);
        StartCoroutine(fadeIn(0.25f));
        flip.pause = true;
        active = wasActive;
    }

    public void updateAnimations(Waypoint WP) {
        enemyAnimation.spawnAnimation = !string.IsNullOrEmpty(WP.newSpawn) ? WP.newSpawn : enemyAnimation.spawnAnimation;
        enemyAnimation.deathAnimation = !string.IsNullOrEmpty(WP.newDeath) ? WP.newDeath : enemyAnimation.deathAnimation;
        enemyAnimation.idleAnimation = !string.IsNullOrEmpty(WP.newIdle) ? WP.newIdle : enemyAnimation.idleAnimation;
        enemyAnimation.moveAnimation = !string.IsNullOrEmpty(WP.newMove) ? WP.newMove : enemyAnimation.moveAnimation;
        enemyAnimation.moveBeginAnimation = !string.IsNullOrEmpty(WP.newMoveBegin) ? WP.newMoveBegin : enemyAnimation.moveBeginAnimation;
        enemyAnimation.moveEndAnimation = !string.IsNullOrEmpty(WP.newMoveEnd) ? WP.newMoveEnd : enemyAnimation.moveEndAnimation;
        enemyAnimation.skillAnimation = !string.IsNullOrEmpty(WP.newSkill) ? WP.newSkill : enemyAnimation.skillAnimation;
        enemyAnimation.phaseStartAnimation = !string.IsNullOrEmpty(WP.newPhaseStart) ? WP.newPhaseStart : enemyAnimation.phaseStartAnimation;
        enemyAnimation.phaseIdleAnimation = !string.IsNullOrEmpty(WP.newPhaseIdle) ? WP.newPhaseIdle : enemyAnimation.phaseIdleAnimation;
        enemyAnimation.phaseEndAnimation = !string.IsNullOrEmpty(WP.newPhaseEnd) ? WP.newPhaseEnd : enemyAnimation.phaseEndAnimation;
    }

    private IEnumerator spawnMinion() {
        yield return new WaitForSeconds(initialCooldown);
        while (spawnsMinions && active) {
            float save_speed = move_speed;
            if (!string.IsNullOrEmpty(enemyAnimation.skillAnimation)) {
                Debug.Log(enemyAnimation.skillAnimation);
                move_speed = 0;
                enemyAnimation.skill();
                yield return new WaitForSeconds(spawnDelay);
            }
            foreach (GameObject minion in minions) {
                Waypoint spawnerEnemyPos = new Waypoint(transform);
                spawnerEnemyPos.pos -= new Vector3(0f, 0f, 1.5f);
                Waypoint[] remaining = waypoints.Skip(index).Where(wp => wp != null).ToArray();
                Waypoint[] tempPath = (new Waypoint[] { new Waypoint(spawnerEnemyPos) }).Concat(remaining.Select(wp => new Waypoint(wp))).ToArray();

                foreach (Waypoint waypoint in tempPath) {
                    waypoint.stop = 0.0f;
                }

                spawner.spawnEnemy(minion, tempPath, transform);
            }
            move_speed = save_speed;
            yield return new WaitForSeconds(minionSpawnInterval);
        }
    }

    public IEnumerator forceKill(float duration) {
        Debug.Log(enemyObj.name);

        active = false;

        if (healthBar != null) {
            healthBar.enabled = false;
        }
        StartCoroutine(fadeOut(duration));
        enemyAnimation.death();
        yield return new WaitForSeconds(enemyAnimation.skeletonAnimation.Skeleton.Data.FindAnimation("Die").Duration);
        Destroy(enemyObj);
    }

    public IEnumerator fadeOut(float duration) {
        active = false;
        //Debug.Log(enemyObj.name);
        Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
        foreach (Canvas c in canvases) {
            c.gameObject.SetActive(false);
        }

        if (shadow != null) {
            shadow.enabled = false;
            shadow.gameObject.SetActive(false);
        }

        yield return StartCoroutine(fadeBlack(duration));
        yield return StartCoroutine(fadeTrans(duration));

        if (headIcon != null) headIcon.SetActive(false);

        if (effect != null) {
            foreach (var system in effect) {
                var emission = system.emission;
                emission.enabled = false;
                system.Stop();
            }
        }
        yield return null;
    }

    public IEnumerator fadeBlack(float duration) {
        float t = 0f; // Elapsed Time
        while (t < duration) {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            Color fadeColor = Color.Lerp(Color.white, Color.black, normalizedTime);
            anim.skeleton.SetColor(fadeColor);
            yield return null;
        }
    }

    public IEnumerator fadeTrans(float duration) {
        float t = 0f; // Elapsed Time
        while (t < duration) {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            float opacity = Mathf.Lerp(1, 0, normalizedTime);
            Color fadeColor = anim.skeleton.GetColor();
            anim.skeleton.SetColor(new Color(fadeColor.r, fadeColor.g, fadeColor.b, opacity));
            yield return null;
        }
    }

    private IEnumerator fadeIn(float duration) {
        Canvas[] canvases = GetComponentsInChildren<Canvas>(true);
        foreach (Canvas c in canvases) {
            c.gameObject.SetActive(true);
        }
        float t = 0f; // Elapsed Time
        while (t < duration) {
            t += Time.deltaTime;
            float normalizedTime = t / duration;

            anim.skeleton.A = 0.5f;
            Color fadeColor = Color.Lerp(Color.black, Color.white, normalizedTime);
            anim.skeleton.SetColor(fadeColor);
            shadow.enabled = true;
            shadow.gameObject.SetActive(true);
            yield return null;
        }
    }

    private IEnumerator accelerate() {
        while (move_speed < max_speed) {
            yield return new WaitForSeconds(0.5f);
            move_speed += 0.125f * 0.5f;
            anim.timeScale += 0.125f * 0.5f;
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
class MoveEditor : Editor {

    SerializedProperty spawnsMinions;
    SerializedProperty isFast;
    SerializedProperty move_speed;

    SerializedProperty minionsList;
    SerializedProperty minionSpawnInterval;
    SerializedProperty maxSpeed;
    SerializedProperty initialCooldown;
    SerializedProperty spawnDelay;

    private void OnEnable() {
        spawnsMinions = serializedObject.FindProperty("spawnsMinions");
        minionsList = serializedObject.FindProperty("minions");
        minionSpawnInterval = serializedObject.FindProperty("minionSpawnInterval");
        initialCooldown = serializedObject.FindProperty("initialCooldown");
        spawnDelay = serializedObject.FindProperty("spawnDelay");

        isFast = serializedObject.FindProperty("isFast");
        maxSpeed = serializedObject.FindProperty("max_speed");
        move_speed = serializedObject.FindProperty("move_speed");
    }

    public override void OnInspectorGUI() {
        var move = (Enemy)target;
        if (move == null) return;

        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.PropertyField(move_speed, new GUIContent("MSPD / 2"));

        EditorGUILayout.PropertyField(isFast);
        if (isFast.boolValue) {
            EditorGUILayout.PropertyField(maxSpeed, new GUIContent("Max MSPD"), true);
        }

        EditorGUILayout.PropertyField(spawnsMinions);
        if (spawnsMinions.boolValue) {
            EditorGUILayout.PropertyField(minionsList, new GUIContent("Minions"), true);
            EditorGUILayout.PropertyField(minionSpawnInterval, new GUIContent("Minion Spawn Interval"));
            EditorGUILayout.PropertyField(initialCooldown, new GUIContent("Initial Cooldown"));
            EditorGUILayout.PropertyField(spawnDelay, new GUIContent("Spawn Delay"));
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif