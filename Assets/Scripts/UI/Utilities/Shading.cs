using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Shading : MonoBehaviour
{
    [SerializeField] Color _defaultColor = Color.white;
    [SerializeField] float _prePause = 0f;
    [SerializeField] float _fadeDuration = 0.5f;

    private IEnumerator Start()
    {
        Image image = GetComponent<Image>();
        image.color = _defaultColor;
        Color targetColor = _defaultColor;
        targetColor.a = 0f;

        yield return new WaitForSecondsRealtime(_prePause);
        image.CrossFadeColor(targetColor, _fadeDuration, true, true);
    }
}
