using UnityEngine;
using UnityEngine.UI;

public class DialogueExpression : MonoBehaviour
{
    public Image Expression;
    public GameObject[] Emotions;
    [Space]
    public RectTransform TransformHolder;
    public RectTransform TransformExpression;
    public RectTransform TransformEmotions;
    public RectTransform TransformBack;
    [Space]
    public Vector2 OffsetHolderRight;
    public Vector2 OffsetHolderLeft;
    [Space]
    public Vector2 OffsetExpressionRight;
    public Vector2 OffsetExpressionLeft;
    [Space]
    public Vector2 OffsetExpressionDetailRight;
    public Vector2 OffsetExpressionDetailLeft;

    private Vector3 flippedScale = new Vector3(-1, 1, 1);

    /// <summary>
    /// set side of extression
    /// </summary>
    /// <param name="position">-1 : left | 1 : right </param>
    public void SetSide (int position)
    {
        if (position == -1)
        {
            TransformHolder.anchoredPosition = OffsetHolderLeft;
            TransformExpression.anchoredPosition = OffsetExpressionLeft;
            TransformEmotions.anchoredPosition = OffsetExpressionDetailLeft;
            TransformExpression.localScale = Vector3.one;
            TransformBack.localScale = flippedScale;
        }
        else
        {
            TransformHolder.anchoredPosition = OffsetHolderRight;
            TransformExpression.anchoredPosition = OffsetExpressionRight;
            TransformEmotions.anchoredPosition = OffsetExpressionDetailRight;
            TransformExpression.localScale = flippedScale;
            TransformBack.localScale = Vector3.one;
        }
    }
}
