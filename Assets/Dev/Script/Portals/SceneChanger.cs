using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SceneChanger
{
    void ChangeScenes(GameManager.scenes sceneToSetActive, List<GameManager.scenes> scenesLoad, List<GameManager.scenes> scenesUnload=null);
}
