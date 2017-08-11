using System;
using System.Linq;
using UnityEngine;

public static class CameraExtensions {
    public static Bounds OrthographicBounds(this Camera camera) {
        float screenAspect = (float) Screen.width / (float) Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}

namespace UnityStandardAssets._2D {
    public class Camera2DFollow : MonoBehaviour {
        private Transform target;
        public float damping = 1;
        public float lookAheadFactor = 3;
        public float lookAheadReturnSpeed = 0.5f;
        public float lookAheadMoveThreshold = 0.1f;

        private float m_OffsetZ;
        private Vector3 m_LastTargetPosition;
        private Vector3 m_CurrentVelocity;
        private Vector3 m_LookAheadPos;

        public Bounds bounds;

        private void Start() {
            bounds = GetComponent<Camera>().OrthographicBounds();
        }

        // Update is called once per frame
        private void Update() {
            if (target == null) {
                var player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
                if (player == null)
                    return;
                target = player.transform;
                m_LastTargetPosition = target.position;
                m_OffsetZ = (transform.position - target.position).z;
                transform.parent = null;
            }

            // only update lookahead pos if accelerating or changed direction
            float xMoveDelta = (target.position - m_LastTargetPosition).x;

            bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

            if (updateLookAheadTarget) {
                m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
            }
            else {
                m_LookAheadPos =
                    Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);
            }

            var aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward * m_OffsetZ;
            var newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping);

            var deltaX = (32f - bounds.size.x) / 2f;
            var minX = bounds.center.x - deltaX;
            var maxX = bounds.center.x + deltaX;

            var deltaY = (18f - bounds.size.y) / 2f;
            var minY = bounds.center.y - deltaY;
            var maxY = bounds.center.y + deltaY;

            var x = Mathf.Clamp(newPos.x, minX, maxX);
            var y = Mathf.Clamp(newPos.y, minY, maxY);

            transform.position = new Vector3(x, y, transform.position.z);

            m_LastTargetPosition = target.position;
        }
    }
}
