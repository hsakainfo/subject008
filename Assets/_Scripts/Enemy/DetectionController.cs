using UnityEngine;

public abstract class DetectionController : MonoBehaviour {
    public bool isAbleToRecord = true;

    //wether the player is being detected or not
    private bool currentlyDetecting;

    public bool CurrentlyDetecting {
        get { return currentlyDetecting; }
    }

    public AwarenessController awarenessController;
    public float increaseAmount; //how much the detection amount shall be increased per second

    public virtual void Start() {
        awarenessController = EventController.Instance.GetComponent<AwarenessController>();
        Debug.Assert(awarenessController != null, "DetectionController has no AwarenessController?");
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !other.GetComponent<CharacterControlScript>().dashing) {
            awarenessController.ActiveDetectors.Add(this);
        }
    }

    public void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && !other.GetComponent<CharacterControlScript>().dashing) {
            awarenessController.ActiveDetectors.Add(this);
        }
    }

    //when the player exits the viewField
    //set detected in Awareness controller false, so the counter stops increasing the detectionlevel
    public void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            awarenessController.ActiveDetectors.Remove(this);
        }
    }

    public void Update() {
        transform.position = transform.position + Vector3.zero;
    }

    public virtual void LateUpdate() {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (!player)
            return;
        var body = player.GetComponent<Rigidbody2D>();
        var playerCollider = body.GetComponent<Collider2D>();
        //Update detectors
        if (!gameObject.GetComponent<Collider2D>().IsTouching(playerCollider)) {
            awarenessController.ActiveDetectors.Remove(this);
        }
    }
}
