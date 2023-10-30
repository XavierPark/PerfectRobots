using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//	This script handles automatic opening and closing sliding doors
//	It is fired by triggers and the door closes if found no character in the trigger area


//	Door status
public enum EndDoorStatus
{
    Closed,
    Open,
    Animating
}

[RequireComponent(typeof(AudioSource))]
public class EndDoor : MonoBehaviour
{

    private EndDoorStatus status = EndDoorStatus.Closed;

    [SerializeField]
    private Transform halfDoorLeftTransform;    //	Left panel of the sliding door
    [SerializeField]
    public Transform halfDoorRightTransform;    //	Right panel of the sliding door

    [SerializeField]
    private float slideDistance = 0.88f;        //	Sliding distance to open each panel the door

    private Vector3 leftDoorClosedPosition;
    private Vector3 leftDoorOpenPosition;

    private Vector3 rightDoorClosedPosition;
    private Vector3 rightDoorOpenPosition;

    [SerializeField]
    private float speed = 1f;                   //	Spped for opening and closing the door

    //	Sound Fx
    [SerializeField]
    private AudioClip doorOpeningSoundClip;
    [SerializeField]
    public AudioClip doorClosingSoundClip;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        leftDoorClosedPosition = new Vector3(0f, 0f, 0f);
        leftDoorOpenPosition = new Vector3(-slideDistance, 0f, 0f);

        rightDoorClosedPosition = new Vector3(0f, 0f, 0f);
        rightDoorOpenPosition = new Vector3(slideDistance, 0f, 0f);

        audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator OpenDoors()
    {
        if (doorOpeningSoundClip != null)
        {
            audioSource.PlayOneShot(doorOpeningSoundClip, 0.7F);
        }

        status = EndDoorStatus.Animating;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            halfDoorLeftTransform.localPosition = Vector3.Slerp(leftDoorClosedPosition, leftDoorOpenPosition, t);
            halfDoorRightTransform.localPosition = Vector3.Slerp(rightDoorClosedPosition, rightDoorOpenPosition, t);

            yield return null;
        }

        status = EndDoorStatus.Open;

    }

    IEnumerator CloseDoors()
    {
        if (doorClosingSoundClip != null)
        {
            audioSource.PlayOneShot(doorClosingSoundClip, 0.7F);
        }

        status = EndDoorStatus.Animating;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * speed;

            halfDoorLeftTransform.localPosition = Vector3.Slerp(leftDoorOpenPosition, leftDoorClosedPosition, t);
            halfDoorRightTransform.localPosition = Vector3.Slerp(rightDoorOpenPosition, rightDoorClosedPosition, t);

            yield return null;
        }

        status = EndDoorStatus.Closed;

    }

    //	Forced door opening
    public bool DoOpenDoor()
    {

        if (status != EndDoorStatus.Animating)
        {
            if (status == EndDoorStatus.Closed)
            {
                StartCoroutine("OpenDoors");
                return true;
            }
        }

        return false;
    }

    //	Forced door closing
    public bool DoCloseDoor()
    {

        if (status != EndDoorStatus.Animating)
        {
            if (status == EndDoorStatus.Open)
            {
                StartCoroutine("CloseDoors");
                return true;
            }
        }

        return false;
    }
}