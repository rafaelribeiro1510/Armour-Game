using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIGlow : MonoBehaviour
{
    public static UIGlow Instance { get; private set; }

    private Image _image;
    
    [SerializeField] private Color successColor;
    [SerializeField] private Color mistakeColor;

    [SerializeField] private float alpha;
    [SerializeField] private float duration;
    
    private void SingletonInitialization()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else {
            Instance = this;
        }
    }
    
    private void Awake()
    {
        SingletonInitialization();
        _image = GetComponent<Image>();
        successColor = new Color(successColor.r, successColor.g, successColor.b, alpha);
        mistakeColor = new Color(mistakeColor.r, mistakeColor.g, mistakeColor.b, alpha);
    }

    public void GlowSuccess()
    {
        _image.color = successColor;
        _image.DOFade(0, duration);
    }
    
    public void GlowMistake()
    {
        _image.color = mistakeColor;
        _image.DOFade(0, duration);
    }
}
