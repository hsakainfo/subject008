using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DashTrail : MonoBehaviour {
    public float TrailTimer;
    public GameObject[] ObjectsToCopy;
    public Color AfterImageColor;
    public float MinDistance;

    private GameObject _objectToCopy;
    private Vector3 _startPosition;

    public void StartTrail() {
        _startPosition = transform.position;
    }

    public void StopTrail() {
        _objectToCopy = ObjectsToCopy.FirstOrDefault(x => x.activeSelf);
        if (_objectToCopy == null)
            return;

        StartCoroutine(PlaceObjectsDelayed(_startPosition));
    }

    private IEnumerator PlaceObjectsDelayed(Vector3 start) {
        yield return new WaitForFixedUpdate();
        PlaceObjects(start, transform.position);
    }

    private void PlaceObjects(Vector3 start, Vector3 end) {
        var distance = Vector3.Distance(start, end);
        var minFraction = MinDistance / distance;
        for (var fraction = minFraction; fraction < 1f; fraction += minFraction) {
            var position = Vector3.Lerp(start, end, fraction);
            var copy = Instantiate(_objectToCopy);
            var sprites = copy.GetComponentsInChildren<SpriteRenderer>();
            foreach (var sprite in sprites) {
                sprite.color = AfterImageColor;
                sprite.sortingOrder = -1;
            }
            copy.transform.position = position;
            copy.isStatic = true;
            Destroy(copy, fraction * TrailTimer);
        }
    }
}
