using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class WGH_CatController : MonoBehaviour
{
    private Animator _anim;

    private Touch _touch;
    private Coroutine _feverAttackRoutine;

    private bool _isFever;                          // 피버 상태인지
    private bool _isPushDown;                       // 누르고 있는지
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        WGH_StatManager.Instance.OnMaxFeverGaze += ChangeIsFever;
        WGH_StatManager.Instance.OnEndFeverGaze += ChangeIsFever;
    }
    private void Update()
    {
        // UI가 아닌 화면 터치
        // 피버게이지가 가득차지 않았을 때
        if (_isFever == false && Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false
            && WGH_StatManager.Instance.GetFeverGaze() > WGH_StatManager.Instance.GetCurFeverGaze())
        {
            _touch = Input.GetTouch(0);
            if (_touch.phase == TouchPhase.Began)
            {
                Attack();
            }
        }
        // 피버게이지가 가득찼을 때
        else if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false
            && WGH_StatManager.Instance.GetFeverGaze() <= WGH_StatManager.Instance.GetCurFeverGaze())
        {

        }

        // UI가 아닌 마우스 클릭
        // 피버게이지가 가득차지 않았을 때
        if (_isFever == false && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false
        && WGH_StatManager.Instance.GetFeverGaze() > WGH_StatManager.Instance.GetCurFeverGaze())
            Attack();
        // 피버게이지가 가득찼을 때
        else if (Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false
        && _isFever)
            FeverAttack();
        // 마우스 클릭을 떼었을 때
        else if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false)
            _isPushDown = false;
    }
    /// <summary>
    /// 공격 메서드(애니메이션, 이펙트, 사운드 등을 호출하고 피버게이지 상승)
    /// </summary>
    public void Attack()
    {
        WGH_MonsterManager.Instance.ReceiveHit(E_AttackType.Attack);
        WGH_StatManager.Instance.IncreaseCurFeverGaze();
        _anim.SetTrigger("isAttack");
    }

    public void FeverAttack()
    {
        if(_feverAttackRoutine == null)
        {
            _isPushDown = true;
            _feverAttackRoutine = StartCoroutine(FeverAttackRoutine());
        }
    }

    IEnumerator FeverAttackRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while (true)
        {
            if (!_isPushDown || !_isFever)
            {
                _feverAttackRoutine = null;
                yield break;
            }
            WGH_MonsterManager.Instance.ReceiveHit(E_AttackType.Attack);
            _anim.SetTrigger("isAttack");
            yield return waitTime;
            yield return null;
        }
    }
    private void ChangeIsFever() { _isFever = !_isFever; }
}
