using UnityEngine;

public class Exit : MonoBehaviour
{
    void Update()
    {
        //Se o jogador apertar Esc, fechar a janela.
        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }
}
