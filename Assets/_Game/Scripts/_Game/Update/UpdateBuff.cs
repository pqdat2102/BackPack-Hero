using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpdateBuff : MonoBehaviour
{
    [SerializeField] private GameObject popupPanel; // Panel của popup
    [SerializeField] private Button option1Button; // Nút cho lựa chọn 1
    [SerializeField] private Button option2Button; // Nút cho lựa chọn 2
    [SerializeField] private Button option3Button; // Nút cho lựa chọn 3
    [SerializeField] private TextMeshProUGUI option1Text; // Text hiển thị lựa chọn 1
    [SerializeField] private TextMeshProUGUI option2Text; // Text hiển thị lựa chọn 2
    [SerializeField] private TextMeshProUGUI option3Text; // Text hiển thị lựa chọn 3

    private Dice dice; // Tham chiếu đến Dice để chỉnh sửa faceTexts
    private System.Action onUpgradeSelected; // Callback khi người chơi chọn nâng cấp

    private void Awake()
    {
        // Ẩn popup ban đầu
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }

        // Gán sự kiện cho các nút
        if (option1Button != null) option1Button.onClick.AddListener(() => OnOptionSelected(0));
        if (option2Button != null) option2Button.onClick.AddListener(() => OnOptionSelected(1));
        if (option3Button != null) option3Button.onClick.AddListener(() => OnOptionSelected(2));
    }

    // Hiển thị popup với các lựa chọn nâng cấp
    public void ShowPopup(Dice diceReference, System.Action onComplete)
    {
        dice = diceReference;
        onUpgradeSelected = onComplete;

        // Hiển thị popup
        if (popupPanel != null)
        {
            popupPanel.SetActive(true);
        }

        // Thiết lập các lựa chọn nâng cấp (có thể tùy chỉnh)
        option1Text.text = "Tăng 3 mặt +2"; // Tăng giá trị của 3 mặt ngẫu nhiên lên 2
        option2Text.text = "Tăng 6 mặt +1"; // Tăng giá trị của tất cả 6 mặt lên 1
        option3Text.text = "Tăng 1 mặt +3"; // Tăng giá trị của 1 mặt ngẫu nhiên lên 3
    }

    // Ẩn popup
    private void HidePopup()
    {
        if (popupPanel != null)
        {
            popupPanel.SetActive(false);
        }
    }

    // Xử lý khi người chơi chọn một tùy chọn
    private void OnOptionSelected(int optionIndex)
    {
        switch (optionIndex)
        {
            case 0: // Tăng 3 mặt +2
                UpgradeRandomFaces(3, 2);
                break;
            case 1: // Tăng 6 mặt +1
                UpgradeAllFaces(1);
                break;
            case 2: // Tăng 1 mặt +3
                UpgradeRandomFaces(1, 3);
                break;
        }

        // Ẩn popup sau khi chọn
        HidePopup();

        // Gọi callback để tiếp tục game
        onUpgradeSelected?.Invoke();
    }

    // Tăng giá trị cho một số mặt ngẫu nhiên
    private void UpgradeRandomFaces(int numberOfFaces, int amount)
    {
        List<int> faceIndices = new List<int> { 0, 1, 2, 3, 4, 5 };
        faceIndices.Shuffle();

        for (int i = 0; i < numberOfFaces && i < faceIndices.Count; i++)
        {
            int faceIndex = faceIndices[i];
            int currentValue = int.Parse(dice.GetComponent<Dice>().faceTexts[faceIndex].text);
            currentValue += amount;
            dice.GetComponent<Dice>().faceTexts[faceIndex].text = currentValue.ToString();
            Debug.Log($"Mặt {faceIndex + 1} tăng giá trị lên {amount}. Giá trị mới: {currentValue}");
        }
    }

    // Tăng giá trị cho tất cả các mặt
    private void UpgradeAllFaces(int amount)
    {
        for (int i = 0; i < dice.GetComponent<Dice>().faceTexts.Count; i++)
        {
            int currentValue = int.Parse(dice.GetComponent<Dice>().faceTexts[i].text);
            currentValue += amount;
            dice.GetComponent<Dice>().faceTexts[i].text = currentValue.ToString();
            Debug.Log($"Mặt {i + 1} tăng giá trị lên {amount}. Giá trị mới: {currentValue}");
        }
    }
}

// Extension method để xáo trộn danh sách (dùng cho UpgradeRandomFaces)
public static class ListExtensions
{
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}