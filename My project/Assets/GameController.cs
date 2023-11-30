using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float limitX1, limitX2, limitY1, limitY2;
    bool x2Speed;
    public TextMeshProUGUI txtX2Speed;
    public float timeSpawn;
    float _timeSpawn = 0;
    public int number = 20;
    public GameObject[] pointLights;
    public GameObject[] pointSpawns;
    public List<CarController> cars;
    //public List<PointRotation> pointRations;
    public Transform prefabCar;

    // Start is called before the first frame update
    void Start()
    {
        x2Speed = false;
        txtX2Speed.text = x2Speed ? "x2" : "x1";
        cars = new List<CarController>(FindObjectsOfType<CarController>());
    }
    void Spawn(Transform prefab)
    {
        number--;
        Huong[] huongs = {Huong.UP, Huong.DOWN, Huong.LEFT, Huong.RIGHT};
        int x = (int)Random.Range(0, 4);
        Transform t = Instantiate(prefab, pointSpawns[x].transform.position, Quaternion.identity);

        Quaternion rotation = Quaternion.identity;
        switch (x)
        {
            case 0:
                rotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case 1:
                rotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case 2:
                rotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case 3:
                rotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }

        t.rotation = rotation;
        t.GetComponent<CarController>().stLight = pointLights[x].GetComponent<LightTraficController>();
        t.GetComponent<CarController>().huong = huongs[x];
        t.GetComponent<CarController>().type = (int)Random.Range(0, 3);
        cars.Add(t.GetComponent<CarController>());
    }
    private void Update()
    {
        if (_timeSpawn > 0) {
            _timeSpawn -= Time.deltaTime;
        }
        else
        {
            if(cars.Count < number)
                Spawn(prefabCar);
            _timeSpawn = timeSpawn;
        }
        for(int i=0; i<cars.Count; i++)
        {
            if (cars[i].transform.position.x < limitX1 || cars[i].transform.position.x > limitX2
                || cars[i].transform.position.y < limitY1 || cars[i].transform.position.y > limitY2) {
                GameObject t = cars[i].gameObject;
                cars.RemoveAt(i);
                i--;
                Destroy(t);
            }
        }
    }
    public void X2Speed()
    {
        x2Speed = !x2Speed;
        txtX2Speed.text = x2Speed ? "x2" : "x1";
        Time.timeScale = x2Speed ? 2 : 1;
    }
    public List<CarController> getCarsRadius(CarController x, float radius)
    {
        List<CarController> result = new List<CarController>();

        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i] != x && cars[i].huong == x.huong && Vector2.Distance(x.transform.position, cars[i].transform.position) < radius)
            {
                Vector2 direction = cars[i].transform.position - x.transform.position;
                direction.Normalize();

                float dotProduct = Vector2.Dot(x.transform.up, direction);
                float cos60 = Mathf.Cos(Mathf.Deg2Rad * 60);

                if (dotProduct >= cos60)
                {
                    result.Add(cars[i]);
                }
            }
        }

        return result;
    }
/*    public bool DieuHuong(CarController x, float radius)
    {
        for (int i = 0; i < pointRations.Count; i++)
        {
            if (pointRations[i].huong == x.huong && Vector2.Distance(x.transform.position, pointRations[i].transform.position) < radius)
            {
                return Random.Range(0, 3) == 1;
            }
        }
        return false;
    }*/
}
