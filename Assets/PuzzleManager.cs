// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;

// public class PuzzleManager : MonoBehaviour
// {
//     [Header("Screens (Các Màn Hình)")]
//     public GameObject startScreen;
//     public GameObject tutorialScreen;
//     public GameObject gameScreen; 
//     public GameObject winScreen;  

//     [Header("UI Elements")]
//     public Button[] buttons;
//     public Button muteButton;
//     public TextMeshProUGUI muteText;
    
//     // --- THÊM TÍNH NĂNG ĐẾM BƯỚC ---
//     public TextMeshProUGUI moveCountText;    // Hiện số bước ở màn hình lúc đang chơi
//     public TextMeshProUGUI winMoveCountText; // Hiện tổng số bước ở màn hình chiến thắng

//     [Header("Audio")]
//     public AudioSource audioSource;
//     public AudioClip slideSound;
//     public AudioClip winSound;

//     private Transform[] tiles = new Transform[9];
//     private int emptyIndex = 8;
//     private bool isMuted = false;
    
//     // --- THÊM BIẾN ĐẾM ---
//     private int moveCount = 0; 

//     void Start()
//     {
//         for (int i = 0; i < 9; i++)
//         {
//             tiles[i] = buttons[i].transform;
//             buttons[i].onClick.RemoveAllListeners();

//             TextMeshProUGUI txt = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
//             if (txt != null)
//             {
//                 if (i < 8) txt.text = (i + 1).ToString();
//                 else
//                 {
//                     txt.text = "";
//                     buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
//                 }
//             }

//             Transform currentTile = tiles[i];
//             buttons[i].onClick.AddListener(() => {
//                 if (this != null) OnTileClick(currentTile);
//             });
//         }

//         if (muteButton != null)
//         {
//             muteButton.onClick.RemoveAllListeners();
//             muteButton.onClick.AddListener(ToggleMute);
//             UpdateMuteUI();
//         }

//         OpenStartScreen();
//     }

//     void OnDestroy()
//     {
//         if (buttons != null)
//         {
//             foreach (var btn in buttons)
//                 if (btn != null) btn.onClick.RemoveAllListeners();
//         }
//         if (muteButton != null) muteButton.onClick.RemoveAllListeners();
//     }

//     public void OpenStartScreen()
//     {
//         if (startScreen) startScreen.SetActive(true);
//         if (tutorialScreen) tutorialScreen.SetActive(false);
//         if (gameScreen) gameScreen.SetActive(false);
//         if (winScreen) winScreen.SetActive(false);
//     }

//     public void OpenTutorialScreen()
//     {
//         if (startScreen) startScreen.SetActive(false);
//         if (tutorialScreen) tutorialScreen.SetActive(true);
//         if (gameScreen) gameScreen.SetActive(false);
//         if (winScreen) winScreen.SetActive(false);
//     }

//     public void StartGame()
//     {
//         if (startScreen) startScreen.SetActive(false);
//         if (tutorialScreen) tutorialScreen.SetActive(false);
//         if (gameScreen) gameScreen.SetActive(true); 
//         if (winScreen) winScreen.SetActive(false);

//         // --- RESET LẠI BƯỚC ĐẾM VỀ 0 KHI BẮT ĐẦU CHƠI ---
//         moveCount = 0;
//         UpdateMoveCountUI();

//         ShuffleBoard(); 
//     }

//     public void ToggleMute()
//     {
//         if (this == null) return;
//         isMuted = !isMuted;
//         if (audioSource != null) audioSource.mute = isMuted;
//         UpdateMuteUI();
//     }

//     void UpdateMuteUI()
//     {
//         if (muteText != null) muteText.text = isMuted ? "Sound: OFF" : "Sound: ON";
//     }

//     // --- CẬP NHẬT CHỮ HIỂN THỊ SỐ BƯỚC ---
//     void UpdateMoveCountUI()
//     {
//         if (moveCountText != null) 
//             moveCountText.text = "Moves: " + moveCount;
            
//         if (winMoveCountText != null) 
//             winMoveCountText.text = "Tổng số bước: " + moveCount;
//     }

//     void OnTileClick(Transform clickedTile)
//     {
//         if (this == null || clickedTile == null) return;

//         int clickedIndex = System.Array.IndexOf(tiles, clickedTile);
//         if (clickedIndex == -1 || clickedIndex == emptyIndex) return;

//         bool moved = false;
//         if (clickedIndex / 3 == emptyIndex / 3)
//         {
//             int step = (clickedIndex < emptyIndex) ? 1 : -1;
//             for (int i = emptyIndex; i != clickedIndex; i -= step) SwapTiles(i, i - step);
//             emptyIndex = clickedIndex;
//             moved = true;
//         }
//         else if (clickedIndex % 3 == emptyIndex % 3)
//         {
//             int step = (clickedIndex < emptyIndex) ? 3 : -3;
//             for (int i = emptyIndex; i != clickedIndex; i -= step) SwapTiles(i, i - step);
//             emptyIndex = clickedIndex;
//             moved = true;
//         }

//         if (moved)
//         {
//             // --- TĂNG SỐ BƯỚC LÊN 1 VÀ CẬP NHẬT GIAO DIỆN ---
//             moveCount++;
//             UpdateMoveCountUI();

//             if (!isMuted && audioSource != null && slideSound != null)
//                 audioSource.PlayOneShot(slideSound);

//             UpdateVisuals();

//             if (CheckWinCondition())
//             {
//                 if (!isMuted && audioSource != null && winSound != null)
//                     audioSource.PlayOneShot(winSound);
                
//                 if (gameScreen) gameScreen.SetActive(false); 
//                 if (winScreen) winScreen.SetActive(true);    
//             }
//         }
//     }

//     void SwapTiles(int indexA, int indexB)
//     {
//         Transform temp = tiles[indexA];
//         tiles[indexA] = tiles[indexB];
//         tiles[indexB] = temp;
//     }

//     void UpdateVisuals()
//     {
//         for (int i = 0; i < 9; i++)
//             if (tiles[i] != null) tiles[i].SetSiblingIndex(i);
//     }

//     void ShuffleBoard()
//     {
//         for (int i = 0; i < 150; i++)
//         {
//             int row = emptyIndex / 3, col = emptyIndex % 3;
//             var validMoves = new System.Collections.Generic.List<int>();
//             if (row > 0) validMoves.Add(emptyIndex - 3);
//             if (row < 2) validMoves.Add(emptyIndex + 3);
//             if (col > 0) validMoves.Add(emptyIndex - 1);
//             if (col < 2) validMoves.Add(emptyIndex + 1);
//             int randomMove = validMoves[Random.Range(0, validMoves.Count)];
//             SwapTiles(emptyIndex, randomMove);
//             emptyIndex = randomMove;
//         }
//         UpdateVisuals();
//     }

//     bool CheckWinCondition()
//     {
//         for (int i = 0; i < 8; i++)
//         {
//             if (tiles[i] == null) return false;
//             var txt = tiles[i].GetComponentInChildren<TextMeshProUGUI>();
//             if (txt == null || txt.text != (i + 1).ToString()) return false;
//         }
//         return true;
//     }
// }
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("Screens (Các Màn Hình)")]
    public GameObject gameScreen; // Màn chứa 9 nút bấm chơi game
    public GameObject winScreen;  // Màn hình hiển thị khi thắng

    [Header("UI Elements")]
    public Button[] buttons;
    public Button muteButton;
    public TextMeshProUGUI muteText;

    [Header("Đếm Bước")]
    public TextMeshProUGUI moveCountText;    // Hiện số bước ở màn hình lúc đang chơi
    public TextMeshProUGUI winMoveCountText; // Hiện tổng số bước ở màn hình chiến thắng

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip slideSound;
    public AudioClip winSound;

    private Transform[] tiles = new Transform[9];
    private int emptyIndex = 8;
    private bool isMuted = false;
    private int moveCount = 0; 

    void Start()
    {
        // Khởi tạo các nút game
        for (int i = 0; i < 9; i++)
        {
            tiles[i] = buttons[i].transform;
            buttons[i].onClick.RemoveAllListeners();

            TextMeshProUGUI txt = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                if (i < 8) txt.text = (i + 1).ToString();
                else
                {
                    txt.text = "";
                    buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
                }
            }

            Transform currentTile = tiles[i];
            buttons[i].onClick.AddListener(() => {
                if (this != null) OnTileClick(currentTile);
            });
        }

        // Khởi tạo nút Mute
        if (muteButton != null)
        {
            muteButton.onClick.RemoveAllListeners();
            muteButton.onClick.AddListener(ToggleMute);
            UpdateMuteUI();
        }

        // --- GỌI NGAY MÀN CHƠI KHI VÀO GAMESCENE ---
        StartGame();
    }

    void OnDestroy()
    {
        if (buttons != null)
        {
            foreach (var btn in buttons)
                if (btn != null) btn.onClick.RemoveAllListeners();
        }
        if (muteButton != null) muteButton.onClick.RemoveAllListeners();
    }

    // ==========================================
    // LOGIC CHUYỂN MÀN VÀ BẮT ĐẦU GAME
    // ==========================================

    public void StartGame()
    {
        if (gameScreen) gameScreen.SetActive(true); 
        if (winScreen) winScreen.SetActive(false);

        // Reset lại bước đếm về 0 khi bắt đầu chơi
        moveCount = 0;
        UpdateMoveCountUI();

        ShuffleBoard(); 
    }

    // ==========================================
    // LOGIC ÂM THANH & GIAO DIỆN
    // ==========================================

    public void ToggleMute()
    {
        if (this == null) return;
        isMuted = !isMuted;
        if (audioSource != null) audioSource.mute = isMuted;
        UpdateMuteUI();
    }

    void UpdateMuteUI()
    {
        if (muteText != null) muteText.text = isMuted ? "Sound: OFF" : "Sound: ON";
    }

    void UpdateMoveCountUI()
    {
        if (moveCountText != null) 
            moveCountText.text = "Moves: " + moveCount;
            
        if (winMoveCountText != null) 
            winMoveCountText.text = "Tổng số bước: " + moveCount;
    }

    // ==========================================
    // LOGIC TRÒ CHƠI (TRƯỢT GẠCH)
    // ==========================================

    void OnTileClick(Transform clickedTile)
    {
        if (this == null || clickedTile == null) return;

        int clickedIndex = System.Array.IndexOf(tiles, clickedTile);
        if (clickedIndex == -1 || clickedIndex == emptyIndex) return;

        bool moved = false;
        if (clickedIndex / 3 == emptyIndex / 3)
        {
            int step = (clickedIndex < emptyIndex) ? 1 : -1;
            for (int i = emptyIndex; i != clickedIndex; i -= step) SwapTiles(i, i - step);
            emptyIndex = clickedIndex;
            moved = true;
        }
        else if (clickedIndex % 3 == emptyIndex % 3)
        {
            int step = (clickedIndex < emptyIndex) ? 3 : -3;
            for (int i = emptyIndex; i != clickedIndex; i -= step) SwapTiles(i, i - step);
            emptyIndex = clickedIndex;
            moved = true;
        }

        if (moved)
        {
            // Tăng số bước lên 1 và cập nhật giao diện
            moveCount++;
            UpdateMoveCountUI();

            if (!isMuted && audioSource != null && slideSound != null)
                audioSource.PlayOneShot(slideSound);

            UpdateVisuals();

            if (CheckWinCondition())
            {
                if (!isMuted && audioSource != null && winSound != null)
                    audioSource.PlayOneShot(winSound);
                
                // Chuyển sang màn hình Win
                if (gameScreen) gameScreen.SetActive(false); 
                if (winScreen) winScreen.SetActive(true);    
            }
        }
    }

    void SwapTiles(int indexA, int indexB)
    {
        Transform temp = tiles[indexA];
        tiles[indexA] = tiles[indexB];
        tiles[indexB] = temp;
    }

    void UpdateVisuals()
    {
        for (int i = 0; i < 9; i++)
            if (tiles[i] != null) tiles[i].SetSiblingIndex(i);
    }

    void ShuffleBoard()
    {
        for (int i = 0; i < 150; i++)
        {
            int row = emptyIndex / 3, col = emptyIndex % 3;
            var validMoves = new System.Collections.Generic.List<int>();
            if (row > 0) validMoves.Add(emptyIndex - 3);
            if (row < 2) validMoves.Add(emptyIndex + 3);
            if (col > 0) validMoves.Add(emptyIndex - 1);
            if (col < 2) validMoves.Add(emptyIndex + 1);
            int randomMove = validMoves[Random.Range(0, validMoves.Count)];
            SwapTiles(emptyIndex, randomMove);
            emptyIndex = randomMove;
        }
        UpdateVisuals();
    }

    bool CheckWinCondition()
    {
        for (int i = 0; i < 8; i++)
        {
            if (tiles[i] == null) return false;
            var txt = tiles[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null || txt.text != (i + 1).ToString()) return false;
        }
        return true;
    }
}