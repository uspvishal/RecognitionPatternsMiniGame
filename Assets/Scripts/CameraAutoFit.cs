using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
      [RequireComponent(typeof(Camera))]
      public class CameraAutoFit : MonoBehaviour
      {
            public enum FitMode { Horizontal, Vertical }

            public Camera Camera;
            public SpriteRenderer Background_mob, Background_ipad;
            public Vector2 Padding;

            public FitMode Mode;
            public float MaxOrthographicSize = 5.4F;

            [SerializeField] private bool autoApplyOnStart = true;
            [SerializeField] private bool DelayedStart;
            public static CameraAutoFit instance;

            public static bool IsWideAspect
            {
                  get
                  {
                        float aspect = (float)Screen.width / Screen.height;
                        return aspect <= 1.29F || aspect >= 1.36F;
                  }
            }

            void Awake()
            {
                  instance = this;
            }


            private void Reset()
            {
                  Camera = GetComponent<Camera>();
            }
            public void Start()
            {
                  if (autoApplyOnStart)
                  {
                        Apply(Mode);

                  }
            }

            void ApplyDElayed()
            {
                  Apply(Mode);
            }

            public void Apply(FitMode mode)
            {
                  float aspect = (float)Screen.width / Screen.height;

                  Bounds bounds = IsWideAspect ? Background_mob.bounds : Background_ipad.bounds;
                  bounds.Expand(new Vector3(Padding.x * 2F, Padding.y * 2F));

                  float target = Mathf.Min(mode switch { FitMode.Horizontal => bounds.extents.x / aspect, FitMode.Vertical => bounds.extents.y, _ => Camera.orthographicSize }, MaxOrthographicSize);

                  Camera.orthographicSize = target;

                  Vector3 position = Camera.transform.position;
                  position.x = bounds.center.x;
                  position.y = bounds.center.y;
                  Camera.transform.position = position;
            }
      }
}