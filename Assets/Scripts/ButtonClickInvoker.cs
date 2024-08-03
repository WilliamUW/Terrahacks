using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickInvoker : MonoBehaviour
{
    public List<Button> targetButtons;

    public void InvokeButtonClick(int index)
    {
        if (index >= 0 && index < targetButtons.Count)
        {
            Button button = targetButtons[index];
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }

    
}
