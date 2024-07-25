
using System.Collections.Generic;
using UnityEngine;

public abstract class Portal : MonoBehaviour, SceneChanger
{
    public abstract void OnTriggerEnter(Collider other);
    public abstract void Teleport(GameObject objectToTeleport);
    public abstract void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload=null);
}

