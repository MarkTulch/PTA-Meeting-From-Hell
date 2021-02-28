using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using System.Collections.Generic;
using System;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Add this class to an empty component in your scene, and it'll allow you to swap characters in your scene when pressing the SwapButton (P, by default)
    /// Each character in your scene will need to have a CharacterSwap class on it, and the corresponding PlayerID.
    /// You can see an example of such a setup in the MinimalCharacterSwap demo scene
    /// </summary>
    [AddComponentMenu("TopDown Engine/Managers/CharacterSwapManager")]
    public class CharacterSwapManager : MonoBehaviour
    {
        /// the name of the axis to use to catch input and trigger a swap on press
        [Tooltip("the name of the axis to use to catch input and trigger a swap on press")]
        public string SwapButtonName = "Player1_SwapCharacter";
        /// The name of the axis to use to catch input and zoom out/deselect units
        public string ZoomOutButtonName = "ZoomOut";
        /// the PlayerID set on the Characters you want to swap between
        [Tooltip("the PlayerID set on the Characters you want to swap between")]
        public string PlayerID = "Player1";

        protected CharacterSwap[] _characterSwapArray;
        protected int _currentSwapIndex;
        protected bool _isZoomedOut;
        protected List<CharacterSwap> _characterSwapList;
        protected TopDownEngineEvent _swapEvent = new TopDownEngineEvent(TopDownEngineEventTypes.CharacterSwap, null);

        /// <summary>
        /// On Start we update our list of characters to swap between
        /// </summary>
        protected virtual void Start()
        {
            UpdateList();
        }

        /// <summary>
        /// Grabs all CharacterSwap equipped characters in the scene and stores them in a list, sorted by Order
        /// </summary>
        public virtual void UpdateList()
        {
            _characterSwapArray = FindObjectsOfType<CharacterSwap>();
            _characterSwapList = new List<CharacterSwap>();

            // stores the array into the list if the PlayerID matches
            for (int i = 0; i < _characterSwapArray.Length; i++)
            {
                if (_characterSwapArray[i].PlayerID == PlayerID)
                {
                    _characterSwapList.Add(_characterSwapArray[i]);
                }
            }

            if (_characterSwapList.Count == 0)
            {
                return;
            }

            // sorts the list by order
            _characterSwapList.Sort(SortSwapsByOrder);
        }

        /// <summary>
        /// Static method to compare two CharacterSwaps
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static int SortSwapsByOrder(CharacterSwap a, CharacterSwap b)
        {
            return a.Order.CompareTo(b.Order);
        }

        /// <summary>
        /// On Update, we watch for input
        /// </summary>
        protected virtual void Update()
        {
            HandleInput();
        }

        /// <summary>
        /// If the user presses the Swap button, we swap characters
        /// </summary>
        protected virtual void HandleInput()
        {
            if (Input.GetButtonDown(SwapButtonName))
            {
                SwapCharacter();
            }
            if (Input.GetButtonDown(ZoomOutButtonName))
            {
                Debug.Log("Zoomin'!");
                if (_isZoomedOut)
                {
                    ZoomIn();
                }
                else
                {
                    ZoomOut();
                }
            }
        }

        /// <summary>
        /// Selects the last character and zooms in 
        /// </summary>
        public virtual void ZoomIn()
        {
            _characterSwapList[_currentSwapIndex].SwapToThisCharacter();

            _isZoomedOut = false;

            LevelManager.Instance.Players[0] = _characterSwapList[_currentSwapIndex].gameObject.MMGetComponentNoAlloc<Character>();
            MMEventManager.TriggerEvent(_swapEvent);
        }

        /// <summary>
        /// Deselects the current character and zooms out
        /// </summary>
        public virtual void ZoomOut()
        {
            if (_characterSwapList.Count == 0)
            {
                return;
            }

            for (int i = 0; i < _characterSwapList.Count; i++)
            {
                if (_characterSwapList[i].Current())
                {
                    _characterSwapList[i].SwapWeapons(true);
                    _currentSwapIndex = i;
                }
                _characterSwapList[i].ResetCharacterSwap();
            }

            _isZoomedOut = true;

            LevelManager.Instance.Players[0] = null;
            MMEventManager.TriggerEvent(_swapEvent);
        }

        /// <summary>
        /// Changes the current character to the next one in line
        /// </summary>
        public virtual void SwapCharacter()
        {
            if (_characterSwapList.Count == 0)
            {
                return;
            }

            int newIndex = -1;

            for (int i = 0; i < _characterSwapList.Count; i++)
            {
                if (_characterSwapList[i].Current())
                {
                    _characterSwapList[i].SwapWeapons(true);
                    newIndex = i + 1;
                }
                _characterSwapList[i].ResetCharacterSwap();
            }

            if (newIndex >= _characterSwapList.Count)
            {
                newIndex = 0;
            }
            _characterSwapList[newIndex].SwapToThisCharacter();

            LevelManager.Instance.Players[0] = _characterSwapList[newIndex].gameObject.MMGetComponentNoAlloc<Character>();
            MMEventManager.TriggerEvent(_swapEvent);
        }
    }
}