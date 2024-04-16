using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentinganimationController : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] GameObject nubes;
    [SerializeField] GameObject pj;
    [SerializeField] GameObject title;
    [SerializeField] GameObject textToStart;
    
    [SerializeField] SpriteRenderer luz;
    [SerializeField] SpriteRenderer spriteDestello;
    [Space(5)]
    [Header("Animcontroller")]
    [SerializeField] Animator animPj;
    [SerializeField] GameObject billos;



    private IEnumerator Start()
    {
        LeanTween.scale(nubes, new Vector3(0.3523525f, 0.396344453f, 1.24420762f), 1).setEaseOutCubic(); // nubes

        
        yield return new WaitForSeconds(0.8f);

        LeanTween.value(gameObject, 0, 1, 0.3f).setOnUpdate((float value) => { spriteDestello.color = new Color(1, 1, 1, value); });//Destello
        LeanTween.delayedCall(gameObject, 0.5f, () => {
            LeanTween.value(gameObject, 1, 0, 1f).setOnUpdate((float value) => { spriteDestello.color = new Color(1, 1, 1, value); });});


            LeanTween.scale(title, new Vector3(0.4f, 0.4f, 0.4f), 0.5f).setEaseOutBack() // titulo
            .setOnComplete(()=> {
                
                LeanTween.scale(title, new Vector3(0.44f, 0.44f, 0.44f), 0.8f).setLoopPingPong(-1); // titulo respirando
                });

        yield return new WaitForSeconds(0.8f);

        LeanTween.moveX(pj, -6f, 0.5f).setEaseOutBack().setOnComplete(()=> { animPj.enabled = true; });

        yield return new WaitForSeconds(0.8f);

        billos.SetActive(true);

        yield return new WaitForSeconds(0.2f);

        LeanTween.value(gameObject, 0, 1, 1.5f).setOnUpdate((float value) => { luz.color = new Color(1, 1, 1, value); })
            .setOnComplete(() => {
                LeanTween.value(gameObject, 1f, 0.5f, 2f).setOnUpdate((float value) => { luz.color = new Color(1, 1, 1, value); }).setLoopPingPong(-1);
            });//Luz

        yield return new WaitForSeconds(1.8f);

        LeanTween.moveY(textToStart, 120, 1).setEaseOutBack().setOnComplete(() => { LeanTween.moveY(textToStart, 130, 1).setLoopPingPong(-1); });
        
        yield return new WaitForSeconds(1f);

        MainMenuUI.canChangeScene = true;

    }

}
