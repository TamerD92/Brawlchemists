using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public const float GRAVITY_SCALE = 3;

    public Transform PlayerReference;

    public Potion potionPrefab;

    public List<Potion> potionList;

    public EffectBaseClass[] PotionEffects;

    List<EffectBaseClass> EffectsPool;

    public CaseBaseClass[] CaseTypes;

    public static GameController instance;

    // Start is called before the first frame update
    void Start()
    {
        potionList = new List<Potion>();
        EffectsPool = new List<EffectBaseClass>();
        for (int i = 0; i < 20; i++)
        {
            Potion newPot = Instantiate(potionPrefab);
            potionList.Add(newPot);

            EffectBaseClass eff = Instantiate(PotionEffects[0]);
            EffectsPool.Add(eff);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Potion CreatePotion(CaseBaseClass Case, EffectBaseClass Effect)
    {
        Potion newPot;

        if (instance.potionList.Count > 0)
        {
            newPot = instance.potionList[0];
            instance.potionList.RemoveAt(0);

            EffectBaseClass effect = instance.EffectsPool[0];
            instance.EffectsPool.RemoveAt(0);

            effect.transform.SetParent(newPot.transform);
            effect.transform.localPosition = Vector3.zero;

            //createProperCopy
            effect = Effect;

            newPot.effect = effect;

            newPot.Case = Case;

            newPot.init();

            return newPot;
        }
        else
        {
            return null;
        }
    }
}
