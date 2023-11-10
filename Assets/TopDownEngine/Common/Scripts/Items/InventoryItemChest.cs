using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;
using DG.Tweening;
using CustomWeapon;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// Add this component to an object in your scene to have it act like a chest. You'll need a key operated zone to open it, and item picker(s) on it to fill its contents
	/// </summary>
	[AddComponentMenu("TopDown Engine/Items/InventoryEngineChest")]
	public class InventoryItemChest : TopDownMonoBehaviour
	{
		protected Animator _animator;
		protected ItemPicker[] _itemPickerList;

		protected bool isChestOpened = false;
		protected List<GameObject> _popupItemList;

		[Header("Popup Item Probabilities")]
		[Space()]
		[SerializeField] protected int bonusItemProb;
		

		[Header("Popup Items")]
		[Space()]
		[SerializeField] protected GameObject[] buffItems;
		[SerializeField] protected GameObject[] bonusItems;


		/// <summary>
		/// On start we grab our animator and list of item pickers
		/// </summary>
		protected virtual void Start()
		{
			_animator = GetComponent<Animator>();
			_itemPickerList = GetComponents<ItemPicker>();
			_popupItemList = new List<GameObject>();
		}

		/// <summary>
		/// A public method to open the chest, usually called by the associated key operated zone
		/// </summary>
		public virtual void OpenChest()
		{
			if (!isChestOpened)
			{
				DiceRoll();
				TriggerOpeningAnimation();
				PopupChestItems();
				//PickChestContents();

				isChestOpened = true;
			}
		}


		private void DiceRoll()
		{
			bool bonusItemFlag = Random.Range(1, 101) <= bonusItemProb ? true : false;
			int popupItemIdx = Random.Range(1, 101)%buffItems.Length;

			_popupItemList.Add(buffItems[popupItemIdx]);

			// item2 (bonus)
			if (bonusItemFlag)
            {
				int bonusItemIdx = Random.Range(0, 99) % bonusItems.Length;
				_popupItemList.Add(bonusItems[bonusItemIdx]);
			}

		}

		/// <summary>
		/// Triggers the opening animation.
		/// </summary>
		protected virtual void TriggerOpeningAnimation()
		{
			if (_animator == null)
			{
				return;
			}
			_animator.SetTrigger("OpenChest");
		}

		/// <summary>
		/// Puts all the items in the associated pickers into the player's inventories
		/// </summary>
		protected virtual void PickChestContents()
		{
			if (_itemPickerList.Length == 0)
			{
				return;
			}
			foreach (ItemPicker picker in _itemPickerList)
			{
				picker.Pick();
			}
		}

		/// <summary>
		/// Popup items from the chest
		/// </summary>
		protected void PopupChestItems()
		{
			if (_popupItemList.Count == 0)
				return;
			StartCoroutine(PopupTweening());
		}

		IEnumerator PopupTweening()
		{
			for (int i = 0; i < _popupItemList.Count; i++)
			{
				var item = Instantiate(_popupItemList[i]);
                item.transform.SetParent(transform);
				item.transform.localPosition = Vector3.zero;

				float posX = _popupItemList.Count % 2 == 1 ? (i - (_popupItemList.Count / 2)) * 1.5f : (i - (_popupItemList.Count / 2) + 0.5f) * 1.5f;

				var tween = item.transform.DOMove(new Vector3(posX, 2, 0), 0.7f).SetRelative().SetEase(Ease.OutCubic);
				yield return tween.WaitForCompletion();
			}
		}
	}
}