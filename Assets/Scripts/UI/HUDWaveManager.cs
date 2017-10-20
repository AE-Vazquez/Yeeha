using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class HUDWaveManager : Singleton<HUDWaveManager> {


    public float panelWidth=10f;

    public Image pointer;

    public GameObject waveLine;

    public bool debug_log=true;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		UpdatePointer();
        UpdateWave();
    }
    

    public void OnWaveStopped()
    {
        ResetPointer();
    }

    void UpdateWave()
    {
        waveLine.GetComponent<drawline2>().amplitud =  (((Mathf.Clamp(VoiceInput.Instance.dbValue, -10f, 3f) + 10f) / (3f + 10f)) * 0.4f);
        waveLine.GetComponent<drawline2>().frecuencia = Mathf.Clamp(GetNormalizedPosition(), 0.1f, 0.8f);
    }

    public void UpdatePointer()
    {
        float newX =( GetNormalizedPosition() * panelWidth)-955;
        //pointer.rectTransform.anchoredPosition = new Vector3(newX, pointer.rectTransform.anchoredPosition.y, 0);
		pointer.rectTransform.DOLocalMoveX(newX, 0.5f);

    }

    public void ResetPointer()
    {

        pointer.rectTransform.DOLocalMoveX(-955, 0.5f);

    }

    public float GetNormalizedPosition()
    {   
        //Debug.Log (("Número mágico" +(Mathf.Clamp(InputManager.Instance.midiNote,45,70)-45) / (70-45)));
        return ((Mathf.Clamp((float)InputManager.Instance.midiNote,InputManager.Instance.minNote,InputManager.Instance.maxNote)
        	-InputManager.Instance.minNote) / (InputManager.Instance.maxNote-InputManager.Instance.minNote));
        //return 0.5f;
    }



}
