#pragma strict


var maxVelocityChange = 10.0;
var canJump = true;
var jumpHeight = 2.0;
var jumpinterval : float = 1.5;
private var nextjump : float = 1.2;

public var speed : float = 4;
var runspeed : float = 8;
private var moveAmount : float;
var smoothSpeed : float = 2;
//private var sensitivityX : float = 6;


public var gravity : float = 25;
public var rotateSpeed : float = 8.0;
public var dampTime : float = 3;
public var mask : LayerMask;
public var downcastrange : float =1.2;
private var horizontalSpeed : float;

var grounded : boolean;
var myaudiosource : AudioSource;
var target : Transform;
var chest : Transform;
var jumpclip : AudioClip;
private var velocityChange : Vector3;

private var running : boolean = false;

private var originalSpeed : float;

private var forward : Vector3 = Vector3.forward;
private var moveDirection : Vector3 = Vector3.zero;
private var right : Vector3;
private var canrun : boolean = true;
private var canjump : boolean = false;
private var isjumping : boolean = false;

private var usegravity : boolean = true;
var shield : Transform;
var weapon : Transform;
var lefthandpos : Transform;
var righthandpos : Transform;
var chestposshield : Transform;
var chestposweapon : Transform;
var equip1sound : AudioClip;
var equip2sound : AudioClip;
var holster1sound : AudioClip;
var holster2sound : AudioClip;
var mainCamera : Camera;
private var fightmodus : boolean = false;
private var didselect : boolean = false;
private var canattack : boolean = true;
enum RotationAxes {MouseXAndY = 0, MouseX=1, MouseY=2};
public var axes : RotationAxes = RotationAxes.MouseXAndY;
public var sensitivityX : float = 15F;
public var sensitivityY : float = 15F;
public var minimumX : float = -360F;
public var maximumX : float = 360F;
public var minimumY : float = -60F;
public var maximumY : float = 60F;
public var rotationY : float = 0F;
var addvector : Vector3;

function Awake()
{
 	GetComponent.<Rigidbody>().freezeRotation = true;
	GetComponent.<Rigidbody>().useGravity = false;
    
}
function Start()
{
     originalSpeed = speed;
    
}

function Update()
{
	if (axes == RotationAxes.MouseXAndY)
     {
         var rotationX : float = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
         
         rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
         rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
         
         mainCamera.transform.localEulerAngles = new Vector3(-rotationY, mainCamera.transform.rotation.x, 0);
		 transform.localEulerAngles = new Vector3(transform.rotation.y, rotationX, 0);
     }
     else if (axes == RotationAxes.MouseX)
     {
         mainCamera.transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
     }
     else
     {
         rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
         rotationY = Mathf.Clamp (rotationY, minimumY, maximumY);
         
         mainCamera.transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
     }
 }

function FixedUpdate()
{ 	 
 	 var animator = GetComponent(Animator);
 	 forward = mainCamera.main.transform.forward;
	 right = new Vector3(forward.z, 0, -forward.x);
	 var hor = Input.GetAxis("Horizontal");
	 var ver = Input.GetAxis("Vertical");
	 var targetDirection : Vector3 = (hor * right) + (ver * forward);
	 targetDirection = targetDirection.normalized;
	
	 var velocity = GetComponent.<Rigidbody>().velocity;
	 var z = GetComponent.<Rigidbody>().velocity.z;
	 var x = GetComponent.<Rigidbody>().velocity.x;
	 var currentmagnitude = new Vector3(x,0,z);
	 var localmagnitude = transform.InverseTransformDirection(currentmagnitude);
	 if (fightmodus)
	 {
		
		
		var localTarget = transform.InverseTransformPoint(target.position);
		var addfloat = (Mathf.Atan2(localTarget.x, localTarget.z));
		
		canrun = false;
		
		var relativePos = target.transform.position - transform.position;
 	 	var lookrotation = Quaternion.LookRotation(relativePos,Vector3.up);
 	 	lookrotation.x = 0;
 	 	lookrotation.z = 0;
 	 	animator.SetFloat("hor",(localmagnitude.x) + (addfloat * 2), dampTime , 0.8);
	 	animator.SetFloat("ver",(localmagnitude.z), dampTime , 0.8);
	 	transform.rotation = Quaternion.Lerp(transform.rotation,lookrotation,Time.deltaTime * rotateSpeed);
	 	
	 	
	 }
	 else
	 {
		
		canrun = true;
		
		if (targetDirection != Vector3.zero)
		{
			var lookrotation2 = Quaternion.LookRotation(targetDirection, Vector3.up);
			lookrotation2.x = 0;
 	 		lookrotation2.z = 0;			
			//transform.rotation = Quaternion.Lerp(transform.rotation,lookrotation2,Time.deltaTime * rotateSpeed);
		}
	 }
	
	 if(grounded)
     {
     	
		var velocityanim = Mathf.Clamp01(currentmagnitude.magnitude);
		
		var targetVelocity = targetDirection;
		
		
		if (Input.GetButton("Fire2") && canrun && !isjumping && ver > 0)
		{
			targetVelocity *= runspeed;
			velocityanim *= 2;
			
			
		}
		else
		{
			targetVelocity *= speed;
			velocityanim *= 1;
		}
		
		
		
		velocityChange = (targetVelocity - velocity);
		velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
	 	velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
		velocityChange.y = 0;
	 	GetComponent.<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
	 	
		
		animator.SetFloat("speed",velocityanim,dampTime, 0.2);
		
		if (Input.GetButton("Jump") && Time.time > nextjump)
		{
			nextjump = Time.time + jumpinterval;
			isjumping = true;
			myaudiosource.clip = jumpclip;
			myaudiosource.loop = false;
			myaudiosource.pitch = 1;
			myaudiosource.Play();
			GetComponent.<Rigidbody>().velocity = Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
			animator.SetBool("jump",true);
		}
		else
     	{
        	 animator.SetBool("jump",false);
        	 isjumping = false;
         	
     	}   
	}
 
	 animator.SetBool("grounded",grounded);	 
     
     GetComponent.<Rigidbody>().AddForce(Vector3 (0, -gravity * GetComponent.<Rigidbody>().mass, 0));
     
	
     testground(); 
     
 }
 
function testground ()
{
             
      yield WaitForSeconds(0.5f);
      var hit : RaycastHit;
      
      if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, hit, downcastrange , mask ))
      {
           grounded = true;
      }      
      else
      {
           grounded = false;
      }
          
          
}

function CalculateJumpVerticalSpeed ()
{
	// From the jump height and gravity we deduce the upwards speed 
	// for the character to reach at the apex.
	return Mathf.Sqrt(2 * jumpHeight * gravity);
}
function grabshield()
{
	shield.parent = lefthandpos;
	shield.position = lefthandpos.position;
	shield.rotation = lefthandpos.rotation;
	fightmodus = true;
	myaudiosource.clip = equip2sound;
	myaudiosource.loop = false;
	myaudiosource.pitch = 0.9 + 0.2*Random.value;
	myaudiosource.Play();
}
function grabweapon()
{
	weapon.parent = righthandpos;
	weapon.position = righthandpos.position;
	weapon.rotation = righthandpos.rotation;
	myaudiosource.clip = equip1sound;
	myaudiosource.loop = false;
	myaudiosource.pitch = 0.9 + 0.2*Random.value;
	myaudiosource.Play();
	
	
}
function holstershield()
{
	shield.parent = chestposshield;
	shield.position = chestposshield.position;
	shield.rotation = chestposshield.rotation;
	myaudiosource.clip = holster1sound;
	myaudiosource.loop = false;
	myaudiosource.pitch = 0.9 + 0.2*Random.value;
	myaudiosource.Play();
	
}
function holsterweapon()
{
	fightmodus = false;
	weapon.parent = chestposweapon;
	weapon.position = chestposweapon.position;
	weapon.rotation = chestposweapon.rotation;
	myaudiosource.clip = holster2sound;
	myaudiosource.loop = false;
	myaudiosource.pitch = 0.9 + 0.2*Random.value;
	myaudiosource.Play();
}
function weaponselect()
{	
	canattack = false;
	var animator = GetComponent(Animator);
	
	yield WaitForSeconds(0.2);
	
	if (!didselect)
	{
		animator.SetBool("grabweapon",true);
		yield WaitForSeconds(2);
		didselect = true;
	}
	else
	{
		
		animator.SetBool("grabweapon",false);
		yield WaitForSeconds(2);
		didselect = false;
	}
	canattack = true;
	
	
}