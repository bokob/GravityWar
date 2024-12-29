using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveKeyInput;    // wasd 입력값 (월드 좌표)
    public Vector3 moveArrowInput;  // 화살표 입력값 (월드 좌표)
    public Vector2 moveMouseInput;  // 마우스 움직임
    public float mouseScroll;
    public bool isClickMouseWheel;
    public bool isClickMouseRight;

    void Update()
    {
        // 키보드 입력
        moveKeyInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // 키보드 방향키 누르면 앞뒤좌우 이동
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) dir += Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow)) dir += Vector3.back;
        if (Input.GetKey(KeyCode.LeftArrow)) dir += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) dir += Vector3.right;
        moveArrowInput = dir;

        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X");            // 마우스 좌우
        float mouseY = Input.GetAxis("Mouse Y");            // 마우스 위아래
        moveMouseInput = new Vector2(mouseX, mouseY);
        isClickMouseRight = Input.GetMouseButton(1);        // 마우스 오른쪽 누르고 있는지
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");   // 마우스 스크롤
        isClickMouseWheel = Input.GetMouseButton(2);        // 마우스 스크롤 누르고 있는지
    }
}