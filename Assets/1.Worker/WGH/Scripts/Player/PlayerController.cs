using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    private Animator _anim;

    private Touch _touch;
    private Coroutine _feverAttackRoutine;
    private Coroutine _attackRoutine;

    private bool _isFever;                                      // 피버 상태인지
    private bool _isPushDown;                                   // 누르고 있는지

    private bool _isLeftAttack;                                 // 기본 공격 좌우 확인용
    private bool _isRapidLeft;                                  // 피버 공격 좌우 확인용
    
    private float _lastAttackTime = -Mathf.Infinity;            // 초기 값은 아주 오래 전
    [SerializeField] private float _switchDelay;                // 공격 전환 시간
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        PlayerDataManager.Instance.OnMaxFeverGaze += ChangeIsFever;
        PlayerDataManager.Instance.OnEndFeverGaze += ChangeIsFever;
    }
    private void Update()
    {
        // 모바일
        // UI가 아닌 화면 터치
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            // UI 위가 아니라면
            if (!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    _isPushDown = true;
                    if (_isFever)
                        FeverAttack();
                    else if (!_isFever && PlayerDataManager.Instance.GetFeverGaze() > PlayerDataManager.Instance.GetCurFeverGaze())
                        Attack(); 
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    _isPushDown = false;

                    if (_feverAttackRoutine != null)
                    {
                        StopCoroutine(_feverAttackRoutine);
                        _feverAttackRoutine = null;
                    }
                }
            }
        }
        // 컴퓨터
        // UI가 아닌 마우스 클릭
        // 피버게이지가 가득차지 않았을 때
        if (_isFever == false && Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false
        && PlayerDataManager.Instance.GetFeverGaze() > PlayerDataManager.Instance.GetCurFeverGaze())
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
        float curTime = Time.time;
        if(curTime - _lastAttackTime > _switchDelay) { _isLeftAttack = !_isLeftAttack; }
        if(_isLeftAttack) { _anim.SetBool("isLeft", true); }

        bool isCritical = Random.value <= PlayerDataManager.Instance.GetCriticalChance();

        if (isCritical == false)
            MonsterManager.Instance.ReceiveHit(E_AttackType.Attack);
        else
            MonsterManager.Instance.ReceiveHit(E_AttackType.Critical);
        DamageTextManager.Instance.ShowDamage(PlayerDataManager.Instance.GetPlayerDmg(), Vector3.zero);

        PlayerDataManager.Instance.IncreaseCurFeverGaze();
        int randomValue = Random.Range(0, 2);
        if (_isLeftAttack)
        {
            switch (randomValue)
            {
                case 0:
                    _anim.SetTrigger("isLeftAttack1");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch1, 0.5f);
                    break;
                case 1:
                    _anim.SetTrigger("isLeftAttack2");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch2, 0.5f);
                    break;
            }
        }
        else
        {
            switch (randomValue)
            {
                case 0:
                    _anim.SetTrigger("isRightAttack1");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch1, 0.5f);
                    break;
                case 1:
                    _anim.SetTrigger("isRightAttack2");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch2, 0.5f);
                    break;
            }
        }
        
        _lastAttackTime = curTime;
    }

    public void FeverAttack()
    {
        if (_feverAttackRoutine == null)
        {
            _isPushDown = true;
            _feverAttackRoutine = StartCoroutine(FeverAttackRoutine());
        }
    }

    IEnumerator FeverAttackRoutine()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.1f);
        while (_isPushDown && _isFever)
        {
            MonsterManager.Instance.ReceiveHit(E_AttackType.Attack);
            RapidAttack();
            yield return waitTime;
        }

        // 루프 탈출 시 코루틴 참조 해제
        _feverAttackRoutine = null;
    }
    private void ChangeIsFever() { _isFever = !_isFever; }

    private void RapidAttack()
    {
        _isRapidLeft = !_isRapidLeft;
        int randomNum = Random.Range(0, 2);
        if (_isRapidLeft)
        {
            switch (randomNum)
            {
                case 0:
                    _anim.SetTrigger("isLeftAttack1");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch1, 0.5f);
                    break;
                case 1:
                    _anim.SetTrigger("isLeftAttack2");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch2, 0.5f);
                    break;
            }
        }
        else
        {
            switch (randomNum)
            {
                case 0:
                    _anim.SetTrigger("isRightAttack1");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch1, 0.5f);
                    break;
                case 1:
                    _anim.SetTrigger("isRightAttack2");
                    SoundManager.Instance.PlaySFX(E_SFX.Punch2, 0.5f);
                    break;
            }
        }
    }

    private void OnDisable()
    {
        PlayerDataManager.Instance.OnMaxFeverGaze -= ChangeIsFever;
        PlayerDataManager.Instance.OnEndFeverGaze -= ChangeIsFever;
    }
}
