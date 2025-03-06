using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Extensions
{
    public static class Extensions
    {
        public static void ResetMoveTween(this Transform transform, float time = 0.5f)
        {
            transform.DOLocalMove(Vector3.zero, time).SetEase(Ease.Linear);
            transform.DOLocalRotate(Vector3.zero, time).SetEase(Ease.Linear);
        }

        public static void Reset(this Transform transform, Transform parent = null)
        {
            transform.SetParent(parent);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
        }

        public static void Open(this Transform transform)
        {
            transform.gameObject.SetActive(true);
        }

        public static void Close(this Transform transform)
        {
            transform.gameObject.SetActive(false);
        }

        public static void Open(this GameObject obj)
        {
            obj.SetActive(true);
        }

        public static void Close(this GameObject obj)
        {
            obj.SetActive(false);
        }

        public static float GetAnimationClipDuration(this Animator animator, string animationName)
        {
            if (animator == null)
            {
                return 0f;
            }

            var runtimeController = animator.runtimeAnimatorController;
            if (runtimeController == null)
            {
                return 0f;
            }

            // Tüm animasyon clip'lerini al
            var clips = runtimeController.animationClips;

            // Belirli bir animasyon clip'ini bul
            foreach (var clip in clips)
            {
                if (clip != null && clip.name == animationName)
                {
                    return clip.length;
                }
            }

            // Animasyon bulunamazsa uyarý ver
            Debug.LogWarning($"Animation clip with name '{animationName}' not found!");
            return 0f;
        }
    }

}
