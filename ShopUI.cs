using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("Shop Scene")]
    public TextMeshProUGUI moreFuelPriceText;
    public Button moreFuel1;
    public Button moreFuel2;
    public Button moreFuel3;

    public TextMeshProUGUI moreThrustPriceText;
    public Button moreThrust1;
    public Button moreThrust2;
    public Button moreThrust3;

    public TextMeshProUGUI moreControlPriceText;
    public Button moreControl1;
    public Button moreControl2;
    
    public TextMeshProUGUI radioPriceText;
    public Button radio1;
    public Button radio2;

    public List<int> playersDone;

    public Dictionary<Upgrades, int> costs = new Dictionary<Upgrades, int>(){
        {Upgrades.FUEL_1, 2},
        {Upgrades.FUEL_2, 4},
        {Upgrades.FUEL_3, 10},

        {Upgrades.THRUST_1, 2},
        {Upgrades.THRUST_2, 4},
        {Upgrades.THRUST_3, 10},

        {Upgrades.CONTROL_1, 6},
        {Upgrades.CONTROL_2, 10},

        {Upgrades.RADIO_1, 6},
        {Upgrades.RADIO_2, 10},
    };

    
    [Header("Misc. Scene")]
    public TextMeshProUGUI playerMoneyText;
    public Button doneButton;

    public Color hasUpgradeColor;
    public Color upgradeElgibleColor;
    public Color upgradeInelgibleColor;

    public void ButtonClicked(int upgradeId, int upgradeIdx){
        Upgrades upgradeSelected = Upgrades.FUEL_1;

        if(upgradeId == 0){
            if(upgradeIdx == 1)
                upgradeSelected = Upgrades.FUEL_1;
            if(upgradeIdx == 2)
                upgradeSelected = Upgrades.FUEL_2;
            if(upgradeIdx == 3)
                upgradeSelected = Upgrades.FUEL_3;
        }
        if(upgradeId == 1){
            if(upgradeIdx == 1)
                upgradeSelected = Upgrades.THRUST_1;
            if(upgradeIdx == 2)
                upgradeSelected = Upgrades.THRUST_2;
            if(upgradeIdx == 3)
                upgradeSelected = Upgrades.THRUST_3;
        }
        if(upgradeId == 2){
            if(upgradeIdx == 1)
                upgradeSelected = Upgrades.CONTROL_1;
            if(upgradeIdx == 2)
                upgradeSelected = Upgrades.CONTROL_2;
        }
        if(upgradeId == 3){
            if(upgradeIdx == 1)
                upgradeSelected = Upgrades.RADIO_1;
            if(upgradeIdx == 2)
                upgradeSelected = Upgrades.RADIO_2;
        }

        if(Game.game.currPlayer.currency >= costs[upgradeSelected] && (Game.game.currPlayer.upgrades & upgradeSelected) == 0){
            Game.game.currPlayer.currency -= costs[upgradeSelected];
            Game.game.currPlayer.upgrades |= upgradeSelected;
            UpdateDisplay();
        }
    }

    void OnEnable(){
        playersDone.Clear();
        UpdateDisplay();
        LinkButtons();
    }

    void Start() => OnEnable();

    // public void Activate() => Start();


    public void DoneButtonPressed(){
        playersDone.Add(Game.game.currPlayer.id);
        Game.game.SetCurrPlayer((Game.game.currPlayer.id + 1)%4);
        UpdateDisplay();
        if(playersDone.Count >= 4){
            Game.stage = Game.Stage.LAUNCH;
        }
    }



    public void LinkButtons(){
        moreFuel1.onClick.AddListener(() => ButtonClicked(0, 1));
        moreFuel2.onClick.AddListener(() => ButtonClicked(0, 2));
        moreFuel3.onClick.AddListener(() => ButtonClicked(0, 3));

        moreThrust1.onClick.AddListener(() => ButtonClicked(1, 1));
        moreThrust2.onClick.AddListener(() => ButtonClicked(1, 2));
        moreThrust3.onClick.AddListener(() => ButtonClicked(1, 3));
        
        moreControl1.onClick.AddListener(() => ButtonClicked(2, 1));
        moreControl2.onClick.AddListener(() => ButtonClicked(2, 2));
        
        radio1.onClick.AddListener(() => ButtonClicked(3, 1));
        radio2.onClick.AddListener(() => ButtonClicked(3, 2));
    }


    public void UpdateDisplay(){
        Player player = Game.game.currPlayer;
        playerMoneyText.text = player.currency.ToString() + " credits";

        #region More Fuel

        if((player.upgrades & Upgrades.FUEL_1) == Upgrades.FUEL_1)
        {
            moreFuel1.interactable = false;
            moreFuel1.image.color = hasUpgradeColor;
            if((player.upgrades & Upgrades.FUEL_2) == Upgrades.FUEL_2)
            {
                moreFuel2.interactable = false;
                moreFuel2.image.color = hasUpgradeColor;
                if((player.upgrades & Upgrades.FUEL_3) == Upgrades.FUEL_3)
                {
                    moreFuel3.interactable = false;
                    moreFuel3.image.color = hasUpgradeColor;

                    
                    moreFuelPriceText.text = "All upgrades bought";
                }else{
                    moreFuel3.interactable = true;
                    if(player.currency >= costs[Upgrades.FUEL_3])
                        moreFuel3.image.color = upgradeElgibleColor;
                    else
                        moreFuel3.image.color = upgradeInelgibleColor;

                    
                    moreFuelPriceText.text = costs[Upgrades.FUEL_3].ToString() + " credits";
                }
            }else{
                moreFuel2.interactable = true;
                if(player.currency >= costs[Upgrades.FUEL_2])
                    moreFuel2.image.color = upgradeElgibleColor;
                else
                    moreFuel2.image.color = upgradeInelgibleColor;

                moreFuel3.interactable = false;
                moreFuel3.image.color= upgradeInelgibleColor;
                
                moreFuelPriceText.text = costs[Upgrades.FUEL_2].ToString() + " credits";
            }
        }else{
            moreFuel1.interactable = true;
            if(player.currency >= costs[Upgrades.FUEL_1])
                moreFuel1.image.color = upgradeElgibleColor;
            else
                moreFuel1.image.color = upgradeInelgibleColor;
            
            moreFuel2.interactable = false;
            moreFuel2.image.color = upgradeInelgibleColor;
            moreFuel3.interactable = false;
            moreFuel3.image.color = upgradeInelgibleColor;

            moreFuelPriceText.text = costs[Upgrades.FUEL_1].ToString() + " credits";
        }

        #endregion

        #region More thrust

        if((player.upgrades & Upgrades.THRUST_1) == Upgrades.THRUST_1)
        {
            moreThrust1.interactable = false;
            moreThrust1.image.color = hasUpgradeColor;
            if((player.upgrades & Upgrades.THRUST_2) == Upgrades.THRUST_2)
            {
                moreThrust2.interactable = false;
                moreThrust2.image.color = hasUpgradeColor;
                if((player.upgrades & Upgrades.THRUST_3) == Upgrades.THRUST_3)
                {
                    moreThrust3.interactable = false;
                    moreThrust3.image.color = hasUpgradeColor;

                    
                    moreThrustPriceText.text = "All upgrades bought";
                }else{
                    moreThrust3.interactable = true;
                    if(player.currency >= costs[Upgrades.THRUST_3])
                        moreThrust3.image.color = upgradeElgibleColor;
                    else
                        moreThrust3.image.color = upgradeInelgibleColor;

                    moreThrustPriceText.text = costs[Upgrades.THRUST_3].ToString() + " credits";
                }
            }else{
                moreThrust2.interactable = true;
                if(player.currency >= costs[Upgrades.THRUST_2])
                    moreThrust2.image.color = upgradeElgibleColor;
                else
                    moreThrust2.image.color = upgradeInelgibleColor;

                moreThrust3.interactable = false;
                moreThrust3.image.color= upgradeInelgibleColor;
                    
                moreThrustPriceText.text = costs[Upgrades.THRUST_2].ToString() + " credits";
            }
        }else{
            moreThrust1.interactable = true;
            if(player.currency >= costs[Upgrades.THRUST_1])
                moreThrust1.image.color = upgradeElgibleColor;
            else
                moreThrust1.image.color = upgradeInelgibleColor;
            
            moreThrust2.interactable = false;
            moreThrust2.image.color = upgradeInelgibleColor;
            moreThrust3.interactable = false;
            moreThrust3.image.color = upgradeInelgibleColor;

            
            moreThrustPriceText.text = costs[Upgrades.THRUST_1].ToString() + " credits";
        }

        #endregion

        #region More Control

        if((player.upgrades & Upgrades.CONTROL_1) == Upgrades.CONTROL_1)
        {
            moreControl1.interactable = false;
            moreControl1.image.color = hasUpgradeColor;
            if((player.upgrades & Upgrades.CONTROL_2) == Upgrades.CONTROL_2)
            {
                moreControl2.interactable = false;
                moreControl2.image.color = hasUpgradeColor;
                
                moreControlPriceText.text = "All upgrades bought";
            }else{
                moreControl2.interactable = true;
                if(player.currency >= costs[Upgrades.CONTROL_2])
                    moreControl2.image.color = upgradeElgibleColor;
                else
                    moreControl2.image.color = upgradeInelgibleColor;    
                moreControlPriceText.text = costs[Upgrades.CONTROL_2].ToString() + " credits";
            }
        }else{
            moreControl1.interactable = true;
            if(player.currency >= costs[Upgrades.CONTROL_1])
                moreControl1.image.color = upgradeElgibleColor;
            else
                moreControl1.image.color = upgradeInelgibleColor;
            
            moreControl2.interactable = false;
            moreControl2.image.color = upgradeInelgibleColor;
            
            moreControlPriceText.text = costs[Upgrades.CONTROL_1].ToString() + " credits";
        }

        #endregion

        #region Radio

        if((player.upgrades & Upgrades.RADIO_1) == Upgrades.RADIO_1)
        {
            radio1.interactable = false;
            radio1.image.color = hasUpgradeColor;
            if((player.upgrades & Upgrades.RADIO_2) == Upgrades.RADIO_2)
            {
                radio2.interactable = false;
                radio2.image.color = hasUpgradeColor;

                radioPriceText.text = "All upgrades bought";
            }else{
                radio2.interactable = true;
                if(player.currency >= costs[Upgrades.RADIO_2])
                    radio2.image.color = upgradeElgibleColor;
                else
                    radio2.image.color = upgradeInelgibleColor;    
                radioPriceText.text = costs[Upgrades.RADIO_2].ToString() + " credits";
            }
        }else{
            radio1.interactable = true;
            if(player.currency >= costs[Upgrades.RADIO_1])
                radio1.image.color = upgradeElgibleColor;
            else
                radio1.image.color = upgradeInelgibleColor;
            
            radio2.interactable = false;
            radio2.image.color = upgradeInelgibleColor;
            
            radioPriceText.text = costs[Upgrades.RADIO_1].ToString() + " credits";
        }

        #endregion
    }

    void Update()
    {
        
    }
}
