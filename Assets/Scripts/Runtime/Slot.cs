using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace USP.MiniGame.recognitionPatterns
{
	[RequireComponent(typeof(Collider2D))]
	public class Slot : MonoBehaviour
	{
		[Header("� R E F E R E N C E S")]
		[SerializeField] private new Collider2D collider;
		[SerializeField] private SpriteRenderer mask, hint;

		[Header("� T W E E N   S E T T I N G S")]
		public Color fadeTarget = Color.white;
		public float fadeTweenDuration = 0.37F;
		public Ease fadeTweenEase = Ease.OutCubic;

		private Color originalMaskColor;
		private Tweener fadeTween;

		public string Key => hint.sprite.name;
		public int Order => mask != null ? mask.sortingOrder : 0;

		public bool isComplete;
		public UnityEvent OnComplete;



		private void Reset()
		{
			collider = GetComponent<Collider2D>();
			var renderers = GetComponentsInChildren<SpriteRenderer>();
			mask = renderers[0]; hint = renderers[1];
			hint.name = "Hint - " + Key;
		}
		private void Awake()
		{
			originalMaskColor = mask.color;
		}
		private void OnEnable()
		{
			collider.enabled = hint.enabled = true;
			fadeTween = mask.DOColor(default, fadeTweenDuration).SetEase(fadeTweenEase).OnKill(() => fadeTween = null).SetAutoKill(false).Pause();
		}
		private void OnDisable()
		{
			collider.enabled = hint.enabled = false;
			fadeTween?.Kill();
		}

		public void ChangeLayer(int index)
		{
			GetComponent<SpriteRenderer>().sortingOrder = index;
			if (transform.childCount > 0)
			{
				transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = index + 1;
			}
		}

		public void Complete()
		{
			
			HideAfterComplete();
			collider.enabled = false;
			isComplete = true;
			OnComplete?.Invoke();
		}

		void HideAfterComplete()
		{
			Color color =Color.white ;
			color.a = Mathf.Clamp01(0);
			fadeTween.ChangeEndValue(color, true).Restart();
		}

		public void Fade(float alpha)
		{
			
			Color color = alpha == 1F ? originalMaskColor : fadeTarget;
			color.a = Mathf.Clamp01(alpha);
			fadeTween.ChangeEndValue(color, true).Restart();
			 //old 
			/*Color color =Color.white ;
			color.a = Mathf.Clamp01(1);
			fadeTween.ChangeEndValue(color, true).Restart();*/
			
		}
	}
}