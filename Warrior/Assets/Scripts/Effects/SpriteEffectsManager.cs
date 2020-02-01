using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteManager))]

public class SpriteEffectsManager : MonoBehaviour
{
    
    public int MaxSprites = 30;

    private ArrayList Sprites = new ArrayList();
    private ArrayList Shadows = new ArrayList();

    private SpriteManager SpriteManager;

    public static SpriteEffectsManager Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        SpriteManager = GetComponent<SpriteManager>();
    }

    void Start()
    {
    }

    void LateUpdate()
    {
        Sprite s;
        for (int i = 0; i < Shadows.Count; i++)
        {
            s = Shadows[i] as Sprite;
            if (s.hidden)
                continue;
            SpriteManager.TransformToGround(s);
        }
    }

    void FixedUpdate()
    {
        if (Sprites.Count > MaxSprites)
        {
            SpriteManager.RemoveSprite(Sprites[0] as Sprite);
            Sprites.RemoveAt(0);
        }
    }

    public void CreateBloodSlatter(Transform t, float minLen, float maxLen)
    {
        float f = Random.Range(minLen, maxLen);
        GameObject obj = new GameObject("splatter");

		obj.transform.parent = AutoElementManager.Instance.transform;

        obj.transform.position = new Vector3(t.position.x, t.position.y, t.position.z);
        obj.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);

        Sprite s = SpriteManager.AddSprite(obj, 0.2f, 0.2f, 0, 128, 128, 128, false);
        StartCoroutine(UpdateSplatter(s, 0.5f, f, 0.8f));
    }

    public void CreateBlood(Transform t)
    {
        float f = Random.Range(0.9f, 1.3f);

        GameObject obj = new GameObject("splash");

		obj.transform.parent = AutoElementManager.Instance.transform;

        obj.transform.position = new Vector3(t.position.x, t.position.y, t.position.z);
        //obj.transform.rotation = t.rotation;
        obj.transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        //Sprites.Add(BloodSplattersManager.AddSprite(obj, f, f, Vector2.zero, new Vector2(1, 1), false));


        switch (Random.Range(0, 10) % 2)
        {
            case 0:
                Sprites.Add(SpriteManager.AddSprite(obj, f, f, 128, 128, 128, 128, false));
                break;
            default:
                Sprites.Add(SpriteManager.AddSprite(obj, f, f, 0, 256, 128,  128, false));
                break;
        }
    }

    public void CreateShadow(GameObject parent, float width, float height)
    {
        Sprite s;
        for (int i = 0; i < Shadows.Count; i++)
        {
            s = Shadows[i] as Sprite;
            if (s.hidden == false)
                continue;

            s.client = parent;
            SpriteManager.ShowSprite(s);
            return;
        }
        s = SpriteManager.AddSprite(parent, width, height, 172, 213, 85, 85, false);
        s.offset.y += 0.2f;
        s.SetSizeXZ(width, height);
        Shadows.Add(s);
    }

    public void ReleaseShadow(GameObject parent)
    {
        Sprite s;
        for (int i = 0; i < Shadows.Count; i++)
        {
            s = Shadows[i] as Sprite;
            if (s.client != parent)
                continue;
            SpriteManager.HideSprite(s);
        }
    }

    public void ReleaseBloodSprites()
    {
        while (Sprites.Count > 0)
        {
            SpriteManager.RemoveSprite(Sprites[0] as Sprite);
            Sprites.RemoveAt(0);

		}
    }

    public void ReleaseShadows()
    {
        while (Shadows.Count > 0)
        {
            SpriteManager.RemoveSprite(Shadows[0] as Sprite);
            Shadows.RemoveAt(0);
        }
    }

    IEnumerator UpdateSplatter(Sprite s, float size, float maxSize, float speed)
    {
        float f = size;

        while (f < maxSize)
        {

            f += speed * Time.deltaTime;
            if (f > maxSize)
                f = maxSize;
            s.SetSizeXZ(f, f);
            yield return new WaitForEndOfFrame();
        }

        Sprites.Add(s);
    }

}
