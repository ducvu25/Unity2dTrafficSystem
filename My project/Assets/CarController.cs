using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public enum Huong
{
    UP, DOWN, LEFT, RIGHT
}
public class CarController : MonoBehaviour
{
    public Sprite[] sprites;
    public int type = 0;


    public float[] speeds;
    public Huong huong;
    float speed;
    bool run;

    float _delayDieuHuong = 0f;
    [SerializeField] LayerMask lmRotation;
    [SerializeField] Transform pointTop, pointRight;
    [SerializeField] float pointRotationDistance = 0.1f;

    public LightTraficController stLight;
    public float distance;
    GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        run = true;
        controller = FindObjectOfType<GameController>();
        transform.GetComponent<SpriteRenderer>().sprite = sprites[type];
        speed = speeds[type];
    }

    // Update is called once per frame
    void Update()
    {
        CheckRun();
        if (run)
            Run();
    }
    void CheckRun()
    {
        _delayDieuHuong -= Time.deltaTime;

        // Kiểm tra có vật thể nào trong bán kính pointRotationDistance hay không
        Collider2D collider = Physics2D.OverlapCircle(pointRight.position, pointRotationDistance, lmRotation);
        if (collider != null && _delayDieuHuong <= 0
            && Physics2D.Raycast(pointTop.position, transform.up, pointRotationDistance, lmRotation).collider == null)
        {
            RotateRightWithDuration();
            _delayDieuHuong = 3f;
        }

        run = true;
        if (stLight.value != 0 && Vector2.Distance(transform.position, stLight.transform.position) < distance)
        {
            run = false;
        }
    }
    void Run()
    {
        List<CarController> carsWithinRadius = controller.getCarsRadius(this, distance);
        if (carsWithinRadius.Count > 0)
        {
            return;
        }

        // Tiếp tục di chuyển nếu không có xe khác trong bán kính
        Vector2 move = Vector2.zero;
        switch (huong)
        {
            case Huong.UP:
                {
                    move = new Vector2(0, 1) * speed;
                    break;
                }
            case Huong.DOWN:
                {
                    move = new Vector2(0, -1) * speed;
                    break;
                }
            case Huong.LEFT:
                {
                    move = new Vector2(-1, 0) * speed;
                    break;
                }
            case Huong.RIGHT:
                {
                    move = new Vector2(1, 0) * speed;
                    break;
                }
        }
        transform.position = new Vector3(transform.position.x + move.x * Time.deltaTime, transform.position.y + move.y * Time.deltaTime, 0);
    }
    public float duration = 1f;
    
    public void RotateRightWithDuration()
    {
        StartCoroutine(RotateRightCoroutine(duration));
        switch (huong)
        {
            case Huong.UP:
                {
                    huong = Huong.RIGHT;
                    break;
                }
            case Huong.DOWN:
                {
                    huong = Huong.LEFT;
                    break;
                }
            case Huong.LEFT:
                {
                    huong = Huong.UP;
                    break;
                }
            case Huong.RIGHT:
                {
                    huong = Huong.DOWN;
                    break;
                }
        }
    }

    private IEnumerator RotateRightCoroutine(float duration)
    {
        float elapsedTime = 0f;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = transform.rotation * Quaternion.Euler(0f, 0f, -90f);
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Vector2 move = Vector2.zero;
            switch (huong)
            {
                case Huong.UP:
                    {
                        move = new Vector2(0.5f, -0.5f) * speed;
                        break;
                    }
                case Huong.DOWN:
                    {
                        move = new Vector2(-0.5f, 0.5f) * speed;
                        break;
                    }
                case Huong.LEFT:
                    {
                        move = new Vector2(0.5f, 0.5f) * speed;
                        break;
                    }
                case Huong.RIGHT:
                    {
                        move = new Vector2(-0.5f, -0.5f) * speed;
                        break;
                    }
            }
            transform.position = new Vector3(transform.position.x + move.x * Time.deltaTime, transform.position.y + move.y * Time.deltaTime, 0);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRotation;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointRight.position, pointRotationDistance);
        //Gizmos.DrawLine(pointWall.position, new Vector3(pointWall.position.x + pointWallDistance, pointWall.position.y, pointWall.position.z));
        //Gizmos.DrawLine(pointRight.position, new Vector3(pointRight.position.x + pointRotationDistance, pointRight.position.y, pointRight.position.z));
        Gizmos.DrawLine(pointTop.position, new Vector3(pointTop.position.x, pointTop.position.y + pointRotationDistance*2, pointRight.position.z));
    }
}
