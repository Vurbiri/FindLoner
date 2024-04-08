using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private string _key = "Open";

    public virtual void PlayNormal() => _animator.SetBool(_key, true);

    public virtual void PlayRevers() => _animator.SetBool(_key, false);
}
