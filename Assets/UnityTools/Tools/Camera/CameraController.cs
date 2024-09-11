using UnityEngine;
using System.Collections;

/// <summary>
/// 直接赋给Camera
/// </summary>
public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotateSpeed = 5f;

    private float zoomSpeed = 50f;
    private float focusDistance = 5f;
    private float allSpeedMultiplier;
    private float runSpeedMultiplier = 1f;
    private float shiftSpeed = 1f;
    private float mouseScrollWheelSpeed = 1f;

    public Transform target;

    private Vector3 lastMousePosition;
    private Vector3 initialRotation;

    #region MONO

    private void Start()
    {
        initialRotation = transform.localEulerAngles;
        allSpeedMultiplier = PlayerPrefs.GetFloat("allSpeedMultiplier", 1f);
    }

    private void Update()
    {
        HandleMouseInput();
        HandleKeyboardInput();
        HandleMouseScrollWheel();
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("allSpeedMultiplier", allSpeedMultiplier);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("allSpeedMultiplier", allSpeedMultiplier);
        PlayerPrefs.Save();
    }

    #endregion MONO

    /// <summary>
    /// 鼠标控制camera旋转
    /// </summary>
    private void HandleMouseInput()
    {
        // 右键按住旋转相机
        if (Input.GetMouseButton(1))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = -delta.y * rotateSpeed * Time.deltaTime;
            float rotationY = delta.x * rotateSpeed * Time.deltaTime;

            transform.localEulerAngles += new Vector3(rotationX, rotationY, 0f);
        }

        // 左键点击选择对象
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                target = hit.transform;
            }
        }

        // 更新鼠标位置
        lastMousePosition = Input.mousePosition;
    }

    /// <summary>
    /// 鼠标滚轮控制camera前进后退和步进
    /// </summary>
    private void HandleMouseScrollWheel()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButton(1))
        {
            if (scroll != 0)
            {
                allSpeedMultiplier += scroll > 0 ? 0.1f : -0.1f;
                allSpeedMultiplier = Mathf.Clamp(allSpeedMultiplier, 0.1f, 2f);
                Debug.Log(allSpeedMultiplier);
            }
        }
        else
        {
            transform.position += allSpeedMultiplier * shiftSpeed * Time.deltaTime * zoomSpeed * transform.forward * scroll;
        }
    }

    /// <summary>
    /// 键盘控制camera移动
    /// </summary>
    private void HandleKeyboardInput()
    {
        if (Input.GetMouseButton(1))
        {
            shiftSpeed = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;
            if (Input.GetKeyUp(KeyCode.LeftShift)) shiftSpeed = 1f;

            if (Input.anyKey)
            {
                runSpeedMultiplier = Mathf.Min(runSpeedMultiplier + 0.02f, 1f);
            }
            else
            {
                runSpeedMultiplier = 1f;
            }

            float h = Input.GetAxis("Horizontal"); // A,D键
            float v = Input.GetAxis("Vertical"); // W,S键
            Vector3 direction = new Vector3(h, 0, v);
            direction = direction.normalized * allSpeedMultiplier * runSpeedMultiplier * shiftSpeed * moveSpeed * Time.deltaTime;
            transform.Translate(direction, Space.Self);

            // Q,E键上下移动相机
            if (Input.GetKey(KeyCode.E))
            {
                transform.Translate(allSpeedMultiplier * moveSpeed * runSpeedMultiplier * shiftSpeed * Time.deltaTime * Vector3.up, Space.Self);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                transform.Translate(allSpeedMultiplier * moveSpeed * runSpeedMultiplier * shiftSpeed * Time.deltaTime * Vector3.down, Space.Self);
            }
        }
        else
        {
            runSpeedMultiplier = 1f;
        }

        // F键看向选择的对象并拉近距离
        if (target != null && Input.GetKeyDown(KeyCode.F))
        {
            Vector3 directionToTarget = (target.position - transform.position).normalized;
            Vector3 newPosition = target.position - directionToTarget * focusDistance;

            StartCoroutine(MoveToPosition(newPosition, target.position));
        }
    }

    /// <summary>
    /// 平滑移动相机到新位置并看向目标
    /// </summary>
    /// <param name="newPosition"></param>
    /// <param name="targetPosition"></param>
    /// <returns></returns>
    private IEnumerator MoveToPosition(Vector3 newPosition, Vector3 targetPosition)
    {
        float duration = 1f;  // 平滑移动的持续时间
        float elapsedTime = 0;

        Vector3 startingPos = transform.position;
        Quaternion startingRot = transform.rotation;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startingPos, newPosition, elapsedTime / duration);
            transform.rotation = Quaternion.Slerp(startingRot, Quaternion.LookRotation(targetPosition - transform.position), elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 确保最终位置和旋转准确
        transform.position = newPosition;
        transform.LookAt(targetPosition);
    }
}