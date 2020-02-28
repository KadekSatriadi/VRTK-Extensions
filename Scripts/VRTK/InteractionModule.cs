using VRTK;
using UnityEngine;

public abstract class InteractionModule : MonoBehaviour
{
    public InteractionPolicy interactionPolicy;

    protected bool enable = true;

    public virtual void Enable()
    {
        enable = true;
    }

    public virtual void Disable()
    {
        enable = false;
    }

    protected void Update()
    {
        if (!enable) return;
    }
}
