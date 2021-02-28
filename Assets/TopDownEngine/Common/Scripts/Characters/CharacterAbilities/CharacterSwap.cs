using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using UnityEngine.UI;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this ability to a Character and it'll be part of a pool of characters in a scene to swap from. 
    /// You'll need a CharacterSwapManager in your scene for this to work.
    /// </summary>
    [MMHiddenProperties("AbilityStopFeedbacks")]
    [AddComponentMenu("TopDown Engine/Character/Abilities/Character Swap")]
    public class CharacterSwap : CharacterAbility
    {
        [Header("Character Swap")]
        /// the order in which this character should be picked 
        [Tooltip("the order in which this character should be picked ")]
        public int Order = 0;
        /// the playerID to put back in the Character class once this character gets swapped
        [Tooltip("the playerID to put back in the Character class once this character gets swapped")]
        public string PlayerID = "Player1";

        [Header("AI")]
        /// if this is true, the AI Brain (if there's one on this character) will reset on swap
        [Tooltip("if this is true, the AI Brain (if there's one on this character) will reset on swap")]
        public bool ResetAIBrainOnSwap = true;
        [Header("Weapon Switching")]
        /// if this is true, the character should switch weapons upon swap
        [Tooltip("if this is true, the character should switch weapons upon swap")]
        public bool SwitchWeaponsOnSwap = false;
        /// first weapon to switch to 
        [Tooltip("AI weapon to switch to ")]
        public Weapon AIWeapon;
        /// second weapon to switch to 
        [Tooltip("Player weapon to switch to ")]
        public Weapon PlayerWeapon;

        [Header("HUD Image")]
        /// reference to the profile HUD image
        [Tooltip("reference to the profile HUD image")]
        public Image HUDImage;
        /// character's profile sprite
        public Sprite CharacterSprite;

        protected string _savedPlayerID;
        protected Character.CharacterTypes _savedCharacterType;
        protected AIBrain _aiBrain;

        /// <summary>
        /// On init, we grab our character type and playerID and store them for later
        /// </summary>
        protected override void Initialization()
        {
            base.Initialization();
            _savedCharacterType = _character.CharacterType;
            _savedPlayerID = _character.PlayerID;
            _aiBrain = this.gameObject.GetComponent<AIBrain>();
            if (HUDImage == null )
            {
                HUDImage = GameObject.Find("AvatarFront").GetComponent<Image>();
            }
        }

        /// <summary>
        /// Called by the CharacterSwapManager, changes this character's type and sets its input manager
        /// </summary>
        public virtual void SwapToThisCharacter()
        {
            PlayAbilityStartFeedbacks();
            _character.PlayerID = PlayerID;
            _character.CharacterType = Character.CharacterTypes.Player;
            _character.SetInputManager();
            if (_aiBrain != null)
            {
                _aiBrain.BrainActive = false;
            }
            if (_character.GetComponent<CharacterHandleWeapon>() != null)
            {
                SwapWeapons(false);
            }
            HUDImage.sprite = CharacterSprite;
        }

        /// <summary>
        /// Called when you want the current character to swap 
        /// </summary>
        public virtual void SwapWeapons(bool shouldEquipAIWeapon)
        {
            if (!SwitchWeaponsOnSwap)
            {
                return;
            }
            if (!shouldEquipAIWeapon)
            {
                _character.GetComponent<CharacterHandleWeapon>().ChangeWeapon(PlayerWeapon, PlayerWeapon.WeaponID);
            } else
            {
                _character.GetComponent<CharacterHandleWeapon>().ChangeWeapon(AIWeapon, AIWeapon.WeaponID);
            }
        }

        /// <summary>
        /// Called when another character replaces this one as the active one, resets its type and player ID and kills its input
        /// </summary>
        public virtual void ResetCharacterSwap()
        {
            _character.CharacterType = Character.CharacterTypes.AI;
            _character.PlayerID = _savedPlayerID;
            _character.SetInputManager(null);
            _characterMovement.SetHorizontalMovement(0f);
            _characterMovement.SetVerticalMovement(0f);
            _character.ResetInput();
            if (_aiBrain != null)
            {
                _aiBrain.BrainActive = true;
                if (ResetAIBrainOnSwap)
                {
                    _aiBrain.ResetBrain();
                }
            }

        }

        /// <summary>
        /// Returns true if this character is the currently active swap character
        /// </summary>
        /// <returns></returns>
        public virtual bool Current()
        {
            return (_character.CharacterType == Character.CharacterTypes.Player);
        }
    }
}