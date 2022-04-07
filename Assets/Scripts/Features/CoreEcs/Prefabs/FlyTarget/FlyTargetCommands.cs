using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Core
{
    public static class FlyTargetCommands
    {
        public static float Speed = 0.1f;

        public static async Task SimpleFly(Transform transform, Vector2 to)
        {
            var flyDuration = 10 * 0.15f;
            await transform.DOMove(to, flyDuration)
                .SetEase(Ease.InBack)
                .AsyncWaitForCompletion();
        }
        
        public static async Task DefaultFly(Transform transform, Vector2 from, Vector2 to, FlyData data )
        {
            transform.position = from + data.offset;
            
            var firstPos = new Vector2(from.x - 0.212f, from.y - 0.636f);
            await FirstAnimation(transform, firstPos, data);
            await SecondAnimation(transform, firstPos);
            await ThirdAnimation(transform, to, data);
        }

        private static async Task ThirdAnimation(Transform transform, Vector2 to, FlyData data)
        {
            const float THIRD_DURATION = 0.5f;
            var rnd_delay = Random.Range(1, 10) * 0.01f;
            transform.DOMove(to, THIRD_DURATION + rnd_delay)
                .SetEase(Ease.InQuad);

            if (data.is_big_item)
            {
                await transform.DOScale(data.destination_scale, THIRD_DURATION + rnd_delay)
                    .SetEase(Ease.InQuad)
                    .AsyncWaitForCompletion();
            }
            else
            {
                await transform.DOScale(data.destination_scale, 0.25f)
                    .SetEase(Ease.Linear)
                    .SetDelay(THIRD_DURATION + rnd_delay - 0.1f)
                    .AsyncWaitForCompletion();
            }
        }

        private static async Task SecondAnimation(Transform transform, Vector2 firstPos)
        {
            const float SECOND_DURATION = 0.1f;
            float rnd_delay = Random.Range(1, 10) * 0.001f;

            var secondPos = new Vector2(firstPos.x - 0.212f, firstPos.y);
            await transform.DOMove(secondPos, SECOND_DURATION + rnd_delay)
                .SetEase(Ease.OutCubic).AsyncWaitForCompletion();
        }

        private static async Task FirstAnimation(Transform transform, Vector2 firstPos, FlyData data)
        {
            const float FIRST_DURATION = 0.1f;
            transform.DOScale(data.max_scale, FIRST_DURATION)
                .SetEase(Ease.InQuad);
            await transform.DOMove(firstPos, FIRST_DURATION)
                .SetEase(Ease.InCubic)
                .AsyncWaitForCompletion();
        }
    }

    public class FlyData
    {
        public bool is_big_item = false;
        public float max_scale = 1;
        public float normal_scale = 1;
        public float destination_scale = 1;
        public float icon_scale = 1;
        public Vector2 offset = Vector2.zero;

        public string soundPlay;
    }
}