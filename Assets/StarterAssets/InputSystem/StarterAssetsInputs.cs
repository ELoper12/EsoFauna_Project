using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

        public GameObject container;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
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

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

        private void Awake() // 컨테이너의 활성화에 따른 조작
        {
			ThirdPersonController.Instance.LockCameraPosition = container.activeInHierarchy;// 인벤이 켜지면 캠고정 / 일종의 비례관계
            cursorLocked = !container.activeInHierarchy;//인벤이 켜지면 커서는 비고정, 반비례관계
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))//지금 이건 임시지만 좀 더 콤팩트하게, 정확하게는 에러가능성을 없애는 것으로
            {
                container.SetActive(!container.activeInHierarchy);
				cursorLocked = !container.activeInHierarchy;
				SetCursorState(cursorLocked);
                ThirdPersonController.Instance.LockCameraPosition = container.activeInHierarchy;
            }
        }
    }
	
}