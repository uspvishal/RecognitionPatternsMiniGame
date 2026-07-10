using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace USP.Minigame.PoliceOfficer
{
	public class Transition : MonoBehaviour
	{
		[SerializeField] private Image target;

		public float initialDelay;
		public float scaleDuration = 1F;
		public Ease scaleEase = Ease.Linear;


		public Color SetColor { set => target.color = value; }
		public void Play(TweenCallback midpoint)
		{
			var seq = DOTween.Sequence();
			seq.AppendInterval(initialDelay);
			seq.Append(target.transform.DOScaleX(1F, scaleDuration).SetEase(scaleEase).From(0F));
			seq.AppendCallback(midpoint);
			seq.Append(target.transform.DOScaleY(0F, scaleDuration).SetEase(scaleEase).From(1F));
			seq.Play();
		}
		public void PlaySlideOnce(TweenCallback midpoint)
		{
			target.transform.localScale = new Vector3(0F, 1F, 1F);
			var seq = DOTween.Sequence();
			seq.AppendInterval(initialDelay);
			seq.Append(target.transform.DOScaleX(1F, scaleDuration).SetEase(scaleEase));
			seq.AppendCallback(midpoint);
			seq.JoinCallback(() => target.rectTransform.pivot = new Vector2(1F, 0F));
			seq.Append(target.transform.DOScaleX(0F, scaleDuration).SetEase(scaleEase));
			seq.OnComplete(() => target.rectTransform.pivot = new Vector2(0F, 0F));
			seq.Play();
		}
	}
}