using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUI : MonoBehaviour
{
    [SerializeField] private TMP_Text interactableText;
    [SerializeField] private TMP_Text itemText;
    [SerializeField] private RawImage itemImage;
    public TMP_Text InteractableText
    {
        get => interactableText;
        set => interactableText = value;
    }

    public TMP_Text ItemText
    {
        get => itemText;
        set => itemText = value;
    }

    public RawImage ItemImage
    {
        get => itemImage;
        set => itemImage = value;
    }
}
