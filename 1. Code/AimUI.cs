using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimUI : MonoBehaviour
{
    public RectTransform selectionBox;
    public RectTransform targettingArrow;

    public bool aiming = true;

    public float thickness = 1f;

    Vector3 p1 = Vector3.zero;
    Vector3 p2 = Vector3.zero;

    public float maxTargettingLength = 10f;

    public GameObject bulletPrefab;

    public Vector3 tankBulletOffset = Vector3.zero;
    public float dynamicBulletOffsetScale = 1f;

    void Update()
    {
        Vector3 screenPos = Game.game.mainCamera.camera.WorldToScreenPoint(Game.game.currPlayer.launcher.transform.position);
        selectionBox.position = screenPos;

        if(aiming){
            Vector3 diff = Vector3.ClampMagnitude(Input.mousePosition - screenPos, maxTargettingLength);
            targettingArrow.position = screenPos + diff;

            if(MainInputController.FireDown){
                Fire();


            }
        }
    }

    public void Fire(){
        Ray ray = Game.game.mainCamera.camera.ScreenPointToRay(Input.mousePosition);
        Vector3 dir = Vector2.Perpendicular(Game.game.currPlayer.launcher.transform.position.xz() - Game.game.players[Game.game.focusingPlayer].launcher.transform.position.xz()).fromXZ();
        Plane plane = new Plane(dir, Game.game.currPlayer.launcher.transform.position);
        plane.Raycast(ray, out float distance);
        Vector3 point = ray.GetPoint(distance);

        GameObject instance = GameObject.Instantiate(bulletPrefab, Game.game.currPlayer.launcher.transform.position + tankBulletOffset + Vector3.ClampMagnitude(point, dynamicBulletOffsetScale), Quaternion.LookRotation(point - Game.game.currPlayer.launcher.transform.position));
        instance.GetComponent<Rocket>().velocity = point - instance.transform.position;
    }
}
