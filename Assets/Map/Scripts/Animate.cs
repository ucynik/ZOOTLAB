using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Unity.VisualScripting;

public class Animate : MonoBehaviour {
    public SkeletonAnimation skeletonAnimation;

    public string idleAnimation = "Idle";
    public string moveBeginAnimation;
    public string moveAnimation = "Move";
    public string moveEndAnimation;
    public string deathAnimation = "Die";
    public string spawnAnimation;
    public string skillAnimation;
    public string phaseStartAnimation;
    public string phaseIdleAnimation;
    public string phaseEndAnimation;
    public int phaseTransitionDuration;

    private bool isMoving = false;
    private bool active = false;
    private bool inactive = false;

    private Vector3 prev;

    void Start() {
        if (!string.IsNullOrEmpty(spawnAnimation)) {
            active = false;
            var enter = skeletonAnimation.AnimationState.SetAnimation(0, spawnAnimation, false);
            enter.Complete += isComplete;
        }
        else {
            active = true;
        }
        prev = transform.position;
    }

    private void isComplete(Spine.TrackEntry tracked) {
        active = true;
    }

    private IEnumerator loopAnimation(System.Action onComplete) {
        yield return new WaitForSeconds(phaseTransitionDuration);
        skeletonAnimation.AnimationState.ClearTrack(1);
        onComplete?.Invoke();
    }

    public void death(System.Action onComplete = null) {
        inactive = true;
        if (!string.IsNullOrEmpty(deathAnimation)) {
            if (skeletonAnimation.AnimationName != deathAnimation) {
                var exit = skeletonAnimation.AnimationState.SetAnimation(0, deathAnimation, false);
                exit.Complete += (Spine.TrackEntry tracked) =>
                    onComplete?.Invoke();
            }
        }
    }

    public void spawn(System.Action onComplete = null) {
        if (!string.IsNullOrEmpty(spawnAnimation)) {
            var enter = skeletonAnimation.AnimationState.SetAnimation(0, spawnAnimation, false);
            enter.Complete += (Spine.TrackEntry track) => {
                inactive = false;
                active = true;
                onComplete?.Invoke();
            };
        }

    }
    
    public void skill(System.Action onComplete = null) {
        if (!string.IsNullOrEmpty(skillAnimation)) {
            var skill = skeletonAnimation.AnimationState.SetAnimation(1, skillAnimation, false);
            skill.Complete += (Spine.TrackEntry tracked) => {
                skeletonAnimation.AnimationState.ClearTrack(1);
                onComplete?.Invoke();
            };
        }
    }
    
    public void phaseStart(System.Action onComplete = null) {
        if (!string.IsNullOrEmpty(phaseStartAnimation)) {
            var skill = skeletonAnimation.AnimationState.SetAnimation(1, phaseStartAnimation, false);
            skill.Complete += (Spine.TrackEntry tracked) => {
                skeletonAnimation.AnimationState.ClearTrack(1);
                onComplete?.Invoke();
            };
        }
    }

    public void phaseIdle(System.Action onComplete = null) {
        if (!string.IsNullOrEmpty(phaseIdleAnimation) && phaseTransitionDuration != 0) {
            var skill = skeletonAnimation.AnimationState.SetAnimation(1, phaseIdleAnimation, false);
            StartCoroutine(loopAnimation(onComplete));
        }
    }

    public void phaseEnd(System.Action onComplete = null) {
        if (!string.IsNullOrEmpty(phaseEndAnimation)) {
            var skill = skeletonAnimation.AnimationState.SetAnimation(1, phaseEndAnimation, false);
            skill.Complete += (Spine.TrackEntry tracked) => {
                skeletonAnimation.AnimationState.ClearTrack(1);
                onComplete?.Invoke();
            };
        }
    }

    private bool startedMoving = false;

    void Update() {
        if (!active || inactive || skeletonAnimation == null || string.IsNullOrEmpty(idleAnimation)) return;

        Vector3 vel = (transform.position - prev) / Time.deltaTime;

        isMoving = vel.magnitude > 0.01f;
        prev = transform.position;

        if (isMoving) {
            if (!startedMoving) {
                startedMoving = true;
                if (!string.IsNullOrEmpty(moveBeginAnimation)) {
                    skeletonAnimation.AnimationState.SetAnimation(0, moveBeginAnimation, false);
                    skeletonAnimation.AnimationState.AddAnimation(0, moveAnimation, true, 0f);
                }
            } else if ((skeletonAnimation.AnimationName != moveAnimation) && startedMoving) {
                skeletonAnimation.AnimationState.SetAnimation(0, moveAnimation, true);

            }
        }
        else {
            if (startedMoving) {
                startedMoving = false;
                if (!string.IsNullOrEmpty(moveEndAnimation)) {
                    skeletonAnimation.AnimationState.SetAnimation(0, moveEndAnimation, false);
                    skeletonAnimation.AnimationState.AddAnimation(0, idleAnimation, true, 0f);
                }
            } else if (skeletonAnimation.AnimationName != idleAnimation) {
                skeletonAnimation.AnimationState.SetAnimation(0, idleAnimation, true);
            }
        }
    }
}
