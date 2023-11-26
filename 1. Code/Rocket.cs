using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public static float fuel0 = 40;
    public static float fuel1 = 60;
    public static float fuel2 = 120f;
    public static float fuel3 = 175f;

    public static float thrust0 = 12.5f;
    public static float thrust1 = 15f;
    public static float thrust2 = 20f;
    public static float thrust3 = 30f;

    public static float dynamicVar0 = 0.4f; // 0.1
    public static float dynamicVelVar0 = 0.15f; // 0.15

    public static float dynamicVar1 = 0.2f; // 0.04
    public static float dynamicVelVar1 = 0.07f; // 0.1

    public static float dynamicVar2 = 0.08f; // 0.025
    public static float dynamicVelVar2 = 0.05f; // 0.05

    public static float courseCorrection0 = 0f;
    public static float courseCorrection1 = 0.5f;
    public static float courseCorrection2 = 0.9f;

    [Header("Settings")]
    public float damageVelocityThreshold = 1f;

    public Vector3 velocity;
    private Vector3 initialVelocity;

    public float aerodynamicVariation = 1f;
    public float aerodynamicVelocityMultiplier = 0.2f;

    public float rotation = 0f;
    private float initialRotation = 0f;
    public float courseCorrection = 0f;
    public float visualRotationSpeed = 1f;


    public float thrust = 1f;
    private float thrustAccumulateY = 0f;
    private float thrustAccumulateX = 0f;

    public float postCollisionThrustModifier = 0.1f;
    public float postCollisionThrustVariance = 0.1f;
    
    private float gravityAccumulate = 0f;
    public float gravity = 0.098f;
    
    public float airDrag = 0.1f;

    public float fuel = 10f;
    public float maxFuel { get; set;} = 0f;

    
    public bool controlledFlight = false;
    public float maxHeight = 0f;
    public float maxHeightTotal = 0f;
 
    
    [Header("Scene")]
    public Rigidbody rigidbody;
    public ParticleSystem[] thrustParticles;
    public List<MeshRenderer> bodyMeshes;
    public List<MeshRenderer> coloredMeshes;
    public List<MeshRenderer> thrustEmissionMeshes;
    public Launcher launcher;

    public static string explosionParticlesPath {get; } = "2. Prefabs/Explosion Particles";

    public static string fireParticlesPath {get; } = "2. Prefabs/Fire Particles";

    private int currBonus = 0;
    private List<int> unclaimedBonuses;
    public int bonusCount {get; private set;} = 0;

    void Start(){
        unclaimedBonuses = new List<int>(Game.bonusesUnclaimed);
        Reset();
    }

    public void Reset(){
        if(maxHeight > maxHeightTotal)
            maxHeightTotal = maxHeight;
        maxHeight = 0f;

        bonusCount = 0;

        FreezeRigidbody();
        controlledFlight = false;

        foreach(MeshRenderer renderer in coloredMeshes)
            renderer.material.color = Color.white;

        transform.position = launcher.transform.position + new Vector3(0f, launcher.rocketOffset, 0f);
        transform.rotation = Quaternion.identity;

        thrustAccumulateX = 0f;
        thrustAccumulateY = 0f;
        gravityAccumulate = 0f;

        for(int i = 0; i < transform.childCount; i++)
            if(transform.GetChild(i).GetComponent<EnableWithUpgrade>())
                transform.GetChild(i).GetComponent<EnableWithUpgrade>().UpdateParts();

        if(unclaimedBonuses.Count > 0)
            currBonus = unclaimedBonuses[0];
    }

    public void FreezeRigidbody(){
        rigidbody.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
    }

    public void UnfreezeRigidbody(){
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void Fire(Vector3 velocity){
        Game.prelaunch = false;
        this.velocity = velocity;
        this.initialVelocity = velocity;

        UnfreezeRigidbody(); // reenables rigidbody physics
        this.controlledFlight = true; // enables calculated physics for one uninterrupted motion until interruption

        rotation = Mathf.Atan2(velocity.y, velocity.x); // sets heading based on velocity
        initialRotation = Mathf.Atan2(initialVelocity.y, initialVelocity.x);

        SetStats();
        
        maxFuel = fuel;

        
        foreach(MeshRenderer renderer in coloredMeshes)
            renderer.material.color = Game.game.players[launcher.player].color;
    }

    public float GravitationalAccelerationAtHeight(float height){
        return (6.6743f * 5.972f)/Mathf.Pow(6371+height, 2) * 10000000f;
    }

    public void SetStats(){
        if((Game.game.players[launcher.player].upgrades & Upgrades.FUEL_3) == Upgrades.FUEL_3)
            fuel = fuel3;
        else if((Game.game.players[launcher.player].upgrades & Upgrades.FUEL_2) == Upgrades.FUEL_2)
            fuel = fuel2;
        else if((Game.game.players[launcher.player].upgrades & Upgrades.FUEL_1) == Upgrades.FUEL_1)
            fuel = fuel1;
        else
            fuel = fuel0;

        
        if((Game.game.players[launcher.player].upgrades & Upgrades.THRUST_3) == Upgrades.THRUST_3)
            thrust = thrust3;
        else if((Game.game.players[launcher.player].upgrades & Upgrades.THRUST_2) == Upgrades.THRUST_2)
            thrust = thrust2;
        else if((Game.game.players[launcher.player].upgrades & Upgrades.THRUST_1) == Upgrades.THRUST_1)
            thrust = thrust1;
        else
            thrust = thrust0;

        if((Game.game.players[launcher.player].upgrades & Upgrades.CONTROL_2) == Upgrades.CONTROL_2)
        {
            aerodynamicVariation = dynamicVar2;
            aerodynamicVelocityMultiplier = dynamicVelVar2;
        }
        else if((Game.game.players[launcher.player].upgrades & Upgrades.CONTROL_1) == Upgrades.CONTROL_1)
        {
            aerodynamicVariation = dynamicVar1;
            aerodynamicVelocityMultiplier = dynamicVelVar1;
        }
        else
        {
            aerodynamicVariation = dynamicVar0;
            aerodynamicVelocityMultiplier = dynamicVelVar0;
        }

        if((Game.game.players[launcher.player].upgrades & Upgrades.RADIO_2) == Upgrades.RADIO_2)
            courseCorrection = courseCorrection2;
        else if((Game.game.players[launcher.player].upgrades & Upgrades.RADIO_1) == Upgrades.RADIO_1)
            courseCorrection = courseCorrection1;
        else
            courseCorrection = courseCorrection0;


        foreach(MeshRenderer mesh in coloredMeshes)
            mesh.material.color = Game.game.players[launcher.player].color;
    }

    void Update()
    {
        if(transform.position.y > maxHeight)
            maxHeight = transform.position.y;
        
        if(fuel >= 0f){
            thrustAccumulateY += Mathf.Sin(rotation) * thrust * Time.deltaTime;
            thrustAccumulateX += Mathf.Cos(rotation) * thrust * Time.deltaTime;
            fuel -= thrust * Time.deltaTime;
            
            foreach(ParticleSystem particles in thrustParticles){
                if(!particles.isPlaying)
                    particles.Play();
            }

            if(!controlledFlight && Game.postCollisionThrust){
                thrust += WindBG.WindNoise(transform.position.z, transform.position.y) * postCollisionThrustVariance;
                rigidbody.AddForce(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0f) * thrust * Time.deltaTime * postCollisionThrustModifier);
            }
        }else {
            foreach(ParticleSystem particles in thrustParticles)
                    particles.Stop();
        }

        if(controlledFlight){
            rotation += aerodynamicVariation * WindBG.WindNoise(transform.position.z, transform.position.y) * UnityEngine.Random.Range(-1f, 1f)
            * (1f + rigidbody.velocity.magnitude * aerodynamicVelocityMultiplier);
            
            rotation += Mathf.Clamp(initialRotation - rotation, -courseCorrection * Time.deltaTime, courseCorrection * Time.deltaTime);
            rotation %= 2 * Mathf.PI;
            
            rigidbody.velocity = new Vector3(thrustAccumulateX, thrustAccumulateY - gravityAccumulate, 0f);

            gravityAccumulate += gravity * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0f, 0f, rotation * Mathf.Rad2Deg - 90f), Time.deltaTime * visualRotationSpeed);

            thrustAccumulateX *= (1f - airDrag * Time.deltaTime);
        }

        if(currBonus != -10 && transform.position.y > currBonus){
            BonusUI.Create(transform.position, launcher.player, EndOfLaunchUI.creditBonuses[currBonus]);
            unclaimedBonuses.Remove(currBonus);
            bonusCount += EndOfLaunchUI.creditBonuses[currBonus];
            Game.game.players[launcher.player].currency += EndOfLaunchUI.creditBonuses[currBonus];
            currBonus = -10;
            foreach(int bonus in unclaimedBonuses){
                if(transform.position.y < bonus){
                    currBonus = bonus;
                    break;
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        controlledFlight = false;
        foreach(ContactPoint contact in collision.contacts){
            GameObject.Instantiate(Resources.Load<GameObject>(explosionParticlesPath), contact.point, Quaternion.identity);
            rigidbody.AddExplosionForce(1f, contact.point, 1f);
            if(collision.relativeVelocity.sqrMagnitude > damageVelocityThreshold * damageVelocityThreshold){
                GameObject fire = GameObject.Instantiate(Resources.Load<GameObject>(fireParticlesPath), contact.point, Quaternion.identity);
                fire.transform.SetParent(transform, true);
            }
        }

        gravityAccumulate = 0f;
        thrustAccumulateY = 0f;
        thrustAccumulateX = 0f;

        // if(collision.collider.GetComponent<HealthObj>()){
        //     collision.collider.GetComponent<HealthObj>().health -= 0.5f;
        //     if(collision.collider.GetComponent<HealthObj>().health < 0f)    
        // }
    
        // GameObject.Destroy(transform.GetChild(0).gameObject);
    }
}
