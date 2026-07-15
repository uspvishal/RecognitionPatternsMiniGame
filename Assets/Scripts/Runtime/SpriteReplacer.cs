using UnityEngine;

namespace USP.MiniGame.recognitionPatterns
{
    /// <summary>
    /// Script by Vishal Lakhani
    /// Email: usp.vishal@gmail.com
    /// Description:
    /// </summary>
    public class SpriteReplacer : MonoBehaviour
    {
        #region Variables
        public Sprite Sprite1;
        public Sprite Sprite2;
        public SpriteReplacer anotherRef;
        public AudioClip clickSfx;
        AudioSource source;
        SpriteRenderer Renderer => GetComponentInChildren<SpriteRenderer>();


        #endregion

        #region Unity Methods

        void Start()
        {
            if (clickSfx)
            {
                source = gameObject.AddComponent<AudioSource>();
            }
        }

        void OnMouseDown()
        {

            if (Renderer.sprite == Sprite1)
            {
                Renderer.sprite = Sprite2;
                anotherRef?.OnSwitch(true);
            }
            else if (Renderer.sprite == Sprite2)
            {
                Renderer.sprite = Sprite1;
                anotherRef?.OnSwitch(false);
            }
            if (source && clickSfx)
            {
                source.PlayOneShot(clickSfx);
            }
        }

        public void OnSwitch(bool val)
        {
            if (val)
            {
                Renderer.sprite = Sprite2;
            }
            else
            {
                Renderer.sprite = Sprite1;
            }
        }

        #endregion

        #region Public Methods



        #endregion

        #region Private Methods



        #endregion
    }
}