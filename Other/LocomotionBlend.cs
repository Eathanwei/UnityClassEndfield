using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionBlend: MonoBehaviour
{
    Animator animator;
    [SerializeField] Transform cameraTransform;
    [Range(-1.0f, 1.0f)] public float direction;
    [Range(-1.0f, 1.0f)] public float speed;
    Vector2 inputVector;
    readonly float rotationDuration = 0.1f;
    private InputAction moveAction;

    CharacterController controller;
    CharacterCombatLive characterCombatLive;
    float verticalVelocity = 0f;
    readonly float changeSpeed = 1f;

    Vector3 camForward;
    Vector3 camRight;
    Vector3 targetDirection;
    Vector3 targetPosition;
    Quaternion targetRotation;
    float angleDiff;
    readonly float moveDistance = 0f;

    bool isSprint = false;
    bool isFast = false;
    [SerializeField] bool isWsadRotate = true;
    [SerializeField] bool isFacingTarget = false;
    Coroutine turnToTargetCoroutine;
    public GameObject currentTarget;
    public GameObject currentSkillTarget;
    [SerializeField] float basicAttackDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        direction = 0;
        speed = 0;
        moveAction = InputSystem.actions.FindAction("Move");
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        characterCombatLive = GetComponent<CharacterCombatLive>();
    }
    void OnEnable()
    {
        moveAction.Enable();
        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;
    }
    void OnDisable()
    {
        inputVector = Vector2.zero;
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.Disable();
    }
    private void Update()
    {
        HandleGravity();
        HandleMovementAndRotation();
        if (isWsadRotate || !isFacingTarget)
        {
            if (turnToTargetCoroutine != null)
            {
                StopCoroutine(turnToTargetCoroutine);
                turnToTargetCoroutine = null;
            }
            TryAttack(ref currentSkillTarget);
        }
        else
        {
            if(TryAttack(ref currentTarget))
            {
                if (turnToTargetCoroutine == null && currentTarget != null) 
                {
                    turnToTargetCoroutine = StartCoroutine(SmoothTurnToTarget(currentTarget.transform));
                }
            }
        }
    }

    void OnMove(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
    }
    void HandleMovementAndRotation()
    {
        // 1. 取得輸入的向量
        float currentInputSpeed = inputVector.magnitude;
        if (InputManager.isUsingUI)
        {
            currentInputSpeed = 0;
        }
        if (currentInputSpeed > 0.1f && !InputManager.isUsingUI)
        {
            // 2. 計算攝影機在 XZ 平面的朝向
            camForward = cameraTransform.forward;
            camForward.y = 0;
            camForward.Normalize();
            camRight = cameraTransform.right;
            camRight.y = 0;
            camRight.Normalize();
            // 3. 計算目標移動方向 (根據 Camera 朝向與 WASD)
            targetDirection = (camForward * inputVector.y + camRight * inputVector.x).normalized;
            float angle = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            targetPosition = transform.position + targetDirection * moveDistance;
            // 4. 直接使用程式驅動旋轉 (Quaternion.LookRotation)
            if (Mathf.Abs(angle) > 175f)
            {
                Vector3 biasedDir = Quaternion.Euler(0, 5f, 0) * targetDirection;
                targetRotation = Quaternion.LookRotation(biasedDir);
            }
            else
            {
                targetRotation = Quaternion.LookRotation(targetDirection);
            }
            // 5. 在約 0.1 秒內 Lerp/Slerp 到目標方向
            // 使用 Slerp (球面線性插值) 旋轉起來最自然
            if (isWsadRotate)
            {
                float t = Time.deltaTime / rotationDuration;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, t);
                transform.position = Vector3.Lerp(transform.position, targetPosition, t);
            }
            // 6. 計算 Animator 的 Direction 參數 (-1 為左轉, 1 為右轉)
            // 這裡使用 Vector3.Cross 來判斷角色當前朝向與目標方向的夾角正負
            angleDiff = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
            direction = angleDiff >= 0 ? 1f : -1f;
            UpdateParameter(currentInputSpeed, ref speed);
            //speed = currentInputSpeed;
        }
        else
        {
            isFast = false;
            direction = 0;
            UpdateParameter(currentInputSpeed, ref speed);
            //speed = 0;
        }
        // 6. 更新 Animator
        animator.SetFloat("direction", direction);
        animator.SetFloat("speed", speed);
    }
    void UpdateParameter(float input, ref float value)
    {
        if (isSprint && value > 0.6666667f)
        {
            if (input < 0.1f)
            {
                value -= changeSpeed * Time.deltaTime * 0.5f;
            }
            value -= changeSpeed * Time.deltaTime * 0.2f;
            return;
        }
        if (input > 0.1f)
        {
            value += changeSpeed * Time.deltaTime * 2f;
            if (isFast && value > 0.6666667f)
            {
                value = 0.6666667f;
            }
            else if (!isFast && value > 0.3333333f)
            {
                value = 0.3333333f;
            }
        }
        else if (input < 0.1f)
        {
            value -= changeSpeed * Time.deltaTime * 2f;
            if (value < 0f)
            {
                value = 0f;
            }
        }
    }
    void HandleGravity()
    {
        if (!controller.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -2f;
        }
        controller.Move(verticalVelocity * Time.deltaTime * Vector3.up);
    }
    public void IsWsadRotate(bool isWsadRotate)
    {
        this.isWsadRotate = isWsadRotate;
    }
    public void IsFacingTarget(bool isFacingTarget)
    {
        this.isFacingTarget = isFacingTarget;
    }
    public void Sprint()
    {
        float currentInputSpeed = inputVector.magnitude;
        if (currentInputSpeed > 0.1f)
        {
            isSprint = true;
            isFast = true;
            speed = 1f;
        }
        else
        {
            animator.SetTrigger("Evade");
            animator.SetTrigger("Exit");
        }
    }
    public bool TryAttack(ref GameObject myTarget)
    {
        if (PlaceManager.Instance.CombatNum == -1)
        {
            myTarget = null;
            return false;
        }
        if (PlaceManager.Instance.CombatNum == 0)
        {
            if (PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum][0].GetComponent<BossState>().Fly)
            {
                myTarget = null;
                return false;
            }
            else 
            {
                myTarget = PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum][0];
                return true;
            }
        }
        bool currentTargetValid = currentTarget != null && !currentTarget.GetComponent<EnemyCombatData>().CombatUnitData.isDead && Vector3.Distance(currentTarget.transform.position, transform.position) < basicAttackDistance - 1;
        if (characterCombatLive.skillNow && currentTargetValid)
        {
            return true;
        }
        GameObject tempTarget = null;
        float minDistance = 10000f;
        for (int i = 0; i < PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum].Count; i++)
        {
            if (PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum][i].GetComponent<EnemyCombatData>().CombatUnitData.isDead)
            {
                continue;
            }
            Vector3 v = PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum][i].transform.position - transform.position;
            v.y = 0;
            float fDist = v.magnitude;
            if (fDist < minDistance)
            {
                minDistance = fDist;
                tempTarget = PlaceManager.Instance.GroupEnemies[PlaceManager.Instance.CombatNum][i].gameObject;
            }
        }
        float currentDis;
        if (currentTargetValid)
        {
            currentDis = Vector3.Distance(currentTarget.transform.position, transform.position);
        }
        else
        {
            currentTarget = null;
            currentDis = 10000f;
        }
        if (minDistance < currentDis - 1)
        {
            myTarget = tempTarget;
        }
        if (myTarget == null)
        {
            return false;
        }
        Vector3 vTarget = myTarget.transform.position - transform.position;
        float targetDist = vTarget.magnitude;
        if (targetDist < basicAttackDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator SmoothTurnToTarget(Transform targetTransform)
    {
        float turnDuration = 0.2f;
        float timeElapsed = 0f;

        Quaternion startRotation = transform.rotation;

        Vector3 vTarget = targetTransform.position;
        vTarget.y = transform.position.y;

        Vector3 direction = vTarget - transform.position;

        direction.y = 0f;

        if (direction == Vector3.zero) yield break;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        while (timeElapsed < turnDuration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, timeElapsed / turnDuration);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = targetRotation;
        turnToTargetCoroutine = null;
    }
}
