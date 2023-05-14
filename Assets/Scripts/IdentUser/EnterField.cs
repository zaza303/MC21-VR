using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnterField: MonoBehaviour
{
	[SerializeField]
	InputField InputFieldName, InputFieldSurName, InputFieldGroup;

	public void StartGame()
    {
		if (InputFieldName.text != "" && InputFieldSurName.text != "" && InputFieldGroup.text != "")
        {
			PlayerPrefs.SetString("GameName", InputFieldSurName.text + " " + InputFieldName.text + " (группа " + InputFieldGroup.text + ")");
			PlayerPrefs.Save();
            SceneManager.LoadScene("Menu");
		}
    }
}
