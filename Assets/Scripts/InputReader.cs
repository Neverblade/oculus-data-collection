using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class InputReader : MonoBehaviour {

    public Transform cameraRig;

    Transform headset;
    Transform leftController;
    Transform rightController;

    // Calibration Related
    string calibration;
    Vector3 calibratedHeadsetPosition;
    Vector3 calibratedHeadsetEuler;
    Vector3 calibratedLeftPosition;
    Vector3 calibratedLeftEuler;
    Vector3 calibratedRightPosition;
    Vector3 calibratedRightEuler;

    // File writing
    string tempPath;
    string exportDir;
    bool recording = false;
    float startingTime;
    int frames;
    StreamWriter sw;

	// Use this for initialization
	void Start () {
        headset = cameraRig.Find("TrackingSpace").Find("CenterEyeAnchor");
        leftController = cameraRig.Find("TrackingSpace").Find("LeftHandAnchor");
        rightController = cameraRig.Find("TrackingSpace").Find("RightHandAnchor");
        tempPath = Application.persistentDataPath + "/temp";
        exportDir = Application.persistentDataPath + "/exports";
        Directory.CreateDirectory(exportDir);
    }

    private void Update() {
        // Calibration handling.
        if (Input.GetKeyDown(KeyCode.C)) {
            print("Updating calibration values...");
            calibration = GetFrameString();
        }

        // Recording handling.
        if (Input.GetKeyDown(KeyCode.Space)) {
            print("Detected space key.");
            recording = !recording;
            if (recording) {
                startingTime = Time.time;
                frames = 0;
                print("Creating temporary file.");
                sw = new StreamWriter(tempPath);
                sw.WriteLine("Frame Time: " + Time.fixedDeltaTime);
                sw.WriteLine("Calibration: " + calibration);
                print("Starting recording.");
            }
            else {
                sw.Close();
                print("Closing temporary file.");
                string fileName = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
                print("Creating export file: " + fileName);
                StreamReader sr = new StreamReader(tempPath);
                sw = new StreamWriter(exportDir + "/" + fileName + ".txt");
                sw.WriteLine("Frames: " + frames);

                string line;
                while ((line = sr.ReadLine()) != null) {
                    sw.WriteLine(line);
                }

                sr.Close();
                sw.Close();
                sw = null;
                File.Delete(tempPath);
                print("Finished recording.");
            }
        }
    }

    private string GetFrameString() {
        return headset.position.x + " "
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
                    + rightController.eulerAngles.z;
    }

    private void FixedUpdate () {
        if (recording && sw != null) {
            sw.WriteLine(GetFrameString());
            frames++;

            //print("Headset: " + headset.position);
            //print("Left: " + leftController.position);
            //print("Right: " + rightController.position);
            //print("Time: " + (Time.time - startingTime));
            //print("vs Time: " + Time.fixedDeltaTime);
        }
    }
}
