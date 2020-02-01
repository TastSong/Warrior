using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class ComponentChoppedBody : MonoBehaviour
{
    public Material TransparentMaterial;
    public Material DiffuseMaterial;


    private Transform Transform;
    private Animation Anims;
    private AudioSource Audio;
    private SkinnedMeshRenderer Renderer;
    private GameObject GameObject;


    void Awake()
    {
        Transform = transform;
        Anims = GetComponent<Animation>();
        Audio = GetComponent<AudioSource>();
        GameObject = gameObject;
        Anims.wrapMode = WrapMode.ClampForever;

        Renderer = GameObject.GetComponentInChildren(typeof(SkinnedMeshRenderer)) as SkinnedMeshRenderer;

        Anims[Anims.clip.name].wrapMode = WrapMode.ClampForever;
    }

    void Start()
    {
    }
		
    public void Enable(Transform spawnTransform)
    {
        Transform.position = spawnTransform.position;
        Transform.rotation = spawnTransform.rotation;

        Anims.Play();
        Audio.Play();

        Renderer.material = DiffuseMaterial;

        StartCoroutine(Fadeout(5));

        //GuiManager.Instance.ShowBloodSplash();
    }

    public void Disable()
    {

    }

    protected IEnumerator Fadeout(float delay)
    {
        if (TransparentMaterial == null)
            yield break;

        yield return new WaitForSeconds(.1f);

        SpriteEffectsManager.Instance.CreateBloodSlatter(Transform, 2, 3);

        yield return new WaitForSeconds(delay);

        CombatEffectsManager.Instance.PlayDisappearEffect(Transform.position, Transform.forward);

        //Material old = Renderer.material;

        Renderer.material = TransparentMaterial;

        Color color = new Color(1, 1, 1, 1);
        TransparentMaterial.SetColor("_Color", color);

        while (color.a > 0)
        {
            color.a -= Time.deltaTime * 4;
            if (color.a < 0)
                color.a = 0;

            TransparentMaterial.SetColor("_Color", color);
            yield return new WaitForEndOfFrame();
        }

        color.a = 0;
        TransparentMaterial.SetColor("_Color", color);

	MissionZone.Instance.ReturnDead(GameObject);
    }

}



