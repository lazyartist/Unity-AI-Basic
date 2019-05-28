using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Test : MonoBehaviour
{

    // Use this for initialization
    StringBuilder sb = new StringBuilder();
    void Start()
    {
        string format = "0.000";
        for (int i = 0; i <= 360; i += 15)
        {
            float rad = i * Mathf.Deg2Rad;
            float sin = Mathf.Sin(rad);
            float cos = Mathf.Cos(rad);
            float tan = Mathf.Tan(rad);

            float x = i * cos;
            float y = i * sin;

            float asin = Mathf.Asin(sin);
            float acos = Mathf.Acos(cos);
            float atan = Mathf.Atan(tan);
            float atan2 = Mathf.Atan2(y, x);
            sb.Append(U.d(
                //i, rad.ToString(format), sin.ToString(format), cos.ToString(format), tan.ToString(format),
                asin.ToString(format)  , (int)(asin * Mathf.Rad2Deg) ,
                acos.ToString(format)  , (int)(acos * Mathf.Rad2Deg) ,
                atan.ToString(format)  , (int)(atan * Mathf.Rad2Deg),
                atan2.ToString(format) , (int)(atan2 * Mathf.Rad2Deg)
                //asin.ToString(format) + string.Format("({0})", asin * Mathf.Rad2Deg),
                //acos.ToString(format) + string.Format("({0})", acos * Mathf.Rad2Deg),
                //atan.ToString(format) + string.Format("({0})", atan * Mathf.Rad2Deg),
                //atan2.ToString(format) + string.Format("({0})", atan2 * Mathf.Rad2Deg)
                ));

            sb.Append("\n");
        }

        Debug.ClearDeveloperConsole();
        Debug.Log(sb);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
