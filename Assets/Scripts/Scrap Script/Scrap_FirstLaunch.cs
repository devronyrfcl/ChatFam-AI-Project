using UnityEngine;
using DG.Tweening;
using System.Collections;

public class scrap_firstlauch : MonoBehaviour
{
    public RectTransform object1;
    public RectTransform object2;
    public RectTransform object3;

    public float moveDuration = 1f; // duration of each move
    public float waitTime = 0.5f;   // wait between moves
    public float stayTime = 1f;     // adjustable time to stay at target position

    private void Start()
    {
        StartCoroutine(AnimateSequence());
        
    }

    // Coroutine to handle the animation sequence
    IEnumerator AnimateSequence()
    {
        //object 1  will move to y = 2000 using dotween and tween animation for 1 sec
        yield return new WaitForSeconds(waitTime);
        yield return new WaitForSeconds(moveDuration);
        object1.DOAnchorPosY(2000, moveDuration).SetEase(Ease.InOutSine);
        //yield return new WaitForSeconds(moveDuration + stayTime);
        //object 2 will move to y = 123 using dotween and tween animation for 1 sec
        object2.DOAnchorPosY(123, moveDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(moveDuration + stayTime);
        //object 2 will move to y = -2000 using dotween and tween animation for 1 sec
        //object2.DOAnchorPosY(2000, moveDuration).SetEase(Ease.InOutSine);
        //yield return new WaitForSeconds(moveDuration + stayTime);
        //object 3 will move to y = 123 using dotween and tween animation for 1 sec
        object3.DOAnchorPosY(174, moveDuration).SetEase(Ease.InOutSine);
        //yield return new WaitForSeconds(moveDuration + stayTime);
        //object 3 will move to y = -2000 using dotween and tween animation for 1 sec
    }


}
