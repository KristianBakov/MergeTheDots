using UnityEngine;

namespace Utils
{
    public class Utilities : MonoSingleton<Utilities>
    {
        public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
        {
            position.z = camera.nearClipPlane;
            return camera.ScreenToWorldPoint(position);
        }
    }
}
