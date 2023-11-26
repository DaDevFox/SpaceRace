using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions{
    public static Vector2 xz(this Vector3 vector){
        return new Vector2(vector.x, vector.z);
    }

    public static Vector3 fromXZ(this Vector2 vector){
        return new Vector3(vector.x, 0f, vector.y);
    }
}


public class ComparerCamera : MonoBehaviour
{
    public Camera camera;

    public List<MeshRenderer[]> playerObjs = new List<MeshRenderer[]>();

    /// <summary>
    /// Sets camera to cross-sectional view if not -1, between target and current turn tank. 
    /// </summary>
    public int target = -1;

    public Vector3 restingPos = new Vector3(0, 30, 0);
    public Quaternion restingRot = Quaternion.Euler(90, 0, 0);
    public float zoomedOffsetMultiple = 1f;
    public Vector3 zoomedOffset = new Vector3(0f, 15f, 0f);

    public float posTransitionSpeed = 1f;

    
    public void Update(){
        if(target == -1){
            transform.position = Vector3.Lerp(transform.position, restingPos, Time.deltaTime * posTransitionSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, restingRot, Time.deltaTime * posTransitionSpeed);
        }else{
            transform.rotation = Quaternion.Lerp(transform.rotation, CalcCrossSectionEulerRotation(), Time.deltaTime * posTransitionSpeed);
            transform.position = Vector3.Lerp(transform.position, CalcCrossSectionGlobalObservationPos(), Time.deltaTime * posTransitionSpeed);
        }
    }

    public Quaternion CalcCrossSectionEulerRotation(){
        Vector3 dir = Vector2.Perpendicular(Game.game.currPlayer.launcher.transform.position.xz() - Game.game.players[target].launcher.transform.position.xz()).fromXZ();
        return Quaternion.LookRotation(dir);
    }

    public Vector3 CalcCrossSectionGlobalObservationPos(){
        return (Game.game.currPlayer.launcher.transform.position + Game.game.players[target].launcher.transform.position)/2f * zoomedOffsetMultiple + zoomedOffset;
    }


}
