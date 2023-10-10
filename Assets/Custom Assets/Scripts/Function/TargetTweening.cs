using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TargetTweening : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //------------------------------ Start is called before the first frame update
    void Start()
    {

    }

    //------------------------------ Update is called once per frame
    void Update()
    {

    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Vector3 startPos, Vector3 lastPos,
        Quaternion startRot, Quaternion lastRot, UnityEvent onCompleted, float duration = 0.7f)
    {
        // Create a sequence of tweens to move and rotate the target_Tf
        Sequence sequence = DOTween.Sequence();

        // Set the initial position and rotation
        target_Tf.position = startPos;
        target_Tf.rotation = startRot;

        // Add a move tween from startPos to lastPos
        sequence.Append(target_Tf.DOMove(lastPos, duration)); // You can adjust the duration

        // Add a rotate tween from startRot to lastRot
        sequence.Join(target_Tf.DORotateQuaternion(lastRot, duration)); // You can adjust the duration

        // Add a callback when the tween is completed
        sequence.OnComplete(() =>
        {
            // Call the UnityEvent when the tween is completed
            onCompleted.Invoke();
        });

        // Start the tween sequence
        sequence.Play();
    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Transform last_Tf,
        UnityEvent onCompleted, float duration = 0.7f)
    {
        Vector3 lastPos = last_Tf.position;
        Quaternion lastRot = last_Tf.rotation;

        // Create a sequence of tweens to move and rotate the target_Tf
        Sequence sequence = DOTween.Sequence();

        // Add a move tween from startPos to lastPos
        sequence.Append(target_Tf.DOMove(lastPos, duration)); // You can adjust the duration

        // Add a rotate tween from startRot to lastRot
        sequence.Join(target_Tf.DORotateQuaternion(lastRot, duration)); // You can adjust the duration

        // Add a callback when the tween is completed
        sequence.OnComplete(() =>
        {
            // Call the UnityEvent when the tween is completed
            onCompleted.Invoke();
        });

        // Start the tween sequence
        sequence.Play();
    }

    //------------------------------
    public static void DoScaleTargetObject(Transform target_Tf, Vector3 lastScale, UnityEvent unityEvent,
        float duration = 1f)
    {
        // Tweening target scale
        target_Tf.DOScale(lastScale, duration)
            .OnComplete(() => unityEvent.Invoke());
    }

}
