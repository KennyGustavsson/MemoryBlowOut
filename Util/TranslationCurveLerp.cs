using Freya;
using UnityEngine;

public static class TranslationCurveLerp
{
    public static Vector3 LerpCurve(Vector3 startPoint, Vector3 endPoint, Vector3 curveDirection, float curveStrength, float t)
    {
        t = t.Clamp01();
        
        Vector3 curve = curveDirection * Mathf.Sin(t * 180 * Mathf.Deg2Rad) * curveStrength;
        
        return curve + Vector3.Lerp(startPoint, endPoint, t);
         
    }
}
