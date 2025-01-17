﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Caughman
{
    public class PlayerMovement : MonoBehaviour
    {
        public bool useMouseForAiming = true;

        /// <summary>
        /// Measurment of Meters per second the player will move
        /// </summary>
        public float speed = 7.5f;

        /// <summary>
        /// Reference to the scenes camera
        /// </summary>
        Camera cam;

        // Start is called before the first frame update
        void Start()
        {
            //cam = GameObject.FindObjectOfType<Camera>()
            cam = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            Move();
            if(useMouseForAiming) RotateWithMouse();
            else RotateWithAnalogStick();

        }

        private void RotateWithMouse()
        {
            if (cam == null)
            {
                Debug.LogError("There's no camera to do a raycast from");
                return;
            }

            Plane plane = new Plane(Vector3.up, transform.position);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(plane.Raycast(ray, out float dis))
            {
                Vector3 mousePos = ray.GetPoint(dis);

                Vector3 vectorToMousePos = mousePos - transform.position;

                float radians = Mathf.Atan2(vectorToMousePos.z, vectorToMousePos.x);
                float degrees = radians * 180 / Mathf.PI;
                transform.eulerAngles = new Vector3(0, -degrees, 0);
            }
        }

        private void RotateWithAnalogStick()
        {
            //horizontal input of controller left stick
            float h = Input.GetAxis("Horizontal2");
            //vertical input of controller left stick
            float v = Input.GetAxis("Vertical2");

            //print($"horizontal input: {h}  vertical input: {v}");

            Vector3 dir = new Vector3(h, 0, v);

            if (dir.magnitude < .5f) return;

            float radians = Mathf.Atan2(v, h);
            float degrees = radians * 180 / Mathf.PI;

            transform.eulerAngles = new Vector3(0, degrees, 0);
        }

        private void Move()
        {
            //horizontal input
            float h = Input.GetAxisRaw("Horizontal");
            //vertical input
            float v = Input.GetAxisRaw("Vertical");

            //direction we want player to move based on our input
            Vector3 dir = new Vector3(h, 0, v).normalized;

            //Players movment in meters per second
            transform.position += dir * speed * Time.deltaTime;
        }
    }
}