using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    private const int MaxPlayers = 4;

    [SerializeField]
    private TextMeshProUGUI label = default;

    private MatchmakingView matchmakingView;
    private Button button;

    public string RoomName { get; private set; }

    public void Init(MatchmakingView parentView, int roomId)
    {
        matchmakingView = parentView;
        RoomName = $"Room{roomId}";

        button = GetComponent<Button>();
        button.interactable = false;
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        // ルーム参加処理中は、全ての参加ボタンを押せないようにする
        matchmakingView.OnJoiningRoom();

        // ボタンに対応したルーム名のルームに参加する（ルームが存在しなければ作成してから参加する）
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = MaxPlayers;
        PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
    }

    public void SetPlayerCount(int playerCount)
    {
        label.text = $"{RoomName}\n{playerCount} / {MaxPlayers}";

        // ルームが満員でない時のみ、ルーム参加ボタンを押せるようにする
        button.interactable = (playerCount < MaxPlayers);
    }
}