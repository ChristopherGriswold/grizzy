using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player name input field. Let the user input his name, will appear above the player in the game.
/// </summary>
[RequireComponent(typeof(InputField))]
public class PlayerNameInputField : MonoBehaviour
{
  //  private PlayerData playerData;

    /// <summary>
    /// MonoBehaviour method called on GameObject by Unity during initialization phase.
    /// </summary>
    void Start()
    {

   //     playerData = GameObject.Find("GameManager").GetComponent<DataHandler>().playerData;
        string defaultName = "";
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if (PlayerPrefs.HasKey("PlayerName"))
            {
                defaultName = PlayerPrefs.GetString("PlayerName");
                _inputField.text = defaultName;
            }
        }

    }

    /// <summary>
    /// Sets the name of the player and save it in the PlayerPrefs for future sessions.
    /// </summary>
    /// <param name="value">The name of the Player</param>
    public void SetPlayerName(string value)
    {
        if(PlayerPrefs.GetString("PlayerName") != value)
        {
            PlayerPrefs.DeleteAll();
        }


        PlayerPrefs.SetString("PlayerName", value);
    }
}
