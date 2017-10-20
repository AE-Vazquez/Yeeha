using UnityEngine;
using System.Collections;

public class InputManager : Singleton<InputManager> {


    public int  midiNote;

    public WaveType currentWave=WaveType.None;

    private bool IsListening = false;

    public delegate void OnWaveChange();

    public OnWaveChange OnWaveDetectedCallback;
    public OnWaveChange OnWaveStoppedCallback;

    public int minNote = 45;

    public int maxNote = 75;

    public bool debug_log=true;

    // Use this for initialization
    void Start ()
    {
        VoiceInput.Instance.OnInputStartedCallback += OnVoiceInputStarted;
        VoiceInput.Instance.OnInputStoppedCallback += OnVoiceInputStopped;

    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(IsListening)
        {
            midiNote = VoiceInput.Instance.midi;
            if(GetWaveType()!=WaveType.None)
            { 
                if (currentWave != GetWaveType())
                {
                    if (debug_log)
                    {
                        Debug.Log("CAMBIO DE WAVE!!!!!! ");
                        Debug.Log("WAVE ANTERIOR = " + currentWave.ToString());
                    }
                    currentWave = GetWaveType();
                    WaveChanged();
                    if (debug_log)
                    {
                        Debug.Log("WAVE NUEVA = " + currentWave.ToString());
                    }
                }
            }
            else
            {
                if (OnWaveStoppedCallback != null)
                {
                    OnWaveStoppedCallback();
                }
            }

        }
	}

    private void WaveChanged()
    {
        if(OnWaveStoppedCallback!=null)
        {
            OnWaveStoppedCallback();
        }

        if(OnWaveDetectedCallback != null)
        {
            OnWaveDetectedCallback();
        }

    }

    public void OnVoiceInputStarted()
    {
        if (!IsListening)
        {
            IsListening = true;
            if (debug_log)
            {
                //Debug.Log("SONIDO DETECTADO");
            }
        }
    }


    public void OnVoiceInputStopped()
    {
        if (IsListening)
        {
            if (debug_log)
            {
                //Debug.Log("SONIDO DETENIDO");
            }
            IsListening = false;

            midiNote = 0;
            currentWave = WaveType.None;

            if (OnWaveStoppedCallback != null)
            {
                OnWaveStoppedCallback();
            }
        }

    }


    public WaveType GetWaveType()
    {
        /*
        if(midiNote<10)
        { 
            return WaveType.None;
        }
        */
        if( midiNote<50)
        {
            return WaveType.Type1;
        }
        if (midiNote<65)
        {
            return WaveType.Type2;
        }

        return WaveType.Type3;
        
       
    }

   


}
