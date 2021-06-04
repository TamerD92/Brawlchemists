using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ListType { Potion, Case, Effect }

public class MakeAChoice : MonoBehaviour
{
    public PlayerController mainController;

    public ListType list;

    public bool isChoosen;

    public int selectedIngrediantOne = 0;

    public TextMesh Text;

    public List<GameObject> ingrediantOne = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //SelectedIngrediantOne();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChoosen)
        {
            ChooseIngrediant();
        }
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
    void SelectedIngrediantOne()
    {

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
