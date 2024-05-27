using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectUtilities : MonoBehaviour
{

  [SerializeField] private Transform itemContainer;
  public RectTransform target;
  [SerializeField] private AnimationCurve curve;
  [SerializeField] private float speed = 1;

  public ScrollRect scrollRect;
  private RectTransform scrollRectTransform;

  private void Start()
  {
    scrollRect = GetComponent<ScrollRect>();
    scrollRectTransform = scrollRect.GetComponent<RectTransform>();
  }

  private void Update()
  {
    //if (Input.GetKeyDown(KeyCode.Space))
      //SlideToItem(target);


   // Debug.Log(IsItemInView(target));
   // Debug.DrawLine(new Vector2(target.rect.xMin, target.rect.yMin), new Vector2(target.rect.xMax, target.rect.yMax), Color.yellow);
   // Debug.DrawLine(new Vector2(target.rect.xMax, target.rect.yMin), new Vector2(target.rect.xMin, target.rect.yMax), Color.yellow);

   // Debug.DrawLine(new Vector2(scrollRectTransform.anchoredPosition.x, scrollRectTransform.anchoredPosition.y), new Vector2(target.rect.xMax, target.rect.yMax), Color.cyan);
   // Debug.DrawLine(new Vector2(target.rect.xMax, target.rect.yMin), new Vector2(target.rect.xMin, target.rect.yMax), Color.yellow);
  }

  public bool IsItemInView(RectTransform target)
  {
    Vector3 positionInWord = target.parent.TransformPoint(target.localPosition);
    Vector3 positionInScroll = scrollRectTransform.InverseTransformPoint(positionInWord);

    float targetMinX = positionInScroll.x + target.rect.xMin;
    float targetMaxX = positionInScroll.x + target.rect.xMax;
    float targetMinY = positionInScroll.y + target.rect.yMin;
    float targetMaxY = positionInScroll.y + target.rect.yMax;

    float checkRectMinX = scrollRectTransform.rect.xMin - scrollRectTransform.rect.width;
    float checkRectMaxX = scrollRectTransform.rect.xMax + scrollRectTransform.rect.width;
    float checkRectMinY = scrollRectTransform.rect.yMin - scrollRectTransform.rect.height;
    float checkRectMaxY = scrollRectTransform.rect.yMax + scrollRectTransform.rect.height;

    bool contains = false;

    if (targetMaxY >= checkRectMinY && targetMinY <= checkRectMaxY && targetMaxX >= checkRectMinX && targetMinX <= checkRectMaxX)
      contains = true;

    return contains;
  }

  public void JumpToItem(int target)
  {
    RectTransform childRectTransform = itemContainer.GetChild(target).GetComponent<RectTransform>();
    JumpToItem(childRectTransform);
  }

  public void JumpToItem(RectTransform target)
  {
    Vector2 targetPosition = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    scrollRect.content.anchoredPosition = targetPosition;
  }

  public void SlideToItem(int target)
  {
    RectTransform childRectTransform = itemContainer.GetChild(target).GetComponent<RectTransform>();
    StartCoroutine(AnimateMoveToItem(childRectTransform));
  }

  public void SlideToItem(RectTransform target)
  {
    StartCoroutine(AnimateMoveToItem(target));
  }

  IEnumerator AnimateMoveToItem(RectTransform target)
  {
    Canvas.ForceUpdateCanvases();
    float curvePosition = 0f;
    Vector2 startPosition = scrollRect.content.anchoredPosition;
    Vector2 targetPosition = (Vector2)scrollRect.transform.InverseTransformPoint(scrollRect.content.position) - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);
    while (curvePosition < 1f)
    {
      scrollRect.content.anchoredPosition = startPosition + curve.Evaluate(curvePosition) * (targetPosition - startPosition);
      curvePosition += Time.deltaTime * speed;
      yield return new WaitForEndOfFrame();
    }
  }
}
