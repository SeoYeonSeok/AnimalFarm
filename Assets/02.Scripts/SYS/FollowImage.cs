using UnityEngine;
using UnityEngine.UI;

public class FollowImage : MonoBehaviour
{
    public Image fillImage;            // Filled Image (Radial)
    public RectTransform ellipseCenter; // 타원의 중심 (FillImage의 RectTransform)
    public RectTransform animalImage;  // 움직일 동물 이미지

    public float a = 200f;  // 가로 반지름
    public float b = 100f;  // 세로 반지름

    void Update()
    {
        float theta = fillImage.fillAmount * 2f * Mathf.PI;  // 0~1 → 0~2π

        // 타원 경로 좌표 계산
        float x = a * Mathf.Sin(theta); // sin을 사용해야 시작점이 'Top'에서 시작됨
        float y = b * Mathf.Cos(theta);

        animalImage.anchoredPosition = new Vector2(x, y);
    }
}
