using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WormHole : MonoBehaviour
{
    private const float InAnimTime = 0.4f;
    private const float OutAnimTime = 0.3f;
    
    [SerializeField] private Image _wormImage;
    
    public bool InAnim = false;

    public void PlaySpawnAnim()
    {
        InAnim = true;
        Transform wormImageTransform = _wormImage.transform;
        
        Sequence sequence = DOTween.Sequence(gameObject);
        
        sequence.Append(wormImageTransform.DOScale(Vector3.one * 1.5f, InAnimTime));
        sequence.Append(wormImageTransform.DOScale(Vector3.zero, OutAnimTime));

        sequence.Play();
        sequence.OnComplete(() => InAnim = false);
        sequence.SetLink(gameObject);
    }

    public void Stop()
    {
        _wormImage.transform.DOKill();
        _wormImage.transform.localScale = Vector3.zero;
        InAnim = false;
    }
}
