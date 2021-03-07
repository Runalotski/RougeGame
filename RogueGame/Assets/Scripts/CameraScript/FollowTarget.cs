using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Camera will follow the target transform at fixed distance and angle.
/// </summary>
public class FollowTarget : MonoBehaviour
{
    public Transform target;

    public enum TrackModes{locationSet, angleSet}

    public TrackModes trackMode;

    [Header("Shared Veriables")]

    /// <summary>
    /// The height relative to the target origin to focus on. This is to allow the camera to look over the model
    /// </summary>
    [Range(-1f,1f)]
    public float heightFocusOffest;

    /// <summary>
    /// Flat Distance to target.
    /// </summary>
    public float followDistance;

    /// <summary>
    /// Angle relative to World Z+
    /// </summary>
    [Range(-180,180f)]
    public float followOrbitAngle;

    [Header("Angle Set Veriables")]

    /// <summary>
    /// The angle foe the camera to hold in AngleSet mode
    /// </summary>
    [Range(0, 90f)]
    public float targetAngle;

    [Header("Location Set Veriables")]

    /// <summary>
    /// Camera Hight from World Y
    /// </summary>
    public float followHeight;

    /// <summary>
    /// Camera rotation around X Axis
    /// </summary>
    float followPitch;


    bool loggedNoTarget = false;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            target = playerObj.transform;
    }

    // Update is called once per frame
    void Update(){

        if (target != null)
        {

            loggedNoTarget = false;

            switch (trackMode)
            {
                case TrackModes.locationSet:
                    PositionSetTrack();
                    break;
                case TrackModes.angleSet:
                    AngleSetTrack();
                    break;
            }
        }
        else if (!loggedNoTarget)
        {
            loggedNoTarget = true;
            Debug.LogWarning(this.gameObject + " has not target to follow");
        }
    }

    /// <summary>
    /// Set the distance and height orbit angle of the camera.
    /// </summary>
    void PositionSetTrack(){

        //Calculate pitch to look at target
        followPitch = (Mathf.Atan((followHeight - heightFocusOffest) / Mathf.Abs(-followDistance)) * Mathf.Rad2Deg);

        //Set Camera rotation
        this.transform.localEulerAngles = new Vector3(followPitch, 0, 0);
        this.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, followOrbitAngle, transform.rotation.eulerAngles.z);

        //Set Camera Position
        //Get Camera Vector if orbitAngle was set to 0
        Vector3 cameraPostion = new Vector3(0, followHeight, -followDistance);

        //rotate Vector by orbitAngle
        cameraPostion = Quaternion.AngleAxis(followOrbitAngle, target.up) * cameraPostion;

        this.transform.position = target.transform.position + cameraPostion;




    }

    void AngleSetTrack(){

        //Set Camera rotation
        this.transform.localEulerAngles = new Vector3(targetAngle, 0, 0);
        this.transform.eulerAngles = new Vector3(transform.rotation.eulerAngles.x, followOrbitAngle, transform.rotation.eulerAngles.z);

        //So/h    C*h = a/h
        //Get the camera offset to player caratcer for set angle and distance.
        float theta = targetAngle;
        float opposite = Mathf.Sin(theta * Mathf.Deg2Rad) * -followDistance;
        float adjacent = Mathf.Cos(theta * Mathf.Deg2Rad) * -followDistance;

        //Set Camera Position
        //Get Camera Vector if orbitAngle was set to 0
        Vector3 cameraPostion = new Vector3(0, -opposite + heightFocusOffest, adjacent);

        //rotate Vector by orbitAngle
        cameraPostion = Quaternion.AngleAxis(followOrbitAngle, target.up) * cameraPostion;

        this.transform.position = target.transform.position + cameraPostion;
    }
}
