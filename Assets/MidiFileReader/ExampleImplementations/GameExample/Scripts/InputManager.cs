using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public EnemySpawner enemySpawner;
	private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerController.instance.Dodge(Direction.Up);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            PlayerController.instance.SetKeyUp();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerController.instance.Dodge(Direction.Down);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            PlayerController.instance.SetKeyUp();
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerController.instance.Dodge(Direction.Left);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            PlayerController.instance.SetKeyUp();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            PlayerController.instance.Dodge(Direction.Right);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            PlayerController.instance.SetKeyUp();
        }
    }
}
