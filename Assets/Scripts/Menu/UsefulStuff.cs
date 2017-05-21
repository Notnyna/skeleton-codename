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

        static public Vector2 MouseToPointVector(Vector2 fromPoint)
        {
            Vector2 MPx = Input.mousePosition; 
            Vector2 MP = Camera.main.ScreenToWorldPoint(new Vector3(MPx.x,MPx.y,Mathf.Abs(Camera.main.transform.position.z)));
            Debug.DrawLine(fromPoint,MP,Color.blue,100);
            return fromPoint-MP;
        }

        static public float MouseToPointRotation(Vector2 fromPoint)
        {
            Vector2 d = MouseToPointVector(fromPoint);
            return FromVectorToRotation(d);
        }

        static public Vector2 FromRotationToVector(float zrotation, bool flip)
        {
            float flipx = 0;
            if (flip) { flipx = 180; }
            zrotation = (zrotation + flipx) * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(zrotation), Mathf.Sin(zrotation));
        }

        static public float FromVectorToRotation(Vector2 v)
        {
            float d = Mathf.Atan2(v.y,v.x) * Mathf.Rad2Deg;
            //Debug.Log(v.ToString() +" into " + d + " by " + Mathf.Atan2(v.y,v.x));
            return d;
        }

    }
}
