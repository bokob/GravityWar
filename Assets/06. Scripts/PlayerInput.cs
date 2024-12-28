using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveKeyInput;    // 방향키 입력값
    public Vector2 moveMouseInput;  // 마우스 움직임
    public bool isClickRight;

    void Update()
    {
        moveKeyInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X"); // 마우스 좌우
        float mouseY = Input.GetAxis("Mouse Y"); // 마우스 위아래
        moveMouseInput = new Vector2(mouseX, mouseY);
    }

    void LateUpdate()
    {
    }
}