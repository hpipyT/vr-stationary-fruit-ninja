using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    GameObject sword;

    InputDevice left;
    InputDevice right;

    InputDevice headset;

    bool isJumping = false;
    bool isTouchingFruit = false;

    bool hitWall = false;


    // Start is called before the first frame update
    void Start()
    {
        sword = GameObject.Find("Player/Camera Offset/RightHand Controller/sword");
        player = GameObject.Find("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Fruit>())
        {
            isTouchingFruit = true;
            StartCoroutine(FruitTimer());
        }
        /*else
        {
            hitWall = true;
            StartCoroutine(WallTimer());
        }*/
    }


    public IEnumerator FruitTimer()
    {
        yield return new WaitForSeconds(2.0f);
        isTouchingFruit = false;
        
    }

    public IEnumerator WallTimer()
    {
        yield return new WaitForSeconds(1f);
        hitWall = false;

    }


    // Update is called once per frame
    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode(XRNode.RightHand);
        headset = InputDevices.GetDeviceAtXRNode(XRNode.Head);

        //right.TryGetFeatureValue(CommonUsages.triggerButton, out bool isPressed);

        // get device rotation

        // Debug.Log(IsTopUp(headset));


        // Ninja running

        Vector2 headsetTilt = LeanDirection(headset);

        float magnitude = Mathf.Sqrt(Mathf.Pow(headsetTilt.x, 2) + Mathf.Pow(headsetTilt.y, 2));

        // arms behind head
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (magnitude >= 0.3f && IsInRunStance(headset, left, right) && !isTouchingFruit && !isJumping && !hitWall)
        {
            // move in direction x/y;
            rb.transform.position += new Vector3(headsetTilt.x, 0, headsetTilt.y) * 0.2f;
            //player.transform.position += new Vector3(headsetTilt.x, 0, headsetTilt.y) * 0.4f;
        }

        // fly detection
        if (left.TryGetFeatureValue(CommonUsages.primaryButton, out bool buttonPressed))
        {
            if (IsJumping() && !isJumping && buttonPressed == true)
            {
                isJumping = true;
                StartCoroutine(SuperJump(5.0f, 10.0f, 0.5f));
            }
        }

        // jump detection

        if (IsJumping() && !isJumping)
        {
            isJumping = true;

            // Jump();
            Debug.Log("Player attempted to jump");

            // headset super jump

            StartCoroutine(SuperJump(0, 1.0f, 0.5f));

            // scale camera offset over time

        }

        // duck detection

        if (IsDucking() && !isJumping)
        {
            isJumping = true;

            // Jump();
            Debug.Log("Player attempted to duck");

            StartCoroutine(Slide(0, 5.0f, 0.5f));

        }





        // if jump, then splays arms outward, begin flying sequence

        // moves in direction of remote

    }

    public bool IsInRunStance(InputDevice headset, InputDevice left, InputDevice right)
    {

        Vector3 leftPos = Vector3.Normalize(GetDevicePosition(left));
        Vector3 rightPos = Vector3.Normalize(GetDevicePosition(right));

        Vector3 headPos = Vector3.Normalize(GetDevicePosition(headset));

        float dotLeft = Vector3.Dot(leftPos, headPos);
        float dotRight = Vector3.Dot(rightPos, headPos);

/*        Debug.Log(dotLeft);
        Debug.Log(dotRight); ;*/

        if (Vector3.Dot(leftPos, headPos) < 0.9f && Vector3.Dot(rightPos, headPos) < 0.9f)
        {
            //Debug.Log("Both behind");
            return true;
        }
        else
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


    public IEnumerator Slide(float slideDistance,  float scale, float timeDelay)
    {
        float distance = 0.40f;
        float time = 1.5f * scale;

        float speed = distance / time;

        // change y position of headset over time

        GameObject headsetOffset = GameObject.Find("Player");
        CapsuleCollider body = player.GetComponent<CapsuleCollider>();

        Vector3 ogPosition = headsetOffset.transform.position;
        Vector3 jumpDest = new Vector3(headsetOffset.transform.position.x, headsetOffset.transform.position.y - distance, headsetOffset.transform.position.z);
        Vector3 forwardPos = Vector3.forward;


        if (headset.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headsetRot))
        {
            forwardPos = headsetRot * Vector3.forward;

            forwardPos = new Vector3(forwardPos.x, 0, forwardPos.z);
            Debug.Log(forwardPos);

        }


        float step = Time.deltaTime * 10.0f;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        yield return StartCoroutine(GoUp());
        //yield return new WaitForSeconds(airTime);
        yield return StartCoroutine(MoveForward(scale));
        // yield return new WaitForSeconds(timeDelay);
        yield return StartCoroutine(ComeDown());

        isJumping = false;

        IEnumerator GoUp()
        {
            float t = 0.0f;
            while (t < time)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, jumpDest, step);
                body.height = 1.0f;
                t += step;

                yield return null;
            }
        }

        IEnumerator MoveForward(float distanceScale)
        {

            float t = 0.0f;

            while (t < time)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, rb.transform.position + (forwardPos) * scale, step);

                t += step;
                yield return null;
            }


            yield return null;
        }

        IEnumerator ComeDown()
        {
            Vector3 newPos = new Vector3(rb.transform.position.x, ogPosition.y, rb.transform.position.z);

            float t = 0.0f;
            while (t < time)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, newPos, step);
                body.height = 2.0f;
                t += step;
                yield return null;
            }

        }
    }

    // airTime
    // time after jumping before moving
    // timeDelay
    // time after moving before landing
    // scale
    // distance moved forward

    public IEnumerator SuperJump(float airTime, float scale, float timeDelay)
    {
        float distance = 20.0f * scale;
        float time = 1.5f * scale;

        float speed = distance / time;

        // change y position of headset over time

        GameObject headsetOffset = GameObject.Find("Player");

        Vector3 ogPosition = headsetOffset.transform.position;
        Vector3 jumpDest = new Vector3(headsetOffset.transform.position.x, headsetOffset.transform.position.y + distance, headsetOffset.transform.position.z);
        Vector3 forwardPos = new Vector3(Vector3.forward.x, headsetOffset.transform.position.y, Vector3.forward.z);


        if (headset.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion headsetRot))
        {
            forwardPos = headsetRot * Vector3.forward;

            forwardPos = new Vector3(forwardPos.x, 0, forwardPos.z);
            Debug.Log(forwardPos);

        }


        float step = Time.deltaTime * speed;
        Rigidbody rb = player.GetComponent<Rigidbody>();
        
        yield return StartCoroutine(GoUp());
        yield return new WaitForSeconds(airTime);
        yield return StartCoroutine(MoveForward(scale));
        yield return new WaitForSeconds(timeDelay);
        yield return StartCoroutine(ComeDown());

        isJumping = false;

        
        IEnumerator GoUp()
        {
            float t = 0.0f;
            while (t < time)
            {
                //headsetOffset.transform.position = Vector3.MoveTowards(headsetOffset.transform.position, jumpDest, step);
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, jumpDest, step);


                t += step;

                yield return null;
            }
        }

        IEnumerator MoveForward(float distanceScale)
        {

            float t = 0.0f;

            while (t < time)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, rb.transform.position + (forwardPos) * scale, step);

                t += step;
                yield return null;
            }
            

            yield return null;
        }


        IEnumerator ComeDown()
        {
            Debug.Log("Came down");
            
            Vector3 newPos = new Vector3(headsetOffset.transform.position.x, ogPosition.y, headsetOffset.transform.position.z);

            float t = 0.0f;
            while (t < time)
            {
                rb.transform.position = Vector3.MoveTowards(rb.transform.position, newPos, step);
                t += step;
                yield return null;
            }

        }

    }





    // grapple hook to reposition enemies

    // energy bars:
    //
    // health
    //
    // jump
    // duck
    // fly
    //
    // collectible:
    //
    // starfruit ninja stars

    // chop
    // stab
    // parry

    // consider adding hand on top of other to extend blade 

    public bool IsJumping()
    {
        if (IsTopUp(right) && IsTopUp(left) && GetDeviceVelocityUp(right) > 0.5f && GetDeviceVelocityUp(left) > 0.5f)
            return true;
        return false;
    }

    public bool IsDucking()
    {
        if (IsTopUp(right) && IsTopUp(left) && GetDeviceVelocityUp(right) < -0.5f && GetDeviceVelocityUp(left) < -0.5f)
            return true;
        return false;
    }


    // returns the headset's lean on the Y-plane 
    public Vector2 LeanDirection(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            Vector3 lean = rot * Vector3.up;

            Vector2 movement = new Vector2(lean.x, lean.z);

            return movement;
        }

        return new Vector2(0,0);
    }


    // checks if device is rightside up with an 'allowance' of wiggle room
    public bool IsTopUp(InputDevice device)
    {
        float allowance = 25.0f;

        if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
        {
            //Quaternion deviceOffset = Quaternion.AngleAxis(90, Vector3.up);


            // Quest controller's up seems to be normal to its spine
            // forward vector is therefore its up when held vertically
            Vector3 up = rot * Vector3.forward;

            float degreesFromUp = Vector3.Angle(up, Vector3.up);

            // float degreesFromUp = Vector3.Angle(up, device.gameObject.transform.position.forward);

            if (degreesFromUp < allowance)
                return true;

        }
        return false;
    }
    public float GetDeviceVelocityUp(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 vel))
        {
            return vel.y;
        }

        return 0.0f;
    }

}


// Jump, button press
// Fly, press and hold with meter, wii golf meter
// Duck, button press

// Adjust camera offset to simulate movement

// get device rotation

// get 
