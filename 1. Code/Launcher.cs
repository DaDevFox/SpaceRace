using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Launcher : MonoBehaviour
{
    public static Vector3 defaultVelocity = new Vector3(0f, 5f, 0f);

    public List<MeshRenderer> coloredMeshes = new List<MeshRenderer>();
    public List<MeshRenderer> meshes = new List<MeshRenderer>();
    public int player;

    public Rocket rocket;

    public float rocketOffset = 7.5f;

    public void Init(int player){
        
        this.player = player;
        foreach(MeshRenderer mesh in coloredMeshes){
            mesh.material.color = Game.game.players[player].color;
        }

        rocket.transform.position = new Vector3(transform.position.x, transform.position.y + rocketOffset, transform.position.z);
    }

    public void Fire(){
        // Ray ray = Game.game.mainCamera.camera.ScreenPointToRay(Input.mousePosition);
        // Vector3 dir = Game.game.currPlayer.launcher.transform.position - Game.game.mainCamera.camera.transform.position;
        // Plane plane = new Plane(dir, Game.game.currPlayer.launcher.transform.position);
        // plane.Raycast(ray, out float distance);
        // Vector3 point = ray.GetPoint(distance);

        // // GameObject instance = GameObject.Instantiate(bulletPrefab, Game.game.currPlayer.launcher.transform.position + tankBulletOffset + Vector3.ClampMagnitude(point, dynamicBulletOffsetScale), Quaternion.LookRotation(point - Game.game.currPlayer.launcher.transform.position));
        // rocket.GetComponent<Rocket>().velocity = point - rocket.transform.position;

        rocket.Fire(defaultVelocity);
    }

    

}
