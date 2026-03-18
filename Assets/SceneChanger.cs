using UnityEngine;
using UnityEngine.SceneManagement; // Dòng này bắt buộc phải có để chuyển Scene

public class SceneChanger : MonoBehaviour
{
    // Hàm mở màn hình Menu chính
    public void LoadStartScene()
    {
        SceneManager.LoadScene("StartScene");
    }

    // Hàm mở màn hình Hướng dẫn
    public void LoadGuideScene()
    {
        SceneManager.LoadScene("GuideScene");
    }

    // Hàm mở màn hình Chơi game
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}