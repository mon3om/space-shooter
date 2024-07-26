using UnityEngine;
using UnityEngine.Events;

public class RendererEvent : MonoBehaviour
{
    public UnityEvent onBecameVisible;
    public UnityEvent onBecameInvisible;

    private void OnBecameVisible()
    {
        onBecameVisible?.Invoke();
    }

    private void OnBecameInvisible()
    {
        onBecameInvisible?.Invoke();
    }
}
