﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    float maxVelocityChange = 10.0f;
    bool canJump = true;
    float jumpHeight = 2.0f;
    float jumpInterval = 1.5f;
    private float nextJump = 1.2f;

    public float speed = 4;
    public float runSpeed = 8;
    private float moveAmount;
    float smoothSpeed = 2;

    public float gravity = 25;
    public float rotateSpeed = 8.0f;
    public float dampTime = 3;
    public LayerMask mask;
    public float downCastRange = 1.2f;
    private float horizontalSpeed;

    bool grounded;
    Transform target;
    Transform chest;
    private Vector3 velocityChange;

    private bool running = false;

    private float originalSpeed;

    private Vector3 forward = Vector3.forward;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 right;
    private bool canRun = true;
    //private bool canJump = false;
    private bool isJumping = false;

    private bool useGravity = true;
    Transform shield;
    Transform weapon;
    Transform lefthandpos;
    Transform righthandpos;
    Transform chestposshield;
    Transform chestposweapon;
    Camera mainCamera;
    private bool fightmodus = false;
    private bool didselect = false;
    private bool canattack = true;
    public enum RotationAxes { MouseXAndY = 0, MouseX=1, MouseY=2};
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 15f;
    public float sensitivityY = 15f;
    public float minimumX = -360f;
    public float maximumX = 360f;
    public float minimumY = -60f;
    public float maximumY = 60f;
    public float rotationY = 0f;
    Vector3 addvector;
    private BetterFootSounds footSounds;

    void Awake()
    {
        GetComponent<Rigidbody>().freezeRotation = true;
        GetComponent<Rigidbody>().useGravity = false;
        footSounds = this.GetComponent<BetterFootSounds>();
    }

    void Start()
    {
        originalSpeed = speed;
        mainCamera = this.transform.FindChild("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            mainCamera.transform.localEulerAngles = new Vector3(-rotationY, mainCamera.transform.rotation.x, 0);
            transform.localEulerAngles = new Vector3(transform.rotation.y, rotationX, 0);
        }
        //else if (axes == RotationAxes.MouseX)
        //{
        //    mainCamera.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        //}
        //else
        //{
        //    rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        //    rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        //    mainCamera.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
        //}
    }

    void FixedUpdate()
    {
        Animator animator = GetComponent<Animator>();
        forward = mainCamera.transform.forward;
        right = new Vector3(forward.z, 0, -forward.x);
        float hor = Input.GetAxis("Horizontal");
        float ver = Input.GetAxis("Vertical");
        Vector3 targetDirection = (hor * right) + (ver * forward);
        targetDirection = targetDirection.normalized;

        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        float z = GetComponent<Rigidbody>().velocity.z;
        float x = GetComponent<Rigidbody>().velocity.x;
        Vector3 currentmagnitude = new Vector3(x, 0, z);
        Vector3 localmagnitude = transform.InverseTransformDirection(currentmagnitude);

        if (fightmodus)
        {
            var localTarget = transform.InverseTransformPoint(target.position);
            var addfloat = (Mathf.Atan2(localTarget.x, localTarget.z));
            canRun = false;
            var relativePos = target.transform.position - transform.position;
            var lookrotation = Quaternion.LookRotation(relativePos, Vector3.up);
            lookrotation.x = 0;
            lookrotation.z = 0;
            animator.SetFloat("hor", (localmagnitude.x) + (addfloat * 2), dampTime, 0.8f);
            animator.SetFloat("ver", (localmagnitude.z), dampTime, 0.8f);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookrotation, Time.deltaTime * rotateSpeed);
        }
        else
        {
            canRun = true;
            if (targetDirection != Vector3.zero)
            {
                var lookrotation2 = Quaternion.LookRotation(targetDirection, Vector3.up);
                lookrotation2.x = 0;
                lookrotation2.z = 0;
                //transform.rotation = Quaternion.Lerp(transform.rotation,lookrotation2,Time.deltaTime * rotateSpeed);
            }
        }
        if (grounded)
        {

            var velocityanim = Mathf.Clamp01(currentmagnitude.magnitude);

            var targetVelocity = targetDirection;


            if (Input.GetButton("Fire2") && canRun && !isJumping && ver > 0)
            {
                targetVelocity *= runSpeed;
                velocityanim *= 2;
                footSounds.Sprint(true);

            }
            else
            {
                targetVelocity *= speed;
                velocityanim *= 1;
                footSounds.Sprint(false);
            }



            velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);


            animator.SetFloat("speed", velocityanim, dampTime, 0.2f);

            if (Input.GetButton("Jump") && Time.time > nextJump)
            {
                nextJump = Time.time + jumpInterval;
                isJumping = true;
                //myaudiosource.clip = jumpclip;
                //myaudiosource.loop = false;
                //myaudiosource.pitch = 1;
                //myaudiosource.Play();
                GetComponent<Rigidbody>().velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
                animator.SetBool("jump", true);
            }
            else
            {
                animator.SetBool("jump", false);
                isJumping = false;

            }
        }

        animator.SetBool("grounded", grounded);

        GetComponent<Rigidbody>().AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

    }

    void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
            grounded = true;
        else
        {
            grounded = false;
        }
    }

    float CalculateJumpVerticalSpeed()
    {
        // From the jump height and gravity we deduce the upwards speed 
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }
    //function grabshield()
    //{
    //    shield.parent = lefthandpos;
    //    shield.position = lefthandpos.position;
    //    shield.rotation = lefthandpos.rotation;
    //    fightmodus = true;
    //    myaudiosource.clip = equip2sound;
    //    myaudiosource.loop = false;
    //    myaudiosource.pitch = 0.9 + 0.2 * Random.value;
    //    myaudiosource.Play();
    //}
    //function grabweapon()
    //{
    //    weapon.parent = righthandpos;
    //    weapon.position = righthandpos.position;
    //    weapon.rotation = righthandpos.rotation;
    //    myaudiosource.clip = equip1sound;
    //    myaudiosource.loop = false;
    //    myaudiosource.pitch = 0.9 + 0.2 * Random.value;
    //    myaudiosource.Play();


    //}
    //function holstershield()
    //{
    //    shield.parent = chestposshield;
    //    shield.position = chestposshield.position;
    //    shield.rotation = chestposshield.rotation;
    //    myaudiosource.clip = holster1sound;
    //    myaudiosource.loop = false;
    //    myaudiosource.pitch = 0.9 + 0.2 * Random.value;
    //    myaudiosource.Play();

    //}
    //function holsterweapon()
    //{
    //    fightmodus = false;
    //    weapon.parent = chestposweapon;
    //    weapon.position = chestposweapon.position;
    //    weapon.rotation = chestposweapon.rotation;
    //    myaudiosource.clip = holster2sound;
    //    myaudiosource.loop = false;
    //    myaudiosource.pitch = 0.9 + 0.2 * Random.value;
    //    myaudiosource.Play();
    //}
    //function weaponselect()
    //{
    //    canattack = false;
    //    var animator = GetComponent(Animator);

    //    yield WaitForSeconds(0.2);

    //    if (!didselect)
    //    {
    //        animator.SetBool("grabweapon", true);
    //        yield WaitForSeconds(2);
    //        didselect = true;
    //    }
    //    else
    //    {

    //        animator.SetBool("grabweapon", false);
    //        yield WaitForSeconds(2);
    //        didselect = false;
    //    }
    //    canattack = true;


    //}
}

