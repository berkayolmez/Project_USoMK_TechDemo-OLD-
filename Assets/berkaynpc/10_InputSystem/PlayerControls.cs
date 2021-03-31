// GENERATED AUTOMATICALLY FROM 'Assets/berkaynpc/10_InputSystem/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""5fe55eae-13ba-4b4e-b258-acf58391f638"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""eb055369-b954-48ac-8b9d-bf2b6e641c73"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""e5bb0141-7502-432e-bdba-c9b00b94e3bd"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""f9e9cb73-767c-4679-97ec-46b4dd3d0458"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""de22e822-a98e-4fa7-bcc9-82fc61fe95bb"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ae038e36-b98b-49f4-888c-00bc5726fb1c"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""16063765-ed47-46ef-9c3f-8258f3f7f703"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""GamePad"",
                    ""id"": ""8eb1dc41-7ae3-440a-8478-9b31f5daa637"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""be006e7f-c115-481f-b9f2-565b0a5f8e48"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""0b2da271-181d-4cc9-bdf8-0d3ea20117ef"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""73ebfb3d-7703-431f-a6b9-e9cd18364a62"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fe770e10-af3c-44cd-9aeb-957db4b079ab"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""PlayerActions"",
            ""id"": ""1609feb8-d22f-4ed5-9a66-df41699c8cfc"",
            ""actions"": [
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""b5f54176-63ec-4476-93cf-ceb12621c206"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PushPull"",
                    ""type"": ""Button"",
                    ""id"": ""13ffa23c-e9ad-447f-ae1e-00c444a10cb2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""SpellCast"",
                    ""type"": ""Button"",
                    ""id"": ""f0037787-1de2-4fc8-95b9-50cfb96d0533"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RB_Input"",
                    ""type"": ""Button"",
                    ""id"": ""fd6d8cd5-35ef-475c-b950-07ed773a9287"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RT_Input"",
                    ""type"": ""Button"",
                    ""id"": ""8e37f7be-13b7-49e9-a248-c37577e340a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d0f02776-1509-44ed-8b87-f8da835f122f"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2ce29f50-d6db-4fa7-a043-218cea45e641"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eac63ef6-1485-4547-9b68-f4ae595b1eed"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PushPull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""eaca085d-34a6-40b2-9fc6-a0d3be28d764"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SpellCast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""58639d82-74c9-45a3-91a7-4a11357de447"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RB_Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""306624f0-908d-4b76-97c3-e7f24ac4b0f4"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RT_Input"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerQuickSlots"",
            ""id"": ""5420749a-4e8a-46df-bb71-2e50b4f24c00"",
            ""actions"": [
                {
                    ""name"": ""DpadUp"",
                    ""type"": ""Button"",
                    ""id"": ""a46c08c9-7a69-48bc-b0ac-4b2a16358333"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DpadDown"",
                    ""type"": ""Button"",
                    ""id"": ""7b3a3742-0625-4bdf-a933-213ff4f05229"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DpadRight"",
                    ""type"": ""Button"",
                    ""id"": ""8fe1d40e-1b40-478a-bee6-4f2147758936"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""DpadLeft"",
                    ""type"": ""Button"",
                    ""id"": ""2f38be88-d8c3-4645-8bdb-2e5305a2334f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e847aa28-c70f-4429-a95a-9ec4a00c59f5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8fddafa3-498e-4bf5-8b5c-71f8ab89facc"",
                    ""path"": ""<Gamepad>/dpad/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16c6e20e-0a7d-45a1-a9b4-1845e7f06bb7"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5907f212-6f5f-4070-8dcb-f425d85735e9"",
                    ""path"": ""<Gamepad>/dpad/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b6670511-3cc0-44ea-a60b-d9886b893577"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""aa489f9c-c355-467a-a377-d8f1a7fc186f"",
                    ""path"": ""<Gamepad>/dpad/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1e3cc202-92f1-4d77-bf48-df95dc4cc9c8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""90a10e94-00b8-4ad2-84a5-fb39d44393db"",
                    ""path"": ""<Gamepad>/dpad/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DpadLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        // PlayerActions
        m_PlayerActions = asset.FindActionMap("PlayerActions", throwIfNotFound: true);
        m_PlayerActions_Roll = m_PlayerActions.FindAction("Roll", throwIfNotFound: true);
        m_PlayerActions_PushPull = m_PlayerActions.FindAction("PushPull", throwIfNotFound: true);
        m_PlayerActions_SpellCast = m_PlayerActions.FindAction("SpellCast", throwIfNotFound: true);
        m_PlayerActions_RB_Input = m_PlayerActions.FindAction("RB_Input", throwIfNotFound: true);
        m_PlayerActions_RT_Input = m_PlayerActions.FindAction("RT_Input", throwIfNotFound: true);
        // PlayerQuickSlots
        m_PlayerQuickSlots = asset.FindActionMap("PlayerQuickSlots", throwIfNotFound: true);
        m_PlayerQuickSlots_DpadUp = m_PlayerQuickSlots.FindAction("DpadUp", throwIfNotFound: true);
        m_PlayerQuickSlots_DpadDown = m_PlayerQuickSlots.FindAction("DpadDown", throwIfNotFound: true);
        m_PlayerQuickSlots_DpadRight = m_PlayerQuickSlots.FindAction("DpadRight", throwIfNotFound: true);
        m_PlayerQuickSlots_DpadLeft = m_PlayerQuickSlots.FindAction("DpadLeft", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // PlayerActions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Roll;
    private readonly InputAction m_PlayerActions_PushPull;
    private readonly InputAction m_PlayerActions_SpellCast;
    private readonly InputAction m_PlayerActions_RB_Input;
    private readonly InputAction m_PlayerActions_RT_Input;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Roll => m_Wrapper.m_PlayerActions_Roll;
        public InputAction @PushPull => m_Wrapper.m_PlayerActions_PushPull;
        public InputAction @SpellCast => m_Wrapper.m_PlayerActions_SpellCast;
        public InputAction @RB_Input => m_Wrapper.m_PlayerActions_RB_Input;
        public InputAction @RT_Input => m_Wrapper.m_PlayerActions_RT_Input;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Roll.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @PushPull.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPushPull;
                @PushPull.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPushPull;
                @PushPull.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnPushPull;
                @SpellCast.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpellCast;
                @SpellCast.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpellCast;
                @SpellCast.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSpellCast;
                @RB_Input.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB_Input;
                @RB_Input.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB_Input;
                @RB_Input.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB_Input;
                @RT_Input.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT_Input;
                @RT_Input.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT_Input;
                @RT_Input.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT_Input;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @PushPull.started += instance.OnPushPull;
                @PushPull.performed += instance.OnPushPull;
                @PushPull.canceled += instance.OnPushPull;
                @SpellCast.started += instance.OnSpellCast;
                @SpellCast.performed += instance.OnSpellCast;
                @SpellCast.canceled += instance.OnSpellCast;
                @RB_Input.started += instance.OnRB_Input;
                @RB_Input.performed += instance.OnRB_Input;
                @RB_Input.canceled += instance.OnRB_Input;
                @RT_Input.started += instance.OnRT_Input;
                @RT_Input.performed += instance.OnRT_Input;
                @RT_Input.canceled += instance.OnRT_Input;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // PlayerQuickSlots
    private readonly InputActionMap m_PlayerQuickSlots;
    private IPlayerQuickSlotsActions m_PlayerQuickSlotsActionsCallbackInterface;
    private readonly InputAction m_PlayerQuickSlots_DpadUp;
    private readonly InputAction m_PlayerQuickSlots_DpadDown;
    private readonly InputAction m_PlayerQuickSlots_DpadRight;
    private readonly InputAction m_PlayerQuickSlots_DpadLeft;
    public struct PlayerQuickSlotsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerQuickSlotsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @DpadUp => m_Wrapper.m_PlayerQuickSlots_DpadUp;
        public InputAction @DpadDown => m_Wrapper.m_PlayerQuickSlots_DpadDown;
        public InputAction @DpadRight => m_Wrapper.m_PlayerQuickSlots_DpadRight;
        public InputAction @DpadLeft => m_Wrapper.m_PlayerQuickSlots_DpadLeft;
        public InputActionMap Get() { return m_Wrapper.m_PlayerQuickSlots; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerQuickSlotsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerQuickSlotsActions instance)
        {
            if (m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface != null)
            {
                @DpadUp.started -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadUp;
                @DpadUp.performed -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadUp;
                @DpadUp.canceled -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadUp;
                @DpadDown.started -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadDown;
                @DpadDown.performed -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadDown;
                @DpadDown.canceled -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadDown;
                @DpadRight.started -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadRight;
                @DpadRight.performed -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadRight;
                @DpadRight.canceled -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadRight;
                @DpadLeft.started -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.performed -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadLeft;
                @DpadLeft.canceled -= m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface.OnDpadLeft;
            }
            m_Wrapper.m_PlayerQuickSlotsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @DpadUp.started += instance.OnDpadUp;
                @DpadUp.performed += instance.OnDpadUp;
                @DpadUp.canceled += instance.OnDpadUp;
                @DpadDown.started += instance.OnDpadDown;
                @DpadDown.performed += instance.OnDpadDown;
                @DpadDown.canceled += instance.OnDpadDown;
                @DpadRight.started += instance.OnDpadRight;
                @DpadRight.performed += instance.OnDpadRight;
                @DpadRight.canceled += instance.OnDpadRight;
                @DpadLeft.started += instance.OnDpadLeft;
                @DpadLeft.performed += instance.OnDpadLeft;
                @DpadLeft.canceled += instance.OnDpadLeft;
            }
        }
    }
    public PlayerQuickSlotsActions @PlayerQuickSlots => new PlayerQuickSlotsActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnRoll(InputAction.CallbackContext context);
        void OnPushPull(InputAction.CallbackContext context);
        void OnSpellCast(InputAction.CallbackContext context);
        void OnRB_Input(InputAction.CallbackContext context);
        void OnRT_Input(InputAction.CallbackContext context);
    }
    public interface IPlayerQuickSlotsActions
    {
        void OnDpadUp(InputAction.CallbackContext context);
        void OnDpadDown(InputAction.CallbackContext context);
        void OnDpadRight(InputAction.CallbackContext context);
        void OnDpadLeft(InputAction.CallbackContext context);
    }
}
