using MelonLoader;
using BTD_Mod_Helper;
using Multicash;
using Il2CppAssets.Scripts.Simulation;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using BTD_Mod_Helper.Api.ModOptions;
using UnityEngine;
using System;
using Il2CppTMPro;
using BTD_Mod_Helper.Api.Enums;

[assembly: MelonInfo(typeof(Multicash.Multicash), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace Multicash;

public class Multicash : BloonsTD6Mod
{

    private static readonly ModSettingHotkey OpenGUI = new(KeyCode.F10)
    {
        displayName = "Multicash Hotkey"
    };

    private static ModSettingDouble Multiplier = new(2.0)
    {
        displayName = "Global Cash Multiplier",
        slider = false
    };

    public static readonly ModSettingCategory MultiplierCategory = new("Individual Multipliers")
    {
        collapsed = true
    };

    public static readonly ModSettingCategory EnabledCategory = new("Toggle Multipliers")
    {
        collapsed = true
    };


    private static readonly ModSettingDouble PopMultiplier = new(1.0)
    {
        displayName = "Pop and Round-ending Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.PopIcon
    };

    private static readonly ModSettingDouble EcoMultiplier = new(1.0)
    {
        displayName = "Farm Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.BananaFarmIcon
    };

    private static readonly ModSettingDouble BankMultiplier = new(1.0)
    {
        displayName = "Bank Deposit Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.MonkeyBankUpgradeIcon
    };

    private static readonly ModSettingDouble CoopMultiplier = new(1.0)
    {
        displayName = "Coop Cash Transfer Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.Coop2PlayerIcon
    };

    private static readonly ModSettingDouble SellingMultiplier = new(1.0)
    {
        displayName = "Tower Selling Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.SellingDisabledIcon
    };

    private static readonly ModSettingDouble BuyingMultiplier = new(1.0)
    {
        displayName = "Tower Buying Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.BattleTowerPropIcon
    };

    private static readonly ModSettingDouble UpgradingMultiplier = new(1.0)
    {
        displayName = "Tower Upgrading Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.UpgradeBtn
    };


    private static readonly ModSettingDouble GeraldoMultiplier = new(1.0)
    {
        displayName = "Geraldo Purchase Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.GeraldoIcon
    };

    private static readonly ModSettingDouble MapMultiplier = new(1.0)
    {
        displayName = "Map Interactibles Multiplier",
        slider = false,
        category = MultiplierCategory,
        icon = VanillaSprites.GiftBoxIcon
    };

    private static readonly ModSettingBool MultiplyCashFromPopsAndRounds = new(true) {
        displayName = "Enable Pop and Round-End-Reward Multiplier",
        description = "Enable to Multiply the cash form all Pops and End of Round rewards",
        button= true,
        category = EnabledCategory,
        icon = VanillaSprites.PopIcon
    };
    private static readonly ModSettingBool MultiplyCashFromEcoEarned = new(true)
    {
        displayName = "Enable Farm Multiplier",
        description = "Enable to Multiply from farms and Eco earned",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.BananaFarmIcon
    };
    private static readonly ModSettingBool MultiplyCashFromBankDeposits = new(true)
    {
        displayName = "Enable Bank Deposit Multiplier",
        description = "Enable to Multiply the cash form Bank deposits (Anything you take out of a bank, so be aware that it can multiply inserted Cash)",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.MonkeyBankUpgradeIcon
    };
    private static readonly ModSettingBool MultiplyCashFromCoopTransfer = new(true)
    {
        displayName = "Enable Coop-Transfer Multiplier",
        description = "Enable to Multiply the cash from Coop-Transfers",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.Coop2PlayerIcon
    };
    private static readonly ModSettingBool MultiplyCashFromSellingTowers = new(true)
    {
        displayName = "Enable Selling Towers Multiplier",
        description = "Enable to Multiply the cash form selling Towers",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.SellingDisabledIcon
    };
    private static readonly ModSettingBool MultiplyCashFromBuyingTowers = new(true)
    {
        displayName = "Enable Buying Towers Multiplier",
        description = "Enable to Multiply the cash form buying Towers (Does not increase the cost of the Towers, it is a cash-source in the code, which can be used by other mods)",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.BattleTowerPropIcon
    };
    private static readonly ModSettingBool MultiplyCashFromUpgradingTowers = new(true)
    {
        displayName = "Enable Upgrading Towers Multiplier",
        description = "Enable to Multiply the cash form Upgrading Towers (Does not increase the cost of the Upgrades, it is a cash-source in the code, which can be used by other mods)",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.UpgradeBtn
    };
    private static readonly ModSettingBool MultiplyCashFromGeraldoPurchases = new(true)
    {
        displayName = "Enable Geraldo Purchase Multiplier",
        description = "Enable to Multiply the cash form Geraldo Purchases (Does nothing in the vanilla game, it is a cash-source in the code, which can be used by other mods)",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.GeraldoIcon
    };
    private static readonly ModSettingBool MultiplyCashFromMapInteractibles = new(true)
    {
        displayName = "Enable Mapinteractibles Multiplier",
        description = "Enable to Multiply the cash form all Mapinteractibles",
        button = true,
        category = EnabledCategory,
        icon = VanillaSprites.GiftBoxIcon
    };


    public override void OnApplicationStart()
    {
        ModHelper.Msg<Multicash>("Multicash loaded!");
    }

    static bool displayOpen = false;
    public override void OnUpdate()
    {
        base.OnUpdate();
        if (displayOpen)
        {
            if (PopupScreen.instance.GetFirstActivePopup() != null)
            {
                PopupScreen.instance.GetFirstActivePopup().GetComponentInChildren<TMP_InputField>().characterValidation = TMP_InputField.CharacterValidation.None;
                displayOpen = false;
            }
        }

        if (OpenGUI.JustPressed())
        {
            Action<string> mod = delegate (string s)
            {
                Multiplier = float.Parse(s.Replace(".",","));

            };
            PopupScreen.instance.ShowSetNamePopup("Cash", "Multiply Cash by", mod, Multiplier.GetValue().ToString());

            displayOpen = true;
        }
    }


    [HarmonyLib.HarmonyPatch(typeof(Simulation), "AddCash")]
    public class MultiplyCash
    {
        [HarmonyLib.HarmonyPrefix]
        public static bool Prefix(ref double c, ref Simulation.CashSource source)
        {
            ModHelper.Msg<Multicash>(source);
            if (source == Simulation.CashSource.Normal && MultiplyCashFromPopsAndRounds)
            {
                c = (c * Multiplier) * PopMultiplier;
            } 
            else if (source == Simulation.CashSource.EcoEarned && MultiplyCashFromEcoEarned)
            {
                c = (c * Multiplier) * EcoMultiplier;
            }
            else if (source == Simulation.CashSource.CoopTransferedCash && MultiplyCashFromCoopTransfer)
            {
                c = (c * Multiplier) * CoopMultiplier;
            }
            else if (source == Simulation.CashSource.TowerSold && MultiplyCashFromSellingTowers)
            {
                c = (c * Multiplier) * SellingMultiplier;
            }
            else if (source == Simulation.CashSource.TowerBrought && MultiplyCashFromBuyingTowers)
            {
                c = (c * Multiplier) * BuyingMultiplier;
            }
            else if (source == Simulation.CashSource.TowerUpgraded && MultiplyCashFromUpgradingTowers)
            {
                c = (c * Multiplier) * UpgradingMultiplier;
            }
            else if (source == Simulation.CashSource.BankDeposit && MultiplyCashFromBankDeposits)
            {
                c = (c * Multiplier) * BankMultiplier;
            }
            else if (source == Simulation.CashSource.GeraldoPurchase && MultiplyCashFromGeraldoPurchases)
            {
                c = (c * Multiplier) * GeraldoMultiplier;
            }
            else if (source == Simulation.CashSource.MapInteractableUsed && MultiplyCashFromMapInteractibles)
            {
                c = (c * Multiplier) * MapMultiplier;
            }
            return true;
        }
    }
}