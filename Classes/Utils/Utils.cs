using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Classes.Utils
{
    public static class Utils
    {
        public static float GetPercent(float value, float maxValue)
        {
            var percent = value / maxValue;
            
            return ((int)(percent * 4) + 1) * 0.25f;
            //return percent <= .25f ? .25f : percent <= .50f ? .50f : percent <= .75f ? .75f : 1;
        }

        public static void Rotate(Transform rotateTarget, Transform rotateToTarget)
        {
            var diff = rotateToTarget.localPosition - rotateTarget.position;
            diff.Normalize();

            rotateTarget.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg);
        }
        
        public static T ClosestFrom<T>(IEnumerable<T> list, Vector2 target)
            where T : IEntity
        {
            var generables = list as T[] ?? list.ToArray();
            var minValue = Vector2.Distance(target, generables.First().Transform.position);
            var closest = generables.Where(res => Vector2.Distance(target, res.Transform.position) < minValue).ToArray();

            return closest.Length >= 1 ? closest.First() : generables.First();
        }

        public static void Flip(SpriteRenderer spriteRenderer, Vector2 direction, Animator anim,
            bool normalized = true, bool fixd = true)
        {
            var isHorizontal = Mathf.Abs(direction.x) > Mathf.Abs(direction.y);
            var flipX = spriteRenderer.flipX;

            flipX = isHorizontal switch
            {
                false when flipX => false,
                true => direction.x < 0,
                _ => false
            };
            spriteRenderer.flipX = flipX;

            if (normalized)
                direction = new Vector2(
                    direction.x != 0 ? direction.x > 0 ? 1 : -1 : 0,
                    direction.y != 0 ? direction.y > 0 ? 1 : -1 : 0);

            anim.SetFloat(Flags.Horizontal,
                fixd
                    ? isHorizontal 
                        ? direction.x 
                        : 0
                    : direction.x);
            anim.SetFloat(Flags.Vertical,
                fixd
                    ? !isHorizontal 
                        ? direction.y 
                        : 0
                    : direction.y);
        }
    }
}