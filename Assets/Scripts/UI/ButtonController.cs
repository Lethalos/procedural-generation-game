using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
public class ButtonController : MonoBehaviour
{
    public GameObject infoPanelParent;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        Debug.Log("Info button pressed!");
        if (GameManager.Instance.currentGameState == GameState.Idle)
        {
            GameManager.Instance.ChangeGameState(GameState.Info);
            InputManager.Instance.lastInputTime = Time.time;
            UIManager.Instance.idleObjectParent.gameObject.SetActive(false);
            UIManager.Instance.ShowInfoPage();
           
         
        }
        else if(GameManager.Instance.currentGameState == GameState.Active)
        {
            GameManager.Instance.ChangeGameState(GameState.Info);
            InputManager.Instance.lastInputTime = Time.time;
            PoolManager.Instance.DeactivateAllPoolObjects();
            UIManager.Instance.ShowInfoPage();
          
            
           
        }
        else
        {
            UIManager.Instance.PlayUIInfoPanel();
        }

      
    }
}
