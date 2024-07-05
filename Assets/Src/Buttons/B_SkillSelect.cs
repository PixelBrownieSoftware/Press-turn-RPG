using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class B_SkillSelect : B_Int
{
    public Image elementPic;
    public S_GuiList cost;
    public void SetElement(S_Element element) {
        if(element.elementImage != null)
            elementPic.sprite = element.elementImage;
        SetButtonColour(element.elementColour);
    }

}
