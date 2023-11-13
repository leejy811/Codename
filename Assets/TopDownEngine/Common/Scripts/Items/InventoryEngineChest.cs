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
	public class InventoryEngineChest : TopDownMonoBehaviour 
	{
		protected Animator _animator;
		protected ItemPicker[] _itemPickerList;

		protected bool isChestOpened = false;
		protected List<GameObject> _popupItemList;

		[Header("Popup Item Probabilities")]
		[Space()]
		[SerializeField] protected int bonusItemProb;
		[SerializeField] protected int weaponRankProb_C;
		[SerializeField] protected int weaponRankProb_B;
		[SerializeField] protected int weaponRankProb_A;
		[SerializeField] protected int weaponRankProb_S;

		[Header("Popup Items")]
		[Space()]
		[SerializeField] protected GameObject[] weaponS;
		[SerializeField] protected GameObject[] weaponA;
		[SerializeField] protected GameObject[] weaponB;
		[SerializeField] protected GameObject[] weaponC;
		


		/// <summary>
		/// On start we grab our animator and list of item pickers
		/// </summary>
		protected virtual void Start()
		{
			_animator = GetComponent<Animator> ();
			_itemPickerList = GetComponents<ItemPicker> ();
			_popupItemList = new List<GameObject>();

			if (weaponRankProb_A + weaponRankProb_B + weaponRankProb_C + weaponRankProb_S != 100)
				Debug.LogError("Input Error! The sum of probs must be 100!");
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
			int weaponRankProb = Random.Range(1, 101);

			// item1 (weapon)
            switch (weaponRankProb)
            {
				case int i when i <= weaponRankProb_C:
					_popupItemList.Add(weaponC[Random.Range(0, weaponC.Length)]);
					break;
				case int i when i <= weaponRankProb_C+ weaponRankProb_B:
					_popupItemList.Add(weaponB[Random.Range(0, weaponB.Length)]);
					break;
				case int i when i <= weaponRankProb_C + weaponRankProb_B+weaponRankProb_A:
					_popupItemList.Add(weaponA[Random.Range(0, weaponA.Length)]);
					break;
				case int i when i <= weaponRankProb_C + weaponRankProb_B + weaponRankProb_A+weaponRankProb_S:
					_popupItemList.Add(weaponS[Random.Range(0, weaponS.Length)]);
					break;
				default:
					Debug.LogError("popup prob does not match with 100");
					return;
			}

			// item2 (bullet)
			_popupItemList.Add(_popupItemList[0].GetComponent<BulletPickupInfo>().bulletPickupItem);
			
			// item3 (bonus)
			if(bonusItemFlag)
				_popupItemList.Add(_popupItemList[1]);

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
			for(int i = 0; i < _popupItemList.Count; i++)
            {
				var item = Instantiate(_popupItemList[i]);
				item.transform.SetParent(transform);
				item.transform.localPosition = Vector3.zero;

				item.GetComponent<BoxCollider2D>().enabled = false;
				float posX = _popupItemList.Count % 2 == 1 ? (i - (_popupItemList.Count / 2)) * 1.5f : (i - (_popupItemList.Count / 2)+0.5f) * 1.5f;

				var tween = item.transform.DOMove(new Vector3(posX,2,0),0.7f).SetRelative().SetEase(Ease.OutCubic);
				yield return tween.WaitForCompletion();
				item.GetComponent<BoxCollider2D>().enabled = true;
			}
        }
	}
}