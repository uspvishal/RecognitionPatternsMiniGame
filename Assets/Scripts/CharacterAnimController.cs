using UnityEngine;

public class CharacterAnimController : MonoBehaviour
{
    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayAnimationTrigger(string key)
    {
        anim.SetTrigger(key);
    }

    public void PlayAnimationBool(string key, bool val)
    {
        anim.SetBool(key, val);
    }

    public void PlayAnimationBoolTrue(string key)
    {
        anim.SetBool(key, true);
    }

    public void PlayAnimationBoolFalse(string key)
    {
        anim.SetBool(key, false);
    }
}
