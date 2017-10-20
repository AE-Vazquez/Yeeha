using UnityEngine;
using System.Collections;

public class CabreroManager : MonoBehaviour {

    Animator anim;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        VoiceInput.Instance.OnInputStartedCallback += ActivateAnimation;
		VoiceInput.Instance.OnInputStoppedCallback += DeActivateAnimation;

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ActivateAnimation()
    {
        if(anim)
        {
            anim.SetBool("play", true);
        }
    }

    void DeActivateAnimation()
    {
        if (anim)
        {
            anim.SetBool("play", false);
        }
    }
}
