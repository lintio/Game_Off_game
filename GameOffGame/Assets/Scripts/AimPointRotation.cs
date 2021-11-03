using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPointRotation : MonoBehaviour
{
    public Transform aimTansform;

    private void Update()
    {

        Vector3 mousePostion = GetMouseWorldPosition();
        Vector3 aimDirection = (mousePostion - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        aimTansform.eulerAngles = new Vector3(0, 0, angle);

    }

    Vector3 GetMouseWorldPosition()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
