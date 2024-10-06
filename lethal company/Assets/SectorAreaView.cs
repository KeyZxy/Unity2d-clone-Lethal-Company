using UnityEngine;

public class SectorAreaView : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;
    public void CreateSprite(float radius, float angle, Color color)
    {
        var size = (int)(radius * 2 * 100);
        var actualRadius = size / 2;
        var halfAngle = angle / 2;
        Texture2D texture2D = new Texture2D(size, size);
        Vector2 centerPixel = Vector2.one * size / 2;

        // 绘制
        var emptyColor = Color.clear;
        Vector2 tempPixel;
        float tempAngle;
        float tempDisSqr;
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                tempPixel.x = x - centerPixel.x;
                tempPixel.y = y - centerPixel.y;

                tempDisSqr = tempPixel.sqrMagnitude;
                if (tempDisSqr <= actualRadius * actualRadius)
                {
                    tempAngle = Vector2.Angle(Vector2.right, tempPixel);
                    if (tempAngle < halfAngle || tempAngle > 360 - halfAngle)
                    {
                        //设置像素色值
                        texture2D.SetPixel(x, y, color);
                        continue;
                    }
                }
                texture2D.SetPixel(x, y, emptyColor);
            }
        }

        texture2D.Apply();
        _renderer.sprite = Sprite.Create(texture2D, new Rect(0, 0, size, size), Vector2.one * 0.5f);
    }
}