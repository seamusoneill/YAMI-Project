using UnityEngine;
using System.Collections;

public class FlickerLight : MonoBehaviour {



    public float distanceFromSource;
    private Light _lightSource;

    public float MaxReduction;
    public float MaxIncrease;
    public float RateDamping;
    public float Strength;
    public bool StopFlickering;
    private float _baseIntensity;
    private bool _flickering;

    // Use this for initialization
    void Start () {
        _lightSource = GetComponent<Light>();
        MaxReduction = 2.5f;
        MaxIncrease = 2.5f;
        RateDamping = 0.1f;
        Strength = 300;
        _baseIntensity = _lightSource.intensity;
        StartCoroutine(DoFlicker());
    }
	
	// Update is called once per frame
	void Update () {

  

   






}


    private IEnumerator DoFlicker()
    {
        _flickering = true;
        while (!StopFlickering)
        {
            _lightSource.intensity = Mathf.Lerp(_lightSource.intensity, Random.Range(_baseIntensity - MaxReduction, _baseIntensity + MaxIncrease), Strength * Time.deltaTime);
            yield return new WaitForSeconds(RateDamping);
        }
        _flickering = false;
    }



}
