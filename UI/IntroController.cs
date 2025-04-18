using System;
using UnityEngine;
using UnityEngine.Playables;

public class IntroController : MonoBehaviour
{
    public PlayableDirector introDirector;

    public ObjectEventSO loadMenuEvent;

    private void Awake()
    {
        introDirector = GetComponent<PlayableDirector>();

        introDirector.stopped += OnIntroFinished;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && introDirector.state == PlayState.Playing)
        {
            introDirector.Stop();
        }
    }

    private void OnIntroFinished(PlayableDirector director)
    {
        loadMenuEvent.RaisEvent(null, this);
    }
}
