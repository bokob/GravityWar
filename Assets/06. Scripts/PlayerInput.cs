using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector3 moveKeyInput;    // ����Ű �Է°�
    public Vector2 moveMouseInput;  // ���콺 ������
    public bool isClickRight;

    void Update()
    {
        moveKeyInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X"); // ���콺 �¿�
        float mouseY = Input.GetAxis("Mouse Y"); // ���콺 ���Ʒ�
        moveMouseInput = new Vector2(mouseX, mouseY);
    }

    void LateUpdate()
    {
    }
}