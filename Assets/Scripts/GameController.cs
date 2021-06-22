using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

public class GameController : MonoBehaviourPunCallbacks
{
    public const float GRAVITY_SCALE = 3;

    public const float PICKUP_TIME = 1;

    public const int PICKUP_LIMIT = 15;

    public int PickupAmount;

    public float PickupTimer;

    public Transform PlayerReference;

    public Potion potionPrefab;

    public ObjectDatabase database;

    public List<Potion> potionList;

    List<EffectBaseClass> EffectsPool;

    public List<PickupBase> PickupPool;

    public Transform PotionPoolTransform, EffectPoolTransform, PickupPoolTransform;

    public GroundLinesStruct[] GroundLines;

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {

        if (instance == null)
        {
            instance = this;
        }

        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("GeneratePools", RpcTarget.MasterClient);
        }
        else
        {
            foreach (var item in GetComponents<Potion>())
            {
                item.gameObject.SetActive(false);
            }
        }

    }

    [PunRPC]
    private void GeneratePools()
    {
        PickupAmount = 0;
        PickupTimer = 0;

        potionList = new List<Potion>();
        EffectsPool = new List<EffectBaseClass>();
        PickupPool = new List<PickupBase>();
        for (int i = 0; i < 40; i++)
        {
            GameObject newPot = PhotonNetwork.Instantiate(potionPrefab.name, Vector3.zero, Quaternion.identity);
            newPot.SetActive(false);
            potionList.Add(newPot.GetComponent<Potion>());

            newPot.transform.SetParent(PotionPoolTransform);
            newPot.transform.localPosition = new Vector3(-1000, -1000, -1000);
            newPot.GetComponent<Potion>().preInit();

            GameObject eff = PhotonNetwork.Instantiate(database.EffectsTypes[i % 5].name, Vector3.zero, Quaternion.identity);
            eff.name = "Effect" + database.EffectsTypes[i % 5].name;
            eff.SetActive(false);
            EffectsPool.Add(eff.GetComponent<EffectBaseClass>());


            eff.transform.SetParent(EffectPoolTransform);
            eff.transform.localPosition = new Vector3(-1000, -1000, -1000);

            GameObject pickup = PhotonNetwork.Instantiate(database.PickupTypes[i % 2].name, Vector3.zero, Quaternion.identity);
            PickupPool.Add(pickup.GetComponent<PickupBase>());

            pickup.transform.SetParent(PickupPoolTransform);
            pickup.transform.localPosition = new Vector3(-1000, -1000, -1000);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }

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

    public void ReturnToPoolBase(object obj)
    {
        if (obj is Potion)
        {
            photonView.RPC("ReturnToPool", RpcTarget.MasterClient, obj as Potion);
        }
        else if (obj is EffectBaseClass)
        {
            photonView.RPC("ReturnToPool", RpcTarget.MasterClient, obj as EffectBaseClass);
        }
        else if (obj is PickupBase)
        {
            photonView.RPC("ReturnToPool", RpcTarget.MasterClient, obj as PickupBase);
        }
    }


    [PunRPC]
    public void ReturnToPool(Potion potion)
    {
        potion.transform.SetParent(PotionPoolTransform);
        potion.transform.localPosition = new Vector3(-1000, -1000, -1000);
        //potion.gameObject.SetActive(false);
    } 
    
    [PunRPC]
    public void ReturnToPool(EffectBaseClass effect)
    {
        effect.transform.SetParent(EffectPoolTransform);
        effect.transform.localPosition = new Vector3(-1000, -1000, -1000);
        effect.gameObject.SetActive(false);
    }
    
    [PunRPC]
    public void ReturnToPool(PickupBase pickup)
    {
        pickup.transform.SetParent(PickupPoolTransform);
        pickup.transform.localPosition = new Vector3(-1000, -1000, -1000);

        PickupAmount--;
    }
}
