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

    public SpriteRenderer image, filling;

    public List<GameObject> ingrediantOne = new List<GameObject>();

    bool scroll;

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
            float dir = Input.GetAxis("Mouse ScrollWheel");
            if (dir != 0)
            {
                ChooseIngrediant(dir);
            }

        }
    }

    public void resetIngridiants()
    {
        PictureStruct pic = mainController.increaseSelector(list);

        image.sprite = pic.image;

        if (pic.image2)
        {
            image.color = pic.color;
            filling.sprite = pic.image2;
            filling.color = pic.color2;
        }
        else if(filling)
        {
            filling.sprite = null;
        }
    }

    public void ChooseIngrediant(float dir)
    {
        //int previousSelectedIngrediant = selectedIngrediantOne;

        PictureStruct pic = new PictureStruct();

        if (dir > 0f)
        {
            pic = mainController.increaseSelector(list);

        }
        if (dir < 0f)
        {
            pic = mainController.decreaseSelector(list);

        }

        image.sprite = pic.image;

        if (pic.image2)
        {
            image.color = pic.color;
            filling.sprite = pic.image2;
            filling.color = pic.color2;
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
