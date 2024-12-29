using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveKeyInput;    // wasd �Է°� (���� ��ǥ)
    public Vector3 moveArrowInput;  // ȭ��ǥ �Է°� (���� ��ǥ)
    public Vector2 moveMouseInput;  // ���콺 ������
    public float mouseScroll;
    public bool isClickMouseWheel;
    public bool isClickMouseRight;

    void Update()
    {
        // Ű���� �Է�
        moveKeyInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // Ű���� ����Ű ������ �յ��¿� �̵�
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) dir += Vector3.forward;
        if (Input.GetKey(KeyCode.DownArrow)) dir += Vector3.back;
        if (Input.GetKey(KeyCode.LeftArrow)) dir += Vector3.left;
        if (Input.GetKey(KeyCode.RightArrow)) dir += Vector3.right;
        moveArrowInput = dir;

        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X");            // ���콺 �¿�
        float mouseY = Input.GetAxis("Mouse Y");            // ���콺 ���Ʒ�
        moveMouseInput = new Vector2(mouseX, mouseY);
        isClickMouseRight = Input.GetMouseButton(1);        // ���콺 ������ ������ �ִ���
        mouseScroll = Input.GetAxis("Mouse ScrollWheel");   // ���콺 ��ũ��
        isClickMouseWheel = Input.GetMouseButton(2);        // ���콺 ��ũ�� ������ �ִ���
    }
}