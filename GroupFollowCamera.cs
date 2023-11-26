using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupFollowCamera : MonoBehaviour
{
    public Launcher launcher;
    public Rocket[] rockets;
    public float followSpeed = 1f;

    public float minZDist = 35f;

    public float offset = 10f;

    public float zCeiling = 1000f;

    public Camera subCamera;

    public Vector3 PredictPos(Rocket rocket) {
        Vector3 _new = rocket.transform.position + rocket.rigidbody.velocity;
        return new Vector3(_new.x, Mathf.Clamp(_new.y, 0f, 1200), _new.z);
    }

    public void Update(){
        if(Game.stage == Game.Stage.LAUNCH || Game.stage == Game.Stage.TRAJECTORY){
            Rocket leftmost = null;
            Rocket rightmost = null;
            Rocket bottommost = null;
            Rocket topmost = null;;
            
            foreach(Rocket rocket in rockets){
                if(leftmost == null || PredictPos(rocket).x < PredictPos(leftmost).x)
                    leftmost = rocket; 
                if(rightmost == null || PredictPos(rocket).x > PredictPos(rightmost).x)
                    rightmost = rocket;
                if(bottommost == null || PredictPos(rocket).y < PredictPos(bottommost).y)
                    bottommost = rocket; 
                if(topmost == null || PredictPos(rocket).y > PredictPos(topmost).y)
                    topmost = rocket;
            }


            // Debug.Log($"{leftmost.name}, {rightmost.name}");

            float offsetZ = Mathf.Min(Mathf.Max(Mathf.Max(rightmost.transform.position.x - leftmost.transform.position.x, topmost.transform.position.y - bottommost.transform.position.y), minZDist), zCeiling); 
            
                // Mathf.Max(Mathf.Abs(((PredictPos(leftmost) + PredictPos(rightmost)) / 2f).magnitude) + offset,
                // Mathf.Max(Mathf.Abs(((PredictPos(bottommost) + PredictPos(topmost)) / 2f).magnitude) + offset, minZDist));

            transform.position = Vector3.Lerp(transform.position, new Vector3((PredictPos(leftmost).x + PredictPos(rightmost).x)/2f, (PredictPos(bottommost).y + PredictPos(topmost).y)/2f, 0f), Time.deltaTime * followSpeed);
            if(subCamera.transform.localPosition != new Vector3(0f, 0f, -offsetZ))
                subCamera.transform.localPosition = Vector3.Lerp(subCamera.transform.localPosition, new Vector3(0f, 0f, -offsetZ), Time.deltaTime * followSpeed);
        }else{
            transform.position = Vector3.Lerp(transform.position, new Vector3(0f, 7f, 0f), Time.deltaTime * followSpeed);
        }
    }


}
