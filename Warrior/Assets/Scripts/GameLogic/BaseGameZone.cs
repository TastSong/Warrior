using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseGameZone : MonoBehaviour, IEDable
{

    /// <summary>
    /// 当前关卡里面的怪物数量
    /// </summary>
    protected List<Agent> _enemies = new List<Agent>();

    public List<Agent> Enemies
    {
        get
        {
            return _enemies;
        }
    }

    public void AddEnemy(Agent enemy)
    {
        _enemies.Add(enemy);
    }

    /// <summary>
    /// 激活该关卡
    /// </summary>
    public virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 关卡隐藏
    /// </summary>
    public virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 关卡重置
    /// </summary>
    public virtual void Reset()
    {
        //停止携程任务
        StopAllCoroutines();

        //清理以创建的怪物
        for (int i = _enemies.Count-1; i >=0 ; i--)
        {
            //销毁怪物，放回缓存
        }
        _enemies.Clear();

        //清理死亡模型数据，放回缓存
    }

    /// <summary>
    /// 设置关卡进度
    /// </summary>
    public virtual void SetInProgress()
    {

    }

    /// <summary>
    /// 获取最近的交互对象
    /// </summary>
    /// <param name="center"></param>
    /// <param name="maxLen"></param>
    /// <returns></returns>
    public virtual InteractionGameObject GetNearestInteractionObject(Vector3 center, float maxLen)
    {
        return null;
    }

    /// <summary>
    /// 是否在交互对象返回内
    /// </summary>
    /// <param name="center"></param>
    /// <param name="maxLen"></param>
    /// <returns></returns>
    public virtual bool IsInteractionObjectInRange(Vector3 center, float maxLen)
    {
        return false;
    }

    /// <summary>
    /// 攻击可破碎物品
    /// </summary>
    /// <param name="attacker"></param>
    public virtual void BreakBreakableObjects(Agent attacker)
    {
    }

    // 攻击受击相关

        /// <summary>
        /// 指定范围内是否有怪
        /// </summary>
        /// <param name="center">指定的范围中心</param>
        /// <param name="maxLen">范围半径</param>
        /// <returns></returns>
    public bool IsEnemyInRange(Vector3 center, float maxLen)
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            Agent a = _enemies[i];
            //todo:死亡怪物就跳过

            if ((a.Position - center).sqrMagnitude < maxLen * maxLen)
                return true;

        }
        return false;
    }

    /// <summary>
    /// 获取指定方向最近的活着的怪物
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="direction"></param>
    /// <param name="maxRadius"></param>
    /// <returns></returns>
    public Agent GetNearestAliveEnemy(Agent agent, E_Direction direction, float maxRadius)
    {
        return null;
    }

    /// <summary>
    /// 获取指定范围内指定类型的怪物
    /// </summary>
    /// <param name="center">指定的中心</param>
    /// <param name="radius">范围半径</param>
    /// <param name="enemyType">怪物类型</param>
    /// <returns></returns>
    public Agent GetNearestAliveEnemy(Vector3 center, float radius, E_EnemyType enemyType)
    {
        return null;
    }


    /// <summary>
    /// 获取指定两个点之间的最近怪物
    /// </summary>
    /// <param name="start">开始位置</param>
    /// <param name="end">结束位置</param>
    /// <param name="radius">范围大小</param>
    /// <returns></returns>
    public Agent GetNearestAliveEnemy(Vector3 start, Vector3 end, float radius)
    {
        return null;
    }
}
