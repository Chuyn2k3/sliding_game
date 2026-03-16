using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] buttons; 
    
    // 1. Thêm biến này để chứa bảng Win
    public GameObject winPanel; 
    
    private Transform[] tiles = new Transform[9];
    private int emptyIndex = 8; 

    void Start()
    {
        // Đảm bảo WinPanel bị tắt khi mới mở game
        if (winPanel != null) winPanel.SetActive(false); 

        for (int i = 0; i < 9; i++)
        {
            tiles[i] = buttons[i].transform;
            
            TextMeshProUGUI txt = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null)
            {
                if (i < 8) 
                {
                    txt.text = (i + 1).ToString(); 
                }
                else 
                {
                    txt.text = ""; 
                    buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 0); 
                }
            }

            Transform currentTile = tiles[i]; 
            buttons[i].onClick.AddListener(() => OnTileClick(currentTile));
        }
RestartGame();
       // ShuffleBoard();
    }
// --- HÀM MỚI: DÙNG ĐỂ CHƠI LẠI TỪ ĐẦU ---
    public void RestartGame()
    {
        // 1. Tắt bảng Win đi
        if (winPanel != null) winPanel.SetActive(false);
        
        // 2. Trộn lại bảng để bắt đầu ván mới
        ShuffleBoard();
    }
    void OnTileClick(Transform clickedTile)
    {
        int clickedIndex = System.Array.IndexOf(tiles, clickedTile);
        if (clickedIndex == emptyIndex) return;

        bool moved = false;

        if (clickedIndex / 3 == emptyIndex / 3)
        {
            int step = (clickedIndex < emptyIndex) ? 1 : -1;
            for (int i = emptyIndex; i != clickedIndex; i -= step)
            {
                SwapTiles(i, i - step);
            }
            emptyIndex = clickedIndex;
            moved = true;
        }
        else if (clickedIndex % 3 == emptyIndex % 3)
        {
            int step = (clickedIndex < emptyIndex) ? 3 : -3;
            for (int i = emptyIndex; i != clickedIndex; i -= step)
            {
                SwapTiles(i, i - step);
            }
            emptyIndex = clickedIndex;
            moved = true;
        }

        if (moved)
        {
            UpdateVisuals();
            
            if (CheckWinCondition())
            {
                // 2. Bật WinPanel sáng lên giữa màn hình khi thắng
                if (winPanel != null) winPanel.SetActive(true);
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
        {
            tiles[i].SetSiblingIndex(i); 
        }
    }

    void ShuffleBoard()
    {
        for (int i = 0; i < 150; i++)
        {
            int row = emptyIndex / 3;
            int col = emptyIndex % 3;
            
            System.Collections.Generic.List<int> validMoves = new System.Collections.Generic.List<int>();
            
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
        for (int i = 0; i < 9; i++)
        {
            if (tiles[i] != buttons[i].transform)
            {
                return false;
            }
        }
        return true;
    }
}