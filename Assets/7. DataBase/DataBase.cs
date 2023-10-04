using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class DataBase : ScriptableObject
{
	public List<WeaponDBEntity> WeaponDB;
	public List<EnemyDBEntity> EnemyDB;
}
