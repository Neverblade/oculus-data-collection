using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class InputReader : MonoBehaviour {

    public Transform cameraRig;

    bool recording = false;
    float startingTime;
    int frames;
    Transform headset;
    Transform leftController;
    Transform rightController;
    StreamWriter sw;

	// Use this for initialization
	void Start () {
        headset = cameraRig.Find("TrackingSpace").Find("CenterEyeAnchor");
        leftController = cameraRig.Find("TrackingSpace").Find("LeftHandAnchor");
        rightController = cameraRig.Find("TrackingSpace").Find("RightHandAnchor");

        string dirPath = Application.persistentDataPath + "/exports";
        Directory.CreateDirectory(dirPath);
    }

    private void FixedUpdate () {
        if (Input.GetKeyDown(KeyCode.Space)) {
            recording = !recording;
            if (recording) {
                startingTime = Time.time;
                frames = 0;
                print("Creating temporary file.");
                sw = new StreamWriter(Application.persistentDataPath + "/temp");
                sw.WriteLine("Frame Time: " + Time.fixedDeltaTime);
                print("Starting recording.");
            } else {
                sw.Close();
                print("Closing temporary file.");
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                print("Creating export file: " + fileName);
                StreamReader sr = new StreamReader(Application.persistentDataPath + "/temp");
                sw = new StreamWriter(Application.persistentDataPath + "/exports/" + fileName + ".txt");
                sw.WriteLine("Frames: " + frames);

                string line;
                while ((line = sr.ReadLine()) != null) {
                    sw.WriteLine(line);
                }

                sr.Close();
                sw.Close();
                print("Finished recording.");
            }
        }

        if (recording) {
            sw.WriteLine(headset.position.x + " "
                    + headset.position.y + " "
                    + headset.position.z + " "
                    + headset.eulerAngles.x + " "
                    + headset.eulerAngles.y + " "
                    + headset.eulerAngles.z + " "
                    + leftController.position.x + " "
                    + leftController.position.y + " "
                    + leftController.position.z + " "
                    + leftController.eulerAngles.x + " "
                    + leftController.eulerAngles.y + " "
                    + leftController.eulerAngles.z + " "
                    + rightController.position.x + " "
                    + rightController.position.y + " "
                    + rightController.position.z + " "
                    + rightController.eulerAngles.x + " "
                    + rightController.eulerAngles.y + " "
                    + rightController.eulerAngles.z);

            frames++;

            //print("Headset: " + headset.position);
            //print("Left: " + leftController.position);
            //print("Right: " + rightController.position);
            //print("Time: " + (Time.time - startingTime));
            //print("vs Time: " + Time.fixedDeltaTime);
        }
    }
}
