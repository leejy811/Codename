using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public void InitEnemy(Room room)
    {
        Instantiate(GameManager.Instance.enemyA, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - GameManager.Instance.mapManger.mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - GameManager.Instance.mapManger.mapSize.y / 2, 0), Quaternion.identity, this.transform);
        Instantiate(GameManager.Instance.enemyB, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - GameManager.Instance.mapManger.mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - GameManager.Instance.mapManger.mapSize.y / 2, 0), Quaternion.identity, this.transform);
        Instantiate(GameManager.Instance.enemyC, new Vector3(Random.RandomRange(room.rect.x, room.rect.x + room.rect.width) - GameManager.Instance.mapManger.mapSize.x / 2, Random.RandomRange(room.rect.y, room.rect.y + room.rect.height) - GameManager.Instance.mapManger.mapSize.y / 2, 0), Quaternion.identity, this.transform);
    }
    
    // 함수 이름 다시 정하기
    public void Enemy1111(TurnType turnType)
    {
        for(int ind = 0; ind < this.transform.childCount; ind++)
        {
            GameObject enemy = this.transform.GetChild(ind).gameObject;
            enemy.GetComponent<TurnModeEnemy>().EnemyMove(turnType);
        }
        
    }
}
