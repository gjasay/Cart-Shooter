using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Color _onHoverColor;
    [SerializeField]
    private Color _defaultColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _text.color = _onHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _text.color = _defaultColor;
    }
}