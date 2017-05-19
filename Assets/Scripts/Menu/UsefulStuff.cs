using UnityEngine;

namespace Menu
{
    static class UsefulStuff
    {
        /// <summary>
        /// In percent starting from center x=-100 y=-100 is bottom left
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        static public Vector2 CalculateScreenPosition(Vector2 where) 
        {
            var d = Mathf.Abs(Camera.main.transform.position.z);
            var frustumh = (2.0f * d * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));// * Camera.main.aspect;
            Vector3 pos = new Vector2(
                frustumh * Camera.main.aspect * where.x / 200,
                frustumh * where.y / 200
                );
            return pos;
        }

        static public float GetScreenWidth()
        {
            var d = Mathf.Abs(Camera.main.transform.position.z);
            return (2.0f * d * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad)) * Camera.main.aspect;
        }

        static public float GetScreenHeight()
        {
            var d = Mathf.Abs(Camera.main.transform.position.z);
            return (2.0f * d * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad));
        }

    }
}
