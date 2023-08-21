using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZoneScript : MonoBehaviour
{

    public AudioSource track1;
    public AudioSource track2;
    public AudioSource track3;
    public AudioSource trackBoss;

    private AudioSource lastTrack;
    private AudioSource nextTrack;

    public EdgeCollider2D edgeCollider;
    public CapsuleCollider2D capsuleCollider;

    private int nextZone;
    private int lastZone = -1;

    private int fadeTime = 2;
    private int blackoutTime = 1;

    private bool isTransitioning = false;

    public void OnTriggerEnter2D(Collider2D collider2D) { 
    
        if (collider2D.IsTouching(edgeCollider) || collider2D.IsTouching(capsuleCollider)) {
                return;
        }

        if (collider2D.CompareTag("Zone01"))
        {
            nextZone = 1;
            nextTrack = track1;
            Debug.Log("CHANGED TO ZONE 01");

        }
        else if (collider2D.CompareTag("Zone02"))
        {
            nextZone = 2;
            nextTrack = track2;
            Debug.Log("CHANGED TO ZONE 02");

        }
        else if (collider2D.CompareTag("Zone03"))
        {
            nextZone = 3;
            nextTrack = track3;
            Debug.Log("CHANGED TO ZONE 03");

        }
        else if (collider2D.CompareTag("ZoneBoss"))
        {
            nextZone = 4;
            nextTrack = trackBoss;
            Debug.Log("CHANGED TO ZONE BOSS");
        }
        else if (collider2D.CompareTag("ZoneNoMusic"))
        {
            Debug.Log("Changed to non music zone");
            StartCoroutine(FadeOutTrack(lastTrack, null));
        }
        else
        {
            return;
        }

        if(lastTrack != null)
        {
            Debug.Log("Fading out : " + lastTrack.name);
        }
        if(nextTrack != null)
        {
            Debug.Log(" Fading in : " + nextTrack.name);
        }

        if (!isTransitioning)
        {
            StartCoroutine(FadeOutTrack(lastTrack, nextTrack));
        }

    }


    public void OnTriggerStay2D(Collider2D Collider2D)
    {
        if(lastZone == -1)
        {
            StartCoroutine(ChooseCurrentTrack(Collider2D));
        }

    }

    public IEnumerator ChooseCurrentTrack(Collider2D Collider2D)
    {
        if (Collider2D.CompareTag("Zone01"))
        {
            lastZone = 1;
            lastTrack = track1;
            
            Debug.Log("IN ZONE 01");
        }
        else if (Collider2D.CompareTag("Zone02"))
        {
            lastZone = 2;
            lastTrack = track2;
            Debug.Log("IN ZONE 02");
        }
        else if (Collider2D.CompareTag("Zone03"))
        {
            lastZone = 3;
            lastTrack = track3;
            Debug.Log("IN ZONE 03");
        }
        else if (Collider2D.CompareTag("ZoneBoss"))
        {
            lastZone = 4;
            lastTrack = trackBoss;
            Debug.Log("IN ZONE BOSS");
        }
        else if (Collider2D.CompareTag("ZoneNoMusic"))
        {
            lastZone = 5;
            lastTrack = null;
            Debug.Log("In non music zone");
        }
        else
        {
            yield return null;
        }
        
        if(lastTrack != null)
        {
            lastTrack.Play();
        }
        yield return new WaitForSecondsRealtime(1);
    }



    public IEnumerator FadeOutTrack(AudioSource oldTrack, AudioSource newTrack)
    {
        isTransitioning = true;
        Debug.Log("Entering fade out");
        if (oldTrack != null)
        {
            float startVolume = oldTrack.volume;
            lastTrack = oldTrack;
            while (oldTrack.volume > 0)
            {
                oldTrack.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            oldTrack.Stop();
            oldTrack.volume = startVolume;

            yield return new WaitForSecondsRealtime(blackoutTime);
            

        }
        if (newTrack != null)
        {
            StartCoroutine(FadeInTrack(newTrack));
        }
        isTransitioning = false;

    }

    public IEnumerator FadeInTrack(AudioSource newTrack)
    {
        Debug.Log("Entering fade in");

        float targetVolume = newTrack.volume;
        newTrack.Play();
        while(newTrack.volume < targetVolume)
        {
            newTrack.volume += targetVolume * Time.deltaTime / fadeTime;
            yield return null;
        }
        lastTrack = newTrack;
        isTransitioning = false;

        if (nextZone != int.Parse(newTrack.name.Substring(newTrack.name.Length - 2)))
        {
            Debug.Log("MUSIC CHANGED AGAIN DUE TO BOOL");            
             StartCoroutine(FadeOutTrack(lastTrack, nextTrack));
        }
    }

    public void FadeOutTrackSingle(AudioSource trackToFadeOut)
    {
        StartCoroutine(FadeOutTrack(trackToFadeOut, null));
    }

}


