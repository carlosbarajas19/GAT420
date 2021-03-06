using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtilityAgent : Agent
{
    [SerializeField] Perception perception;
    [SerializeField] MeterUI meter;
    [SerializeField] float objectCooldown = 10;

    const float MIN_SCORE = 0.2f;

    Need[] needs;
    UtilityObject activeUtilityObject = null;
    public bool isUsingUtilityObject { get { return activeUtilityObject != null; } }

    public float happiness
    {
        get
        {
            float totalMovive = 0;
            foreach(var need in needs)
            {
                totalMovive += need.motive;
            }

            return 1 - (totalMovive / needs.Length);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        needs = GetComponentsInChildren<Need>();

        meter.text.text = "";

    }

    // Update is called once per frame
    void Update()
    {
        //animator.SetFloat("speed", movement.velocity.magnitude);
        
        if(activeUtilityObject == null)
        {
            var gameObjects = perception.GetGameObjects();
            List<UtilityObject> utilityObjects = new List<UtilityObject>();
            foreach(var go in gameObjects)
            {
                if(go.TryGetComponent<UtilityObject>(out UtilityObject utilityObject))
                {
                    utilityObject.visible = true;
                    utilityObject.score = GetUtilityObjectScore(utilityObject);
                    if (utilityObject.score > MIN_SCORE && utilityObject.cooldown == 0) utilityObjects.Add(utilityObject);
                }
            }

            //set active utility object to first utility object
            activeUtilityObject = (utilityObjects.Count == 0) ? null : utilityObjects[0];
            if(activeUtilityObject != null)
            {
                StartCoroutine(ExecuteUtilityObject(activeUtilityObject));
            }
        }
    }

    private void LateUpdate()
    {
        meter.slider.value = happiness;
        meter.worldPosition = transform.position + (Vector3.up * 4);
    }

    IEnumerator ExecuteUtilityObject(UtilityObject utilityObject)
    {
        //go to location
        movement.MoveTowards(utilityObject.location.position);
        while(Vector3.Distance(transform.position, utilityObject.location.position) > 2.0f)
        {
            Debug.DrawLine(transform.position, utilityObject.location.position);

            yield return null;
        }

        print("start effect");

        //start effect
        if (utilityObject.effect != null) utilityObject.effect.SetActive(true);

        //wait duration
        yield return new WaitForSeconds(utilityObject.duration);

        //stop effect
        if (utilityObject.effect != null)
        {
            utilityObject.effect.SetActive(false);
            utilityObject.cooldown = objectCooldown;
        }
        
        print("stop effect");

        //apply
        ApplyUtilityObject(utilityObject);

        activeUtilityObject = null;

        yield return null;
    }

    void ApplyUtilityObject(UtilityObject utilityObject)
    {
        foreach (var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if (need != null)
            {
                need.input += effector.change;
                need.input = Mathf.Clamp(need.input, -1, 1);
            }
        }

    }

    float GetUtilityObjectScore(UtilityObject utilityObject)
    {
        float score = 0;
        foreach(var effector in utilityObject.effectors)
        {
            Need need = GetNeedByType(effector.type);
            if(need != null)
            {
                float futureNeed = need.getMotive(need.input + effector.change);
                score += need.motive - futureNeed;
            }
        }
        return score;
    }

    Need GetNeedByType(Need.Type type)
    {
        return needs.First(need => need.type == type);
    }

    UtilityObject GetHighestUtilityObject(UtilityObject[] utilityObjects)
    {
        UtilityObject highestUtilityObject = null;
        float highestScore = MIN_SCORE;
        foreach (var utilityObject in utilityObjects)
        {
            var score = utilityObject.score;// get the score of the utility object
            if (score > highestScore)// if score > highest score then set new highest score and highest utility object
            {
                highestScore = score;
                highestUtilityObject = utilityObject;
            }
        }

        return highestUtilityObject;
    }

    UtilityObject GetRandomUtilityObject(UtilityObject[] utilityObjects)
    {
        // evaluate all utility objects
        float[] scores = new float[utilityObjects.Length];
        float totalScore = 0;
        for (int i = 0; i < utilityObjects.Length; i++)
        {
            var score = utilityObjects[i].score; // <get the score of the utility objects[i]>
            scores[i] = score; // <set the scores[i] to the score>
            totalScore += score; // <add score to total score>
        }

        // select random utility object based on score
        // the higher the score the greater the chance of being randomly selected

        float random = Random.Range(0, totalScore);// <float random = value between 0 and totalScore>
        for (int i = 0; i < scores.Length; i++)
        {
            // <check if random value is less than scores[i]>
            if (random < scores[i]) return utilityObjects[i]; // <return utilityObjects[i] if less than>
            random -= scores[i]; // <subtract scores[i] from random value>
        }

        return null;
    }



    /*private void OnGUI()
    {
        Vector2 screen = Camera.main.WorldToScreenPoint(transform.position);

        GUI.color = Color.black;
        int offset = 0;
        foreach (var need in needs)
        {
            GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), need.type.ToString() + ": " + need.motive);
            offset += 20;
        }
        //GUI.Label(new Rect(screen.x + 20, Screen.height - screen.y - offset, 300, 20), mood.ToString());
    }*/

}
