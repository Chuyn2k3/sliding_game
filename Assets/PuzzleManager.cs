using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PuzzleManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Button[] buttons;
    public GameObject winPanel;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip slideSound;
    public AudioClip winSound;      // --- THÊM BIẾN MỚI CHO ÂM THANH WIN ---

    private Transform[] tiles = new Transform[9];
    private int emptyIndex = 8;

    void Start()
    {
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
    }

    public void RestartGame()
    {
        if (winPanel != null) winPanel.SetActive(false);
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
            // Phát âm thanh khi trượt
            if (audioSource != null && slideSound != null)
            {
                audioSource.PlayOneShot(slideSound);
            }

            UpdateVisuals();

            // KIỂM TRA CHIẾN THẮNG
            if (CheckWinCondition())
            {
                // --- PHÁT ÂM THANH WIN ---
                if (audioSource != null && winSound != null)
                {
                    audioSource.PlayOneShot(winSound);
                }

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
        for (int i = 0; i < 8; i++)
        {
            TextMeshProUGUI txt = tiles[i].GetComponentInChildren<TextMeshProUGUI>();
            if (txt == null || txt.text != (i + 1).ToString())
            {
                return false;
            }
        }
        return true;
    }
}