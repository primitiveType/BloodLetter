using UnityEngine;

public class CustomRadialInput : UltimateRadialMenuInputManager
{

    private Vector2 lastInput;
    public override void CustomInput(ref bool enableMenu, ref bool disableMenu, ref Vector2 input, ref float distance, ref bool inputDown,
        ref bool inputUp, int radialMenuIndex)
    {

        enableMenu = Input.GetMouseButton(2);
        disableMenu = !Input.GetMouseButton(2);
        input = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        if (input.sqrMagnitude > 0)
        {
            lastInput = input;
        }
        else
        {
            input = lastInput;
        }
        distance = input.sqrMagnitude;
        inputDown = true;
    }
}