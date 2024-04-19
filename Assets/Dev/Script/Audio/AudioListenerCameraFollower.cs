using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioListenerCameraFollower : MonoBehaviour
{
    [SerializeField] private Camera mainMenuCamera;
    [SerializeField] private Camera devMainGameCamera;

    private enum scenes
    {
        MainMenu,
        DevMainGame,
    }

    private void Update()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;

        if (activeSceneName == Enum.GetName(typeof(scenes), 0))
        {
            FollowCamera(mainMenuCamera);
        }
        else if (activeSceneName == Enum.GetName(typeof(scenes), 1))
        {
            FollowCamera(devMainGameCamera);
        }
    }

    private void FollowCamera(Camera cameraToFollow)
    {
        if (cameraToFollow != null)
        {
            transform.position = cameraToFollow.transform.position;
            transform.rotation = cameraToFollow.transform.rotation;
        }
    }
}