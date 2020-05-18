using UnityEngine;

public class VFXPlay : MonoBehaviour
{
    private ParticleSystem[] vfx;

    private void Awake()
    {
        vfx = GetComponentsInChildren<ParticleSystem>();
    }

    public void Play()
    {
        foreach (ParticleSystem part in vfx)
            part.Play();
    }
}
