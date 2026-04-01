using UnityEngine;

namespace AQS.Test
{
    /// <summary>
    /// Attach to Raccoon_Weapon_Test root. Monitors fired Bolt projectiles and logs
    /// their position, velocity, and direction for the first few frames after firing.
    /// </summary>
    public class ProjectileTracker : MonoBehaviour
    {
        [Header("Settings")]
        public float trackDuration = 3f;

        private GameObject _lastProjectile;
        private float _trackTimer;
        private Vector3 _spawnPos;
        private int _frameCount;
        private int _boltId;

        private void Update()
        {
            // Find any new Bolt(Pool) objects
            GameObject[] allBolts = GameObject.FindGameObjectsWithTag("Untagged");
            foreach (GameObject go in allBolts)
            {
                if (go.name == "Bolt(Pool)" && go.activeInHierarchy && go != _lastProjectile)
                {
                    _lastProjectile = go;
                    _trackTimer = trackDuration;
                    _frameCount = 0;
                    _spawnPos = go.transform.position;
                    _boltId++;

                    Rigidbody rb = go.GetComponent<Rigidbody>();
                    string rbInfo = rb != null
                        ? $"vel={rb.linearVelocity} speed={rb.linearVelocity.magnitude:F1}"
                        : "no Rigidbody";

                    Collider col = go.GetComponent<Collider>();
                    string colInfo = col != null ? $"collider={col.GetType().Name} trigger={col.isTrigger}" : "no collider";

                    Debug.Log($"[ProjectileTracker] BOLT #{_boltId} SPAWNED at {go.transform.position} forward={go.transform.forward} {rbInfo} {colInfo}");
                    break;
                }
            }

            // Track active projectile
            if (_lastProjectile != null && _lastProjectile.activeInHierarchy && _trackTimer > 0)
            {
                _trackTimer -= Time.deltaTime;
                _frameCount++;

                if (_frameCount <= 10 || _frameCount % 30 == 0)
                {
                    Vector3 pos = _lastProjectile.transform.position;
                    float totalDist = Vector3.Distance(_spawnPos, pos);
                    Rigidbody rb = _lastProjectile.GetComponent<Rigidbody>();
                    string rbInfo = rb != null
                        ? $"vel={rb.linearVelocity} speed={rb.linearVelocity.magnitude:F1}"
                        : "";
                    Debug.Log($"[ProjectileTracker] #{_boltId} f={_frameCount} pos={pos} totalDist={totalDist:F2} {rbInfo}");
                }
            }
            else if (_lastProjectile != null && !_lastProjectile.activeInHierarchy && _frameCount > 0)
            {
                Debug.Log($"[ProjectileTracker] #{_boltId} DEACTIVATED after {_frameCount} frames. Last pos={_lastProjectile.transform.position} totalDist={Vector3.Distance(_spawnPos, _lastProjectile.transform.position):F2}");
                _frameCount = 0;
            }
        }
    }
}
