using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static public InputManager Instance;
    static int mainControl = 0;
    public static int MainControl => mainControl;
    List<InputAction> skillAction = new();
    InputAction qAction;
    InputAction eAction;
    InputAction mouseLeftAction;
    InputAction mouseRightAction;
    InputAction altAction;

    //[SerializeField] List<int> comboOrderTestList;
    List<int> comboOrder = new();
    public IReadOnlyList<int> ComboOrder => comboOrder;
    List<float> comboOrderTime = new()
    {
        6f, 6f, 6f, 6f
    };
    public IReadOnlyList<float> ComboOrderTime => comboOrderTime;
    static float comboSkillTempTimeMax = 6;
    public float ComboSkillTempTimeMax => comboSkillTempTimeMax;
    public event Action OnComboOrderChanged;

    [SerializeField] List<GameObject> characters;
    List<Animator> animators = new();
    List<CharacterCombatLive> characterCombatLives = new();
    List<LocomotionBlend> locomotionBlends = new();
    List<NPCBehavior> nPCBehavior = new();
    List<CharacterController> characterControllers = new();
    List<CapsuleCollider> capsuleCollider = new();

    [SerializeField] HW2_camera hW2_Camera;
    static public bool isUsingUI = false;
    public List<GameObject> Characters => characters;
    public event Action OnHpChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        //for (int i = 0, len = comboOrderTestList.Count; i < len; i++)
        //{
        //    comboOrder.Add(comboOrderTestList[i]); //comboTest
        //}
        for (int i = 0, len = characters.Count; i < len; i++)
        {
            animators.Add(characters[i].GetComponent<Animator>());
            characterCombatLives.Add(characters[i].GetComponent<CharacterCombatLive>());
            locomotionBlends.Add(characters[i].GetComponent<LocomotionBlend>());
            nPCBehavior.Add(characters[i].GetComponent<NPCBehavior>());
            characterControllers.Add(characters[i].GetComponent<CharacterController>());
            capsuleCollider.Add(characters[i].GetComponent<CapsuleCollider>());
        }
        skillAction.Add(InputSystem.actions.FindAction("1"));
        skillAction.Add(InputSystem.actions.FindAction("2"));
        skillAction.Add(InputSystem.actions.FindAction("3"));
        skillAction.Add(InputSystem.actions.FindAction("4"));
        qAction = InputSystem.actions.FindAction("Q");
        eAction = InputSystem.actions.FindAction("E");
        mouseLeftAction = InputSystem.actions.FindAction("MouseLeft");
        mouseRightAction = InputSystem.actions.FindAction("MouseRight");
        altAction = InputSystem.actions.FindAction("Alt");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void OnEnable()
    {
        for(int i = 0, len = skillAction.Count; i < len; i++)
        {
            int index = i;
            skillAction[i].Enable();
            skillAction[i].performed += ctx => Skill(ctx, index);
        }
        qAction.Enable();
        qAction.performed += ChangeMain;
        eAction.Enable();
        eAction.performed += Combo;
        mouseLeftAction.Enable();
        mouseLeftAction.performed += BasicAttack;
        mouseLeftAction.canceled += BasicAttackCancel;
        mouseRightAction.Enable();
        mouseRightAction.performed += SprintAndEvade;
        altAction.Enable();
        altAction.performed += ChangeMouseMode;
    }
    void OnDisable()
    {
        qAction.performed -= ChangeMain;
        qAction.Disable();
        eAction.performed -= Combo;
        eAction.Disable();
        mouseLeftAction.performed -= BasicAttack;
        mouseLeftAction.canceled -= BasicAttackCancel;
        mouseLeftAction.Disable();
        mouseRightAction.performed -= SprintAndEvade;
        mouseRightAction.Disable();
        altAction.performed -= ChangeMouseMode;
        altAction.Disable();
    }
    private void Update()
    {
        for (int i = 0; i < comboOrder.Count; i++) 
        {
            if (comboOrderTime[i] > 0)
            {
                comboOrderTime[i] -= Time.deltaTime;
            }
            if (comboOrderTime[i] <= 0)
            {
                comboOrderTime.RemoveAt(i);
                comboOrder.RemoveAt(i);
                i--;
            }
        }
        OnComboOrderChanged?.Invoke();
    }
    void Skill(InputAction.CallbackContext context, int i)
    {
        if (isUsingUI)
        {
            return;
        }
        if (context.performed)
        {
            if (context.interaction is UnityEngine.InputSystem.Interactions.TapInteraction)
            {
                characterCombatLives[i].PressShortNumSkill();
            }
            else if (context.interaction is UnityEngine.InputSystem.Interactions.HoldInteraction)
            {
                characterCombatLives[i].PressLongNumSkill();
            }
        }
    }
    void ChangeMain(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        int nextControl = (mainControl + 1) % 4;
        if (animators[mainControl].GetCurrentAnimatorStateInfo(0).IsTag("Skill") || animators[nextControl].GetCurrentAnimatorStateInfo(0).IsTag("Skill"))
        {
            return;
        }
        int lastControl = mainControl;
        mainControl = nextControl;

        float direction = locomotionBlends[lastControl].direction;
        locomotionBlends[lastControl].direction = 0;
        float speed = locomotionBlends[lastControl].speed;
        locomotionBlends[lastControl].speed = 0;
        bool basicAttack = characterCombatLives[lastControl].basicAttack;

        locomotionBlends[lastControl].enabled = false;
        animators[lastControl].SetFloat("direction", 0);
        animators[lastControl].SetFloat("speed", 0);
        animators[lastControl].SetBool("basicAttack", false);
        animators[lastControl].SetBool("isPlayer", false);
        nPCBehavior[lastControl].enabled = true;

        for (int i = 0, len = 4; i < len; i++)
        {
            nPCBehavior[i].MainControl(mainControl);
        }
        locomotionBlends[mainControl].enabled = true;
        locomotionBlends[mainControl].direction = direction;
        locomotionBlends[mainControl].speed = speed;
        animators[mainControl].SetBool("basicAttack", basicAttack);
        animators[mainControl].SetInteger("attackCount", 0);
        animators[mainControl].SetBool("isPlayer", true);

        if (animators[mainControl].GetCurrentAnimatorStateInfo(0).IsTag("basic"))
        {
            animators[mainControl].SetTrigger("ToMain");
            animators[mainControl].SetTrigger("Exit");
        }
        

        nPCBehavior[mainControl].enabled = false;

        characterControllers[lastControl].enabled = false;
        characterControllers[mainControl].enabled = false;
        capsuleCollider[lastControl].enabled = false;
        capsuleCollider[mainControl].enabled = false;
        Vector3 pos = characters[lastControl].transform.position;
        characters[lastControl].transform.position = characters[mainControl].transform.position;
        characters[mainControl].transform.position = pos;
        Quaternion rot = characters[lastControl].transform.rotation;
        characters[lastControl].transform.rotation = characters[mainControl].transform.rotation;
        characters[mainControl].transform.rotation = rot;
        characterControllers[lastControl].enabled = true;
        characterControllers[mainControl].enabled = true;
        capsuleCollider[lastControl].enabled = true;
        capsuleCollider[mainControl].enabled = true;

        hW2_Camera.MainControl(mainControl);
        PlaceManager.MainControl = mainControl;
        OnHpChanged?.Invoke();
    }
    void Combo(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        if (comboOrder.Count > 0)
        {
            if (characterCombatLives[comboOrder[0]].PressCombo())
            {
                //comboOrder.Add(comboOrder[0]);
                comboOrderTime.RemoveAt(0);
                comboOrder.RemoveAt(0);
            }
        }
    }
    void BasicAttack(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        characterCombatLives[mainControl].PressMouseLeft();
    }
    void BasicAttackCancel(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        characterCombatLives[mainControl].PressCancelMouseLeft();
    }
    public void BasicAttackCancel()
    {
        characterCombatLives[mainControl].PressCancelMouseLeft();
    }
    void SprintAndEvade(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        locomotionBlends[mainControl].Sprint();
    }
    void ChangeMouseMode(InputAction.CallbackContext context)
    {
        if (isUsingUI)
        {
            return;
        }
        if (context.performed)
        {
            // 檢查目前是否已經鎖定
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // 如果目前是鎖定的 -> 解鎖並顯示
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // 如果目前是開啟的 -> 鎖定到畫面中央並隱藏
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void ComboAdd(int id)
    {
        if (comboOrder.IndexOf(id) != -1)
            comboOrderTime.Remove(comboOrder.IndexOf(id));
        comboOrder.Remove(id);
        comboOrderTime.Add(comboSkillTempTimeMax);
        comboOrder.Add(id);
        OnComboOrderChanged?.Invoke();
        Debug.LogWarning("combo " + id);
    }
    public void ComboRemove(int id)
    {
        if (comboOrder.IndexOf(id) != -1) 
            comboOrderTime.Remove(comboOrder.IndexOf(id));
        comboOrder.Remove(id);
        OnComboOrderChanged?.Invoke();
        Debug.LogWarning("combo remove " + id);
    }
}
