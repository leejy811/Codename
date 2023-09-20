using UnityEngine;
using System.Collections;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using System.Collections.Generic;
using DG.Tweening;

namespace MoreMountains.TopDownEngine
{
	/// <summary>
	/// Add this component to an object in your scene to have it act like a chest. You'll need a key operated zone to open it, and item picker(s) on it to fill its contents
	/// </summary>
	[AddComponentMenu("TopDown Engine/Items/InventoryEngineChest")]
	public class InventoryEngineChest : TopDownMonoBehaviour 
	{
		protected Animator _animator;
		protected ItemPicker[] _itemPickerList;

		protected bool isChestOpened = false;
		[SerializeField] protected GameObject[] _popupItemList;

		/// <summary>
		/// On start we grab our animator and list of item pickers
		/// </summary>
		protected virtual void Start()
		{
			_animator = GetComponent<Animator> ();
			_itemPickerList = GetComponents<ItemPicker> ();
		}

		/// <summary>
		/// A public method to open the chest, usually called by the associated key operated zone
		/// </summary>
		public virtual void OpenChest()
		{
			if (!isChestOpened)
			{
				TriggerOpeningAnimation();
				PopupChestItems();
				//PickChestContents();

				isChestOpened = true;
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
			_animator.SetTrigger ("OpenChest");
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
				picker.Pick ();
			}
		}

		protected void PopupChestItems()
        {
			if (_popupItemList.Length == 0)
				return;
			StartCoroutine(PopupTweening());
        }

		IEnumerator PopupTweening()
        {
			for(int i = 0; i < _popupItemList.Length; i++)
            {
				var item = Instantiate(_popupItemList[i]);
				item.transform.SetParent(transform);
				item.transform.localPosition = Vector3.zero;

				float posX = (i -(_popupItemList.Length /2))* 1.5f;
				var tween = item.transform.DOMove(new Vector3(posX,2,0),1).SetRelative();
				yield return tween.WaitForCompletion();
            }
        }
	}
}