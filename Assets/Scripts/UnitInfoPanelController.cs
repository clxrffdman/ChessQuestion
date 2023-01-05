using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnitInfoPanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI unitDescriptionText;


    public void Start()
    {
        ChessManager.Instance.OnUnitSelectedEvent.AddListener(UpdateInfoPanelUI);
    }

    public void UpdateInfoPanelUI()
    {
        BaseUnit unitToDisplay = ChessManager.Instance.selectedUnit;
        if (unitToDisplay == null)
        {
            ResetInfoPanel();
            return;
        }

        panel.SetActive(true);
        unitNameText.text = ChessManager.Instance.selectedUnit.displayName;
        unitDescriptionText.text = ChessManager.Instance.selectedUnit.description;
    }

    void ResetInfoPanel()
    {
        panel.SetActive(false);
        unitNameText.text = "";
        unitDescriptionText.text = "";
    }


}
