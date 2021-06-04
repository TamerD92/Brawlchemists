using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingController : BaseController
{
    public float ThrowStrength, throwAngle;

    public Transform handTransform, ThrowTransform;

    private void Update()
    {
        if (mainController.isPreparing)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = (mainController.transform.position.z - Camera.main.transform.position.z);
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector2 dir = mousePos - handTransform.position;

            handTransform.right = dir;


            mousePos = mousePos - mainController.transform.position;
            throwAngle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
            //if (throwAngle < 0.0f) throwAngle += 360.0f;
            //Debug.Log("1) " + throwAngle);


            ThrowStrength = Mathf.PingPong(Time.time * 3, mainController.selectedPotion.Case.MaxStrength);
            //Debug.Log(ThrowStrength);
        }
    }

    public void PrepareThrow()
    {
        if (mainController.selectedPotion == null)
        {
            mainController.selectedPotion = mainController.CurrentPotions[0];
            
        }
        ThrowStrength = 0;
        mainController.selectedPotion.gameObject.SetActive(true);
        mainController.selectedPotion.transform.SetParent(ThrowTransform);
        mainController.selectedPotion.transform.position = ThrowTransform.position;

        mainController.DisableAllOtherControllers(this);
    }

    public void Throw()
    {
        mainController.selectedPotion.Launch(ThrowStrength, throwAngle);
        mainController.CurrentPotions.Remove(mainController.selectedPotion);
        mainController.selectedPotion = null;
        mainController.EnableAllControllers();
    }
}
