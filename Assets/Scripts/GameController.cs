using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const float GRAVITY_SCALE = 3;

    public const float PICKUP_TIME = 1;

    public const int PICKUP_LIMIT = 15;

    public int PickupAmount;

    public float PickupTimer;

    public Transform PlayerReference;

    public Potion potionPrefab;

    public EffectBaseClass[] EffectsTypes;

    public CaseBaseClass[] CaseTypes;

    public PickupBase[] PickupTypes;

    public List<Potion> potionList;

    List<EffectBaseClass> EffectsPool;

    public List<PickupBase> PickupPool;

    public Transform PotionPoolTransform, EffectPoolTransform, PickupPoolTransform;

    public GroundLinesStruct[] GroundLines;

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        PickupAmount = 0;
        PickupTimer = 0;

        potionList = new List<Potion>();
        EffectsPool = new List<EffectBaseClass>();
        PickupPool = new List<PickupBase>();
        for (int i = 0; i < 40; i++)
        {
            Potion newPot = Instantiate(potionPrefab);
            potionList.Add(newPot);

            newPot.transform.SetParent(PotionPoolTransform);
            newPot.transform.localPosition = Vector3.zero;
            newPot.preInit();

            EffectBaseClass eff = Instantiate(EffectsTypes[i%5]);
            EffectsPool.Add(eff);

            eff.transform.SetParent(EffectPoolTransform);
            eff.transform.localPosition = Vector3.zero;

            PickupBase pickup = Instantiate(PickupTypes[i % 2]);
            PickupPool.Add(pickup);

            pickup.transform.SetParent(PickupPoolTransform);
            pickup.transform.localPosition = Vector3.zero;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        PickupTimer += Time.deltaTime;

        if (PickupTimer >= PICKUP_TIME && PickupAmount < PICKUP_LIMIT)
        {
            GeneratePickup();
            PickupTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            //GeneratePickup();
        }
    }

    public void GeneratePickup()
    {

        PickupBase pickup;
        Vector3 finalWorldPos = new Vector3();

        int type = Random.Range(0, 100);
        if (type < 60)
        {
            pickup = PickupPool.First(o => o is EffectPickup);
        }
        else
        {
            pickup = PickupPool.First(o => o is CasePickup);
        }

        PickupPool.Remove(pickup);

        pickup.transform.SetParent(null);
        pickup.Generate();

        GroundLinesStruct baseLocation = GroundLines[Random.Range(0, GroundLines.Length - 1)];

        finalWorldPos.y = baseLocation.Base.position.y;
        finalWorldPos.x = Random.Range(baseLocation.Start.position.x, baseLocation.End.position.x);

        pickup.transform.position = finalWorldPos;

        PickupAmount++;
    }

    public static Potion CreatePotion(CaseBaseClass Case, EffectBaseClass Effect)
    {
        Potion newPot;

        if (instance.potionList.Count > 0)
        {
            newPot = instance.potionList[0];
            instance.potionList.RemoveAt(0);

            EffectBaseClass effect = instance.EffectsPool.Where(o => o.ID == Effect.ID).ToList()[0];
            instance.EffectsPool.Remove(effect);

            effect.transform.SetParent(newPot.transform);
            effect.transform.localPosition = Vector3.zero;

            //createProperCopy
            //effect = Effect;

            newPot.effect = effect;

            newPot.Case = Case;

            newPot.init();
            newPot.gameObject.SetActive(false);

            return newPot;
        }
        else
        {
            return null;
        }
    }

    public void ReturnToPool(Potion potion)
    {
        potion.transform.SetParent(PotionPoolTransform);
        potion.transform.localPosition = Vector3.zero;
        //potion.gameObject.SetActive(false);
    }

    public void ReturnToPool(EffectBaseClass effect)
    {
        effect.transform.SetParent(EffectPoolTransform);
        effect.transform.localPosition = Vector3.zero;
        effect.gameObject.SetActive(false);
    }

    public void ReturnToPool(PickupBase pickup)
    {
        pickup.transform.SetParent(PickupPoolTransform);
        pickup.transform.localPosition = Vector3.zero;

        PickupAmount--;
    }
}
