using System;
using System.Linq.Expressions;
using UnityEngine;

public static class ExpressionEX 
{
    public static bool ExprEqual<T>(Expression<Func<T, T>> a, Expression<Func<T, T>> b)
    {
        if (a == null || b == null) return a == b;
        return a.ToString() == b.ToString();
    }
}

public static class RbEX
{
    public static void ResetVel2D(this Rigidbody2D rb)
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    public static void ResetVel3D(this Rigidbody rb)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
