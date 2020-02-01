using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour
{
    public GameObject Target;
    public Animation Animation;
    private Animation ParentAnimation;


    private CameraOffset Offset;

    private Transform CameraTransform;
    private Transform TargetTransform;

    public static CameraBehaviour Instance;
    float DisabledTime = 0;

    float CurrentFovTime;
    float FovTime;
    float FovStart;
    float FovCameraEnd;
    float CurrentCameraFov = 0;
    bool FovCameraOk;

  /*  float CurrentBlurTime;
    float BlurTime;
    float BlurStart;
    float BlurEnd;
    float CurrentBlur;
    bool BlurOk = true;*/

    float CurrentShiftTime;
    float ShiftTime;
    float TimeScaleStart;
    float TimeScaleEnd;
    float TimeScaleCurrent;
    bool ShiftOk = false;
    
   // Vignetting CriticalHitEffect; 

    public float BaseFov;

    void Awake()
    {
        Instance = this;
        Animation = Camera.main.GetComponent<Animation>();
        ParentAnimation = GetComponent<Animation>();
    }

    void Start()
    {
        DisabledTime = 0;

   /*     CriticalHitEffect =  GetComponentInChildren<Vignetting>();
        CriticalHitEffect.blurVignette = 0;
        CriticalHitEffect.enabled = false;*/
  //      BlurOk = true;


        Offset = Target.GetComponent("CameraOffset") as CameraOffset;

        CameraTransform = transform;

        TargetTransform = Target.transform;

      //  CameraTransform.position = Offset.GetCameraPosition();

     /*   Vector3 dir = CameraTransform.forward;
        dir.y = 0;
        dir.Normalize();
        Vector3 t = TargetTransform.position;
        t += dir * 1.5f;

        CameraTransform.LookAt(t);*/


        CurrentFovTime = 0;
        FovTime = 0;
        FovStart = 0;
        FovCameraEnd = 0;
        BaseFov = CurrentCameraFov = Camera.main.fov;
        FovCameraOk = true;
        TimeScaleCurrent = 1;
        ShiftOk = true;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Game.Instance.GameState == E_GameState.Game)
        {
            UpdateFov();
            UpdateSloMotion();
    //        UpdatePPE();
        }

        if (DisabledTime >= Time.timeSinceLevelLoad)
            return;

        // Where should our camera be looking right now?
        Vector3 goalPosition = Offset.CameraPosition;

        CameraTransform.position = Vector3.Lerp(CameraTransform.position, goalPosition, Time.deltaTime * 4);

        Vector3 dir = CameraTransform.forward;
        dir.y = 0;
        dir.Normalize();
        Vector3 t = TargetTransform.position;
        t += dir * 0.7f;

        Vector3 lookAt = t - CameraTransform.position;
        lookAt.Normalize();

        CameraTransform.forward = Vector3.Lerp(CameraTransform.forward, lookAt, Time.deltaTime * 4);
    }

    public void PlayCameraAnim(string animName, bool overrideBehaviour, bool fade)
    {
        if (ParentAnimation[animName] == null)
            return;

        if (overrideBehaviour)
        {
            StartCoroutine(FadeInOutAndCameraPlay(animName));
        }
        else
        {
            ParentAnimation[animName].blendMode = AnimationBlendMode.Blend;
            ParentAnimation.CrossFade(animName, 0.5f);

            if (overrideBehaviour)
                DisabledTime = Time.timeSinceLevelLoad + ParentAnimation[animName].length;
        }
    }

    public void ComboShake(int comboLevel)
    {
        string[] animations = { "shakeCombo1", "shakeCombo2", "shakeCombo3" };

        if (Animation[animations[comboLevel]] == null)
            return;

        Animation[animations[comboLevel]].blendMode = AnimationBlendMode.Blend;
        Animation.Play(animations[comboLevel]);
    }

    public void BigInjuryShake()
    {
        if (Animation["shakeInjury"] == null)
            return;

        Animation["shakeInjury"].blendMode = AnimationBlendMode.Blend;
        Animation.Play("shakeInjury");
    }


    public void Reset()
    {
        CameraTransform.position = Offset.CameraPosition;

        Vector3 dir = CameraTransform.forward;
        dir.y = 0;
        dir.Normalize();
        Vector3 t = TargetTransform.position;
        t += dir * 0.7f;

        Vector3 lookAt = t - CameraTransform.position;
        lookAt.Normalize();
        CameraTransform.forward = lookAt;
    }

    public void Activate(Vector3 pos, Vector3 lookAt)
    {
//        Debug.Log(pos);
        DisabledTime = 0;

        CameraTransform.position = pos;
        CameraTransform.LookAt(lookAt);
    }

    public void InterpolateScaleFovBack()
    {
        CancelInvoke("InterpolateScaleFovBack");
        InterpolateTimeScale(1, 0.4f);
        InterpolateFov(BaseFov, 0.4f);

        Player.Instance.Agent.BlackBoard.Invulnerable = false;
   /*     if (CriticalHitEffect)
            CriticalHitEffect.enabled = false;*/
    }

    public void InterpolateFov(float newFov, float inTime)
    {
        CancelInvoke("InterpolateScaleFovBack");
        CurrentFovTime = 0;
        FovTime = inTime;
        FovStart= CurrentCameraFov;
        FovCameraEnd = newFov;

        FovCameraOk = false;
    }

    void UpdateFov()
    {
        if (FovCameraOk == false)
        {
            CurrentFovTime += Time.deltaTime                        ;

            if (CurrentFovTime > FovTime)
            {
                CurrentFovTime = FovTime;
                FovCameraOk = true;
            }

            if (CurrentFovTime >= 0)
                CurrentCameraFov = Mathfx.Hermite(FovStart, FovCameraEnd, CurrentFovTime / FovTime);
        }
        Camera.main.fov = CurrentCameraFov;
    }

    public void InterpolateTimeScale(float newTimeScale, float inTime)
    {
        CancelInvoke("InterpolateScaleFovBack");
  //      Debug.Log(Time.timeSinceLevelLoad + "scale" + newTimeScale + " " + inTime);
        CurrentShiftTime = 0;
        ShiftTime = inTime;
        TimeScaleStart = Time.timeScale;
        TimeScaleEnd = newTimeScale;

        ShiftOk = false;

        //if (CriticalHitEffect)
            //CriticalHitEffect.enabled = true;

        Player.Instance.Agent.BlackBoard.Invulnerable = true;
    }

    public void InterpolateBlur(float newBlur, float inTime)
    {
 /*       CurrentBlurTime = 0;
        BlurTime = inTime;
        BlurStart = CriticalHitEffect.blurVignette;
        BlurEnd = newBlur;

        BlurOk = false;*/
    }

   /* void UpdateCriticalPPE()
    {
        if (BlurOk == false)
        {
            CurrentBlurTime += Time.deltaTime;

            if (CurrentBlurTime > BlurTime)
            {
                CurrentBlurTime = BlurTime;
                BlurOk = true;
            }

            if (CurrentBlurTime >= 0)
                CurrentBlur = Mathfx.Lerp(BlurStart, BlurEnd, CurrentBlurTime / BlurTime);

            CriticalHitEffect.blurVignette = CurrentBlur;
            CriticalHitEffect.enabled = CriticalHitEffect.blurVignette != 0;
        }
    }*/

    void UpdateSloMotion()
    {
        if (ShiftOk == false)
        {
            CurrentShiftTime += Time.deltaTime;

            if (CurrentShiftTime > ShiftTime)
            {
                CurrentShiftTime = ShiftTime;
                ShiftOk = true;
            }

            if (CurrentShiftTime >= 0)
                TimeScaleCurrent = Mathfx.Hermite(TimeScaleStart, TimeScaleEnd, CurrentShiftTime / ShiftTime);
        }
        Time.timeScale = TimeScaleCurrent;
    }

    IEnumerator FadeInOutAndCameraPlay(string animName)
    {
        //GuiManager.Instance.FadeOut(0.1f, 1);
        yield return new WaitForSeconds(0.2f);

        ParentAnimation[animName].blendMode = AnimationBlendMode.Blend;
        ParentAnimation.CrossFade(animName, 0.5f);

        DisabledTime = Time.timeSinceLevelLoad + ParentAnimation[animName].length;

        StartCoroutine(FadeInOutAndCameraPlayEnd(ParentAnimation[animName].length - 0.5f));

        yield return new WaitForSeconds(0.1f);

        //GuiManager.Instance.FadeIn();
    }

    IEnumerator FadeInOutAndCameraPlayEnd(float delay)
    {
        yield return new WaitForSeconds(delay);

        //GuiManager.Instance.FadeOut(0.3f, 1);
        yield return new WaitForSeconds(0.5f);
        //GuiManager.Instance.FadeIn();
    }
}

