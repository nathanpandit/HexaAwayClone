using DG.Tweening;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private float displayDuration = 4f;
    [SerializeField] private float widthRatio = 0.8f; // 80% of screen width
    [SerializeField] private float heightRatio = 0.45f; // 45% of screen height
    
    // Reference resolution where scale 1,1 gives the desired ratios
    // iPhone 12 resolution: 390x844 (points) or 1170x2532 (pixels)
    // Using a common reference: 1080x1920 (matches Canvas Scaler reference)
    [SerializeField] private Vector2 referenceResolution = new Vector2(1080f, 1920f);
    
    private Vector3 calculatedScale;

    private void Awake()
    {
        CalculateScale();
    }

    private void OnEnable()
    {
        // Recalculate scale in case screen size changed
        CalculateScale();
        transform.localScale = Vector3.zero;
        transform.DOScale(calculatedScale, displayDuration);
    }

    private void CalculateScale()
    {
        // Get current screen dimensions
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        
        // Calculate scale to maintain 80% width and 45% height across all devices
        // The scale is proportional to the screen size relative to the reference resolution
        // This ensures the popup always takes the same percentage of screen space
        float scaleX = (screenWidth / referenceResolution.x);
        float scaleY = (screenHeight / referenceResolution.y);
        
        // Apply the width and height ratios to maintain consistent screen coverage
        // The ratios are already factored into the base scale calculation
        calculatedScale = new Vector3(scaleX * widthRatio, scaleY * heightRatio, 1f);
    }
}