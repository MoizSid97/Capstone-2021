using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    class Bullet
    {
        public float time;
        public Vector3 initialPosition;
        public Vector3 initialVelocity;
        public TrailRenderer tracer;
        public int bounce;
    }

    //Weapon Stats
    public bool isFiring = false;
    public float bulletDMG = 15f;
    public float bulletSpeed = 1000.0f;
    public float bulletDrop = 0.0f;
    public float bulletImpactForceAmount = 1.0f;
    public int fireRate = 25;
    public int maxBounce = 0;
    public int ammoCount;
    public int clipSize;

    //Audio
    public AudioSource gunFX;

    //Particles
    public ParticleSystem[] muzzleFlash;
    public ParticleSystem hitEffect;
    public TrailRenderer tracerEffect;

    //Transforms
    public Transform raycastOrigin;
    public Transform raycastDestination;

    //Gameobjects
    public GameObject muzzleLight;
    public GameObject magazine;

    //Animations
    //public AnimationClip weaponAnimation;
    public string weaponName;

    //Scripts
    public ActiveWeapon.WeaponSlot multipleWeapons;
    public WeaponRecoil accessRecoilScript;
    public ReloadWeapon accessReloadScript;

    //Layermask
    public LayerMask layerMask;

    //Privates
    Ray ray;
    RaycastHit hitInfo;
    float accumulatedTime;
    float maxLifeTime = 3.0f;
    List<Bullet> bullets = new List<Bullet>();
    float nextFire;

    private void Awake()
    {
        accessRecoilScript = GetComponent<WeaponRecoil>();
    }

    Vector3 GetPosition(Bullet bullet)
    {
        //intial position of bullet plus velocity times time + gravity times time
        Vector3 gravity = Vector3.down * bulletDrop;
        return (bullet.initialPosition) + (bullet.initialVelocity * bullet.time) + (0.5f * gravity * bullet.time * bullet.time);
    }

    Bullet CreateBullet(Vector3 position, Vector3 velocity)
    {
        Bullet bullet = new Bullet();
        bullet.initialPosition = position;
        bullet.initialVelocity = velocity;
        bullet.time = 0.0f;
        //Spawn tracer effect
        bullet.tracer = Instantiate(tracerEffect, position, Quaternion.identity);
        bullet.tracer.AddPosition(position);
        //Add crazy bullet bounce (fun effect)
        bullet.bounce = maxBounce;

        return bullet;
    }

    public void StartFiring()
    {
        //Weapon is being fired
        isFiring = true;
        //accumulatedTime = 0.0f;

        //accessRecoilScript.Reset();

        //FireBullet();

        if(Time.time > nextFire && ammoCount >= 0)
        {
            FireBullet();
            nextFire = Time.time + fireRate;
            //ammoCount--;
        }
    }

    public void UpdateFiring(float deltatime)
    {
        accumulatedTime += deltatime;

        //Time between each bullet
        float fireInterval = 1.0f / fireRate;

        while(accumulatedTime >= 0.0f)
        {
            FireBullet();
            accumulatedTime -= fireInterval;
        }
    }

    public void UpdateBullets(float deltatime)
    {
        SimulateBullets(deltatime);
        DestroyBullets();
    }

    void SimulateBullets(float deltatime)
    {
        bullets.ForEach(bullet =>
            {
                Vector3 p0 = GetPosition(bullet);
                bullet.time += deltatime;
                Vector3 p1 = GetPosition(bullet);
                RaycastSegment(p0, p1, bullet);
            });
    }

    void DestroyBullets()
    {
        bullets.RemoveAll(bullet => bullet.time >= maxLifeTime);
    }

    void RaycastSegment(Vector3 start, Vector3 end, Bullet bullet)
    {
        Vector3 direction = end - start;
        float distance = direction.magnitude;

        ray.origin = start;
        ray.direction = direction;

        //check if we hit something
        if (Physics.Raycast(ray, out hitInfo, distance, layerMask))
        {
            //Debug.DrawLine(ray.origin, hitInfo.point, Color.red, 1.0f);

            //Spawn hit effect on walls and objects
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Emit(1);

            bullet.tracer.transform.position = hitInfo.point;
            bullet.time = maxLifeTime;

            //Add collision to bullets 
            var bulletRB = hitInfo.collider.GetComponent<Rigidbody>();
            if(bulletRB)
            {
                bulletRB.AddForceAtPosition(ray.direction * bulletImpactForceAmount, hitInfo.point, ForceMode.Impulse);
            }

            //Bullet ricochet
            if(bullet.bounce > 0)
            {
                bullet.time = 0;
                bullet.initialPosition = hitInfo.point;
                bullet.initialVelocity = Vector3.Reflect(bullet.initialVelocity, hitInfo.normal);
                bullet.bounce--;
            }

            //Access AIHitBox script and deal damage
            var hitBox = hitInfo.collider.GetComponent<AIHitbox>();
            if (hitBox)
            {
                hitBox.OnRayCastHit(this, ray.direction);
            }

            //Old script for doing damage to AI
            /*BasicAI enemy = hitInfo.transform.GetComponent<BasicAI>();
            if(enemy != null)
            {
                enemy.TakeDamage(bulletDMG);
            }*/
        }

        else
        {
           bullet.tracer.transform.position = end;
        }

        //bullet.tracer.transform.position = end;
    }

    private void FireBullet()
    {
        gunFX.Play();

        if (ammoCount <= 0)
        {
            return;
        }
        ammoCount--;

        //Spawn muzzle flash particle system
        foreach (var particle in muzzleFlash)
        {
            particle.Emit(1);
            muzzleLight.SetActive(true);
        }

        Vector3 velocity = (raycastDestination.position - raycastOrigin.position).normalized * bulletSpeed;
        var bullet = CreateBullet(raycastOrigin.position, velocity);
        bullets.Add(bullet);

        //Do recoil stuff
        accessRecoilScript.GenerateRecoil(weaponName);
    }

    public void StopFiring()
    {
        //Weapon no longer being fired
        isFiring = false;
        muzzleLight.SetActive(false);
    }
}
