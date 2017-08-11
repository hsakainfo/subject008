using UnityEngine;

public class ModifyDetectionComponent : ItemBaseComponent
{
    public Collider2D Col2D;
    private float delta;
    private Color blend = new Color(1f,1f,1f,0.1f);
    private SpriteRenderer renderer;
    public override void Start() {
        //Disable detection of this gameObject
        DetectionController dc = gameObject.GetComponent<DetectionController>();
		dc.isAbleToRecord = false;
        //Blending
        renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.color = blend;
        delta = 0f;
        //Calling base start
        base.Start();
    }

    public override void Update() {
        delta += Time.deltaTime / time;
        renderer.color = Color.Lerp(blend, Color.white, delta);
        //Call base
        base.Update();
    }
	public override void OnDestroy() {
		DetectionController dc = gameObject.GetComponent<DetectionController>();
		dc.isAbleToRecord = true;
        //reset
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
		base.OnDestroy();
	}

} 