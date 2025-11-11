using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace MiniGameCollection.Games2025.Team06
{
    public class TwoPlayerCamera : MonoBehaviour
    {
        public List<Transform> targets;
        public Vector3 offset;
        private Vector3 velocity;
        public float smoothTime = 0.5f;
        public int minZoom = 100;
        public int maxZoom = 200;
        public int zoomLimiter = 50;
        public int zoomSpeed = 1;
        public PixelPerfectCamera cam;
        public int extremeCloseZoom, closeZoom, regularZoom, farZoom, farthestZoom;

        
        void Start()
        {
            cam = GetComponent<PixelPerfectCamera>();
        }
        void LateUpdate()
        {
            if (targets.Count == 0)
            {
                return;
            }
            Move();
            Zoom();
        }

        void Move()
        {
            Vector3 centerPoint = GetCameraPos();
            Vector3 newPosition = centerPoint + offset;
            transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
        }
        
        void Zoom()
        {
            int newZoom = GetGreatestDistance();
            if (newZoom <= 50)
            {
                if(cam.assetsPPU < extremeCloseZoom)
                {
                    cam.assetsPPU += 1;
                }
            }
            else if (newZoom <= 100 && newZoom > 50)
            {
                if (cam.assetsPPU < closeZoom)
                {
                    cam.assetsPPU += 1;
                }
                if (cam.assetsPPU > closeZoom)
                {
                    cam.assetsPPU -= 1;
                }
            }
            if (newZoom <= 200 && newZoom > 100)
            {
                if (cam.assetsPPU < regularZoom)
                {
                    cam.assetsPPU += 1;
                }
                if (cam.assetsPPU > regularZoom)
                {
                    cam.assetsPPU -= 1;
                }
            }
            else if (newZoom <= 300 && newZoom > 200)
            {
                if (cam.assetsPPU < farZoom)
                {
                    cam.assetsPPU += 1;
                }
                if (cam.assetsPPU > farZoom)
                {
                    cam.assetsPPU -= 1;
                }
            }
            else if(newZoom <= 1000 && newZoom > 300)
            {
                if (cam.assetsPPU < farthestZoom)
                {
                    cam.assetsPPU += 1;
                }
                if (cam.assetsPPU > farthestZoom)
                {
                    cam.assetsPPU -= 1;
                }
            }
            
        }
        Vector3 GetCameraPos()
        {
            if (targets.Count == 1)
            {
                return targets[0].position;
            }

            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center;
        }

        int GetGreatestDistance()
        {
            var bounds = new Bounds(targets[0].position, Vector3.zero);
            for (int i = 0; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }


            if(bounds.size.y > bounds.size.x)
            {
                int roundedFloat = (int)bounds.size.y * 50;
                //roundedFloat += 50;
                //Debug.Log("roundedFloat " + roundedFloat);
                return roundedFloat;
            }
            if (bounds.size.x >= bounds.size.y)
            {
                int roundedFloat = (int)bounds.size.x * 50;
                //Debug.Log("roundedFloat " + roundedFloat);
                return roundedFloat;
            }
            else
            {
                int roundedFloat = (int)bounds.size.x * 50;
                //Debug.Log("roundedFloat " + roundedFloat);
                roundedFloat += 100;
                return roundedFloat;
            }
            
            
        }
    }
}
 
