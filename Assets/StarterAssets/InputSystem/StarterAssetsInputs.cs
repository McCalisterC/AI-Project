using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header ("UI Values")]
		public RadialMenu weaponWheel;

		[Header("Weapon Values")]
		public GameObject weaponParent;
		public IWeapons currentWeapon;

		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool crouch;
		public bool fire;
		public bool knife;
		public bool utility;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		//Other Stuff
		private PlayerInteractionScript playerInteract;
		public bool pauseInputs = false;

		private void Awake() {
			if(playerInteract == null){
				playerInteract = this.GetComponent<PlayerInteractionScript>();
			}
			currentWeapon = weaponParent.GetComponentInChildren<IWeapons>();
		}

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			if(!pauseInputs)
				MoveInput(value.Get<Vector2>());
			else{
				MoveInput(Vector2.zero);
			}
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				if(!pauseInputs)
					LookInput(value.Get<Vector2>());
				else{
					LookInput(Vector2.zero);
				}
			}
		}

		public void OnJump(InputValue value)
		{
			if(!pauseInputs)
				JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if(!pauseInputs)
				SprintInput(value.isPressed);
		}
		public void OnFire(InputValue value)
		{
			if(!pauseInputs)	
				FireInput(value.isPressed);
    	}
		public void OnKnife(InputValue value)
		{
			if(!pauseInputs)	
				KnifeInput(value.isPressed);
    	}
		public void OnUtility(InputValue value)
		{
			if(!pauseInputs)	
				UtilityInput(value.isPressed);
		}
		public void OnReload(InputValue value){
			if(!pauseInputs)
				currentWeapon.ButtonReload();
		}
		public void OnInteract(InputValue value){
			if(!pauseInputs)
				playerInteract.Interact();
		}
		public void OnWeaponWheel(InputValue value){
			weaponWheel.Toggle();
		}

		public void OnCrouch(InputValue value){
			if(!pauseInputs)
				CrouchInput(value.isPressed);
		}
		
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void CrouchInput(bool newCrouchState)
		{
			crouch = newCrouchState;
		}
		
		public void FireInput(bool newFireState){
			fire = newFireState;
		}

		public void KnifeInput(bool newKnifeState){
			knife = newKnifeState;
		}

		public void UtilityInput(bool newUtilState){
			utility = newUtilState;
		}
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}