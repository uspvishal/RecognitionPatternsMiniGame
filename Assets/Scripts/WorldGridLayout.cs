using DG.Tweening;
using UnityEngine;
using USP.MiniGame.Addition;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// </summary>
    [ExecuteAlways]
    public class WorldGridLayout : MonoBehaviour
    {
        
        public float spacing = 0.2f;
        public float spacingIPAD = .1f;

        public enum Alignment
        {
            Left,
            Center,
            Right
        }
        [Header("Layout")]
        public Alignment alignment = Alignment.Center;

        [Header("Axis")]
        public bool horizontal = true;

        [ContextMenu("Update Position")]
        public void UpdatePosition()
        {
            int childCount = transform.childCount;

            if (childCount == 0)
                return;

            float totalWidth = 0f;
            Debug.Log("IS IPAD " + IsWideAspect);
            float finalSpacing = IsWideAspect ? spacing : spacingIPAD;
            // Calculate total required width
            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);

                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

                if (sr != null)
                {
                    totalWidth += GetSize(sr);
                }
                
                if (i < childCount - 1)
                    totalWidth += finalSpacing;
            }

            // Determine starting offset
            float startOffset = 0f;

            switch (alignment)
            {
                case Alignment.Left:
                    startOffset = 0f;
                    break;

                case Alignment.Center:
                    startOffset = -totalWidth * 0.5f;
                    break;

                case Alignment.Right:
                    startOffset = -totalWidth;
                    break;
            }

            float current = startOffset;

            for (int i = 0; i < childCount; i++)
            {
                Transform child = transform.GetChild(i);

                SpriteRenderer sr = child.GetComponent<SpriteRenderer>();

                if (sr == null)
                    continue;

                float size = GetSize(sr);

                // move to center of sprite
                current += size * 0.5f;

                Vector3 pos;

                if (horizontal)
                {
                    pos = transform.position + new Vector3(current, 0, 0);
                }
                else
                {
                    pos = transform.position + new Vector3(0, current, 0);
                }

                child.position = pos;

                // move to next slot
                current += size * 0.5f + finalSpacing;
            }
        }

        private void Awake()
        {
            UpdatePosition();
        }

        float GetSize(SpriteRenderer sr)
        {
            return horizontal
                ? sr.bounds.size.x
                : sr.bounds.size.y;
        }

        private void OnValidate()
        {
            UpdatePosition();
        }

        public  bool IsWideAspect
        {
            get
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                {
                    var size = UnityEditor.Handles.GetMainGameViewSize();

                    float aspect = size.x / size.y;

                    return aspect <= 1.29f || aspect >= 1.36f;
                }
#endif

                float runtimeAspect = (float)Screen.width / Screen.height;

                return runtimeAspect <= 1.29f || runtimeAspect >= 1.36f;
            }
        }
       

#if UNITY_EDITOR
        private void OnTransformChildrenChanged()
        {
            UpdatePosition();
        }
#endif
    }
}