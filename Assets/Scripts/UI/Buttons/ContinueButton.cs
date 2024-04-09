using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour
{
    private Button _thisButton;
    private DataGame _dataGame;

    private void Awake()
    {
        _thisButton = GetComponent<Button>();
        _dataGame = DataGame.Instance;
    }

    private void OnEnable()
    {
        _thisButton.interactable = !_dataGame.IsNewGame;
    }
}
