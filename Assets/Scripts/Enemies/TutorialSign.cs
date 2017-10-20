using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialSign : MonoBehaviour {


    public GameObject completedSign;

    public bool completed;

    private bool saving = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Show()
    {
        GetComponent<RectTransform>().DOLocalMoveY(-200, 1f);
    }


    public void StartSaving()
    {
        if (!saving)
        {
            saving = true;
            transform.DOShakePosition(2, 2.5f);
            Invoke("CompleteSave", 1.5f);
        }

    }

    void CompleteSave()
    {
        completed = true;
        DOTween.Kill(transform);
        completedSign.SetActive(true);
        TutorialController.Instance.NotifyCompletion();
    }

    public void CancelSave()
    {
		CancelInvoke ();
        if (saving)
        {
            saving = false;
            DOTween.Kill(transform);
        }
    }
}
