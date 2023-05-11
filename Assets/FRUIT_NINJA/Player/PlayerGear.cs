using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.XR;

public class PlayerGear : MonoBehaviour
{
    UnityEngine.XR.InputDevice left;
    UnityEngine.XR.InputDevice right;

    public GameObject grappleLauncher;
    public GameObject starPoint;

    public GameObject starPrefab;

    GameObject ninjaStar;
    public int starCount = 0;

    float grappleLength = 5.0f;
    float grappleSpeed = 1.0f;
    float grappleThrowStrength = 200.0f;

    public bool grappleFired = false;
    public bool starInHand = false;

    GameObject grappledFruit;
    Fruit fruitScript;

    float ogFruitSpeed;
    Vector3 releaseFruitSpeed = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        left = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        right = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        // check if grapple button is pressed (left grip)

        if (left.TryGetFeatureValue(CommonUsages.gripButton, out bool gripPressed))
        {

            // fire grapple

            if (gripPressed && !grappleFired && !starInHand)
            {
                grappleFired = true;

                // shoot grapple raycast


                // create raycast 
                RaycastHit hit;



                GameObject grapple = GameObject.Find("GrappleLauncher");

                Vector3 fwd = grapple.transform.TransformDirection(Vector3.forward);

                if (Physics.Raycast(grapple.transform.position, fwd, out hit, grappleLength))
                {
                    Debug.Log("Sent grapple");

                    // check if raycast touches a fruit
                    if (hit.collider)
                    {
                        if (hit.collider.GetComponent<Fruit>() != null)
                        {
                            grappledFruit = hit.collider.gameObject;

                            Debug.Log(grappledFruit.name);

                            // stop movement of fruit
                            fruitScript = grappledFruit.GetComponent<Fruit>();
                            ogFruitSpeed = fruitScript.GetFruitSpeed();

                            fruitScript.ModifyFruitSpeed(0.0f);
                            grappledFruit.GetComponent<Rigidbody>().velocity = Vector3.zero;

                            // turn off its gravity
                            grappledFruit.GetComponent<Rigidbody>().useGravity = false;

                            grappledFruit.transform.parent = grappleLauncher.transform;

                            

                            

                        }
                        // play nasty gutting sound



                    }
                }


            }

            // grapple after fire

            if (gripPressed && grappleFired)
            {
                // moving stuff 

                Vector2 joystick;

                Quaternion grappleRot = grappleLauncher.transform.rotation;

                Vector3 fruitDisplace = grappleRot * Vector3.forward;


                if (left.TryGetFeatureValue(CommonUsages.primary2DAxis, out joystick))
                {
                    // forward 

                    Debug.Log(joystick);

                    float step = Time.deltaTime * grappleSpeed;

                    if (Vector2.Angle(new Vector2(0, 1), joystick) <= 45.0f && joystick.y > 0.30f)
                    {
                        // move in direction 
                        grappledFruit.transform.position = new Vector3(grappledFruit.transform.position.x + fruitDisplace.x * step, grappledFruit.transform.position.y + fruitDisplace.y * step, grappledFruit.transform.position.z + fruitDisplace.z * step);
                    }

                    // backward
                    if (Vector2.Angle(new Vector2(0, -1), joystick) <= 45.0f && joystick.y < -0.30f)
                    {
                        grappledFruit.transform.position = new Vector3(grappledFruit.transform.position.x - fruitDisplace.x * step, grappledFruit.transform.position.y - fruitDisplace.y * step, grappledFruit.transform.position.z - fruitDisplace.z * step);
                    }
                    


                }

                releaseFruitSpeed = grappledFruit.GetComponent<Rigidbody>().velocity;

            }


            // grapple release
            if (!gripPressed && grappleFired)
            {
                grappleFired = false;

                //grappledFruit = null;
                // launch fruit 


                grappledFruit.GetComponent<Rigidbody>().useGravity = true;
                grappledFruit.transform.parent = null;
                fruitScript.ModifyFruitSpeed(ogFruitSpeed);
                grappledFruit.GetComponent<Rigidbody>().AddForce(DeviceVelocity(left) * grappleThrowStrength);

            }

        }


        // check if ninja star button is pressed

        if (left.TryGetFeatureValue(CommonUsages.triggerButton, out bool triggerPressed))
        {
            if (triggerPressed && !starInHand && !grappleFired && starCount > 0)
            {
                // spawn ninja star as child of point on left hand
                Quaternion starRotation = Quaternion.Euler(0, 90, 0);
                Vector3 starPos = new Vector3(starPoint.transform.position.x - 0.5f, starPoint.transform.position.y, starPoint.transform.position.z);
                ninjaStar = Instantiate(starPrefab, starPos, starRotation);
                ninjaStar.GetComponent<Rigidbody>().useGravity = false;
                ninjaStar.transform.parent = starPoint.transform;

                starInHand = true;

            }

            if (!triggerPressed && starInHand)
            {

                StartCoroutine(StarLaunchSequence());
                starCount--;
                starInHand = false;
            }
        }
    }

    public IEnumerator StarLaunchSequence()
    {
        GameObject starTracker = ninjaStar;

        // make star not child of point
        starTracker.transform.parent = starPoint.transform;
        // throw star in direction of left hand forward vector at magnitude of angular velocity

        starTracker.GetComponent<Rigidbody>().useGravity = true;
        starTracker.GetComponent<Rigidbody>().AddForce(DeviceVelocity(left) * 750);

        yield return new WaitForSeconds(5.0f);
        Destroy(starTracker);

        yield return null;
    }

    public Vector3 DeviceVelocity(InputDevice device)
    {
        if (device.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 vel))
        {
            //Debug.Log(vel);
            return vel;
        }

        return new Vector3(0, 0, 0);
    }

}
