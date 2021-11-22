using UnityEngine;
using UnityEngine.SceneManagement;

public class TeoriManager : MonoBehaviour
{
    [SerializeField] private GameObject jawaban1Panel;
    [SerializeField] private GameObject jawaban2Panel;

    private void Start()
    {
        jawaban1Panel.SetActive(true);
        jawaban2Panel.SetActive(false);
    }
   
    public void OnClickBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnClickNextButton()
    {
        jawaban1Panel.SetActive(false);
        jawaban2Panel.SetActive(true);
    }

    public void OnClickPreviousButton()
    {
        jawaban1Panel.SetActive(true);
        jawaban2Panel.SetActive(false);
    }
}
