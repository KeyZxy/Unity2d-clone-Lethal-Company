using System.Collections.Generic;
using UnityEngine;
public class SectorArea : MonoBehaviour
{
    [SerializeField]
    private Transform _centerTrans;
    public Vector2 CenterPos { get { return _centerTrans.position; } }
    [SerializeField]
    private Transform _dirTrans;
    public Vector2 Dir { get { return _dirTrans.right; } }
    [SerializeField, Range(1, 359)]
    private float _angles;
    [SerializeField, Range(0.1f, 15)]
    private float _radius;

    [SerializeField]
    private SectorAreaView _view;
    [SerializeField]
    private Color _viewColor = Color.red;

    public void Init()
    {
        RefreshView();
    }
    public void MyDestroy()
    {

    }

    public bool IsInArea(Vector2 pos)
    {
        var ret = false;
        if (Vector2.Distance(CenterPos, pos) < _radius)
        {
            var halfAngle = _angles / 2;
            var dir = (pos - CenterPos).normalized;
            var curAngle = Vector2.Angle(Dir, dir);
            if (curAngle < halfAngle || curAngle > 360 - halfAngle)
            {
                ret = true;
            }
        }
        return ret;
    }

    // 在范围内是否能找到指定层的对象
    public bool CanFindTarget(LayerMask targetLayer, out Transform target)
    {
        var ret = CanFindTargets(targetLayer, out Transform[] targets);
        target = ret ? targets[0] : null;
        return ret;
    }
    public bool CanFindTargets(LayerMask targetLayer, out Transform[] targets)
    {
        var ret = false;
        var targetList = new List<Transform>();
        var cols = Physics2D.OverlapCircleAll(CenterPos, _radius, targetLayer);
        if (cols != null)
        {
            foreach (var col in cols)
            {
                Vector2 pos = col.transform.position;
                if (IsInArea(pos))
                {
                    targetList.Add(col.transform);
                }
            }
        }

        ret = targetList.Count > 0;
        targets = targetList.ToArray();

        return ret;
    }


    [ContextMenu("刷新扇形面积的展示")]
    private void RefreshView()
    {
        _view.CreateSprite(_radius, _angles, _viewColor);
    }
}