using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
	//Attributes
	public bool simpleButton;
	public Color normalColor;
	public Color hoverColor;
	public Color pressedColor;
	public Color disabledColor;

	private Text buttonText;
	private Image buttonImage;
	private bool hasFocus;
	private Button button;

	void Awake ()
	{
		buttonText = GetComponentInChildren<Text> ();
		hasFocus = false;
		button = GetComponent<Button> ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (!button.interactable && buttonText.color != disabledColor) {
			buttonText.color = disabledColor;
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		buttonText.color = hoverColor;
		hasFocus = true;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		buttonText.color = normalColor;
		hasFocus = false;
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (simpleButton) {
				buttonText.color = normalColor;
			} else {
				buttonText.color = pressedColor;
			}
		}
	}

	public void OnPointerUp (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			if (hasFocus) {
				buttonText.color = hoverColor;
			} else {
				buttonText.color = normalColor;
			}
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			hasFocus = false;
			buttonText.color = normalColor;
		}
	}

	public void SetColor (Color color)
	{
		if (buttonText != null) {
			buttonText.color = color;
		}
	}
}