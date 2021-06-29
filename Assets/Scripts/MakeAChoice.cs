using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public enum ListType { Potion, Case, Effect }

public class MakeAChoice : MonoBehaviourPunCallbacks
{
    public PlayerController mainController;

    public ListType list;

    public bool isChoosen;

    public TextMesh Text;

    public List<GameObject> ingrediantOne = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
        {
            gameObject.SetActive(false);
        }

        //SelectedIngrediantOne();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoosen && photonView.IsMine)
        {
            ChooseIngrediant();
        }
    }

    public void resetIngridiants()
    {
        Text.text = mainController.increaseSelector(list);
    }

    public void ChooseIngrediant()
    {
        //int previousSelectedIngrediant = selectedIngrediantOne;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Text.text = mainController.increaseSelector(list);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Text.text = mainController.decreaseSelector(list);
        }

        //if (previousSelectedIngrediant != selectedIngrediantOne)
        //{
        //    SelectedIngrediantOne();
        //}
    }

    private void OnMouseEnter()
    {
        isChoosen = true;
    }
    private void OnMouseExit()
    {
        isChoosen = false;
    }
}
