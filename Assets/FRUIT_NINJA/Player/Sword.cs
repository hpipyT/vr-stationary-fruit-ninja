using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class Sword : MonoBehaviour
{

    public AudioSource squelch1;
    public AudioSource squelch2;
    public AudioSource squelch3;
    public AudioSource noSquelch;

    // one handed Wakizashi

    // two handed Katana
    Vector3 swordScale;

    InputDevice left;
    public InputDevice right;

    PlayerGear gear;

    public bool isThrusting = false;

    // Start is called before the first frame update
    void Start()
    {
        gear = GameObject.Find("Player").GetComponent<PlayerGear>();
        swordScale = gameObject.transform.GetChild(0).localScale;
    }

    // Update is called once per frame
    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode (XRNode.RightHand);

        

        if (IsStacked(left, right) && gear.starInHand == false && gear.grappleFired == false)
        {
            //Debug.Log("Power stance");

            

            gameObject.transform.GetChild(0).localScale = new Vector3(swordScale.x *3.0f, swordScale.y * 3.0f, swordScale.z * 1.5f);
        }
        else
        {
            
            gameObject.transform.GetChild(0).localScale = new Vector3(swordScale.x, swordScale.y, swordScale.z);
        }

        isThrusting = false;
/*        if (IsThrusting(right))
        {
            isThrusting = true;
            // play sound
        }*/

        // IsChopping(right);

        // IsParallel(left, right);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = GameObject.FindObjectOfType<Player>();

        if (right.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 vel))
        {
            if (vel.magnitude < 1.1f)
                return;
        }

        // grapefruit thrust
        if (IsThrusting(right) && other.name == "Sliced_grapefruit" || other.name == "Sliced_grapefruit(clone)")
        {
            Debug.Log("Stabbed something");
            Debug.Log(other.name);
            if (other.gameObject.GetComponent<GrapefruitSlice>())
            {
                Debug.Log("Thrusted fruit");
                squelch3.Play();
                other.GetComponent<Fruit>().TakeDamage(10);
            }
            return;
        }
            

        
        if (other.gameObject.GetComponent<Fruit>())
        {
            if (other.name == "Sliced_grapefruit" || other.name == "Sliced_grapefruit(clone)")
            {
                noSquelch.Play();
                return;
            }

            if (other.name == "watermelon" || other.name == "watermelon(clone)")
            {
                if (IsStacked(left, right) && gear.starInHand == false && gear.grappleFired == false)
                {
                    other.GetComponent<Fruit>().TakeDamage(10);
                    squelch1.Play();
                }
                noSquelch.Play();
                return;
            }

            other.GetComponent<Fruit>().TakeDamage(10);
            player.counterSeconds -= other.GetComponent<Fruit>().xp * 100;
            Debug.Log("Timer shaved off seconds: " + other.GetComponent<Fruit>().xp);
            Debug.Log("New time: " + player.counterSeconds);
            squelch2.Play();

        }
    }

    public bool IsThrusting(InputDevice device)
    {
        Vector3 velocity = DeviceVelocity(device);

        if (IsTopForward(device) && velocity.z > 1.0f && velocity.x < 1.0f && velocity.y < 1.0f)
        {
            Debug.Log("Thrusting! Kya! ");
            return true;
        }

        

        return false;
    }

    public Vector3 DeviceVelocity(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 vel))
        {
            //Debug.Log(vel);
            return vel;
        }

        return new Vector3(0,0,0);
    }

    public bool IsTopForward(InputDevice device)
    {
        float allowance = 45.0f;

        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            //Quaternion deviceOffset = Quaternion.AngleAxis(90, Vector3.up);


            // Quest controller's up seems to be normal to its spine
            // forward vector is therefore its up when held vertically
            Vector3 up = rot * Vector3.forward;

            float degreesFromUp = Vector3.Angle(up, Vector3.forward);

            // float degreesFromUp = Vector3.Angle(up, device.gameObject.transform.position.forward);

            if (degreesFromUp < allowance)
                return true;

        }
        return false;
    }


    // checks if right device is on top of left
    public bool IsStacked(InputDevice left, InputDevice right)
    {

        Vector3 leftPos = GetDevicePosition(left);
        Vector3 rightPos = GetDevicePosition(right);

/*        Debug.Log(leftPos);
        Debug.Log(rightPos);*/

        float leftX = leftPos.x;
        float rightX = rightPos.x;
        float xDist = 0.2f;

        float leftY = leftPos.y;
        float rightY = rightPos.y;
        float yDist = 0.20f;

        float leftZ = leftPos.z;
        float rightZ = rightPos.z;
        float zDist = 0.2f;

        // if right is > than left
        // // and distance not > 0.2
        // if x's ~= the same
        // if z's ~= the same

        // devices are stacked
        if (leftY < rightY && rightY - leftY < yDist && Mathf.Abs(rightX - leftX) < xDist && Mathf.Abs(rightZ - leftZ) < zDist)
        {
            //Debug.Log("Stacked");
            return true;
        }

        return false;
    }


    public Vector3 GetDevicePosition(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePos))
        {
            return devicePos;
        }

        return new Vector3(0, 0, 0);
    }

    public bool IsParallel(InputDevice left, InputDevice right)
    {
        Quaternion leftRot = GetDeviceRotation(left);
        Quaternion rightRot = GetDeviceRotation(right);

        float allowance = 25.0f;

            //Quaternion deviceOffset = Quaternion.AngleAxis(90, Vector3.up);


            // Quest controller's up seems to be normal to its spine
            // forward vector is therefore its up when held vertically
            Vector3 leftUp = leftRot * Vector3.forward;
            Vector3 rightUp = rightRot * Vector3.forward;

            float degreesFromUp = Vector3.Angle(leftUp, rightUp);

        // float degreesFromUp = Vector3.Angle(up, device.gameObject.transform.position.forward);

        if (degreesFromUp < allowance)
        {
            //Debug.Log("Parallel");
            return true;
        }


        return false;
    }

    public Quaternion GetDeviceRotation(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            return rot;
        }
        return Quaternion.identity;
    }


}
