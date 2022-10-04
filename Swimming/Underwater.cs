using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class Underwater : MonoBehaviour
{
    public GameObject PlayerCam, Boundingbox;
    public Color UnderwaterColour;
    public bool Isunderwater;
    public Volume Post;

    private Vignette VG;
    private DepthOfField DOF;
    private ColorAdjustments CA;

    private void Start()
    {
        Post.profile.TryGet(out VG);
        Post.profile.TryGet(out DOF);
        Post.profile.TryGet(out CA);
    }

    private void FixedUpdate()
    {
        if (Boundingbox.GetComponent<BoxCollider>().bounds.Contains(PlayerCam.transform.position))
        {
            Isunderwater = true;
        }
        else
        {
            Isunderwater = false;
        }

        if (Isunderwater)
        {
            VG.intensity.value = 0.35f;
            DOF.focusDistance.value = 0.1f;
            CA.colorFilter.value = UnderwaterColour;
        }
        else
        {
            VG.intensity.value = 0.292f;
            DOF.focusDistance.value = 5f;
            CA.colorFilter.value = Color.white;
        }

    }

}
