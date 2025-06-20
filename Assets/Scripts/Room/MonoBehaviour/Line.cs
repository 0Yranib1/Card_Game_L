using System;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer LineRenderer;
    public float offsetSpeed = 0.1f;

    private void Update()
    {
        if (LineRenderer != null)
        {
            //获取纹理偏移
            Vector2 offset = LineRenderer.material.mainTextureOffset;
            
            offset.x+=offsetSpeed * Time.deltaTime;
            
            LineRenderer.material.mainTextureOffset = offset;
        }
    }
}
