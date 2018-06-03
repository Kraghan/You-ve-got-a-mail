using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texture_random : MonoBehaviour {

	public Material[] Couleurs;
	public Transform All_buildings;

	// Use this for initialization
	void Start () {
		
	}
	
	//Fonction de couleurs aléatoires dans notre palette
	public void Color_them_all () {
		
		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

        foreach (Transform lebat in Bat_all) {

            if ((lebat.name == "Batiment") || (lebat.name == "Etage_01") || (lebat.name == "Etage_02"))
            {

                //Debug.Log("Ancien" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

                //Je choisis une couleur aléatoire
                Material lacouleur = Couleurs[Random.Range(0, Couleurs.Length)];

                //Je récupère ses matériaux
                Material[] mesmats = lebat.GetComponent<Renderer>().sharedMaterials;

                //J'y affecte la couleur aléatoire pour la couleur du mur
                for (int i = 0; i < mesmats.Length;i++)
                {

					if ((mesmats[i].name != "Blanc") && (mesmats[i].name != "Fenetre") && (mesmats[i].name != "BrushedMetal"))
                    {

                        Debug.Log("ici");

                        mesmats[i] = lacouleur;

                    }

                }
               

                //Je donne cette couleur au mur pour de bon
                lebat.GetComponent<Renderer>().sharedMaterials = mesmats;

                //Debug.Log("Nouveau" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

                foreach (Transform ladeco in lebat)
                {

                    //Si c'est une décoration je la colore comme le batiment
                    if (ladeco.tag == "Bat_Deco")
                    {
                        //Je récupère ses matériaux
                        Material[] decomats = ladeco.GetComponent<Renderer>().sharedMaterials;

                        //J'y affecte la couleur aléatoire pour la couleur du mur
                        //J'y affecte la couleur aléatoire pour la couleur du mur
                        for (int i = 0; i < decomats.Length; i++)
                        {

                            if ((decomats[i].name != "Blanc") && (decomats[i].name != "Fenetre"))
                            {

                                decomats[i] = lacouleur;

                            }

                        }

                        //Je donne cette couleur au mur pour de bon
                        ladeco.GetComponent<Renderer>().sharedMaterials = decomats;

                    }
                }
            }
		}
	}

	//Fonction saruman du fun
	public void Saruman_the_multicolored () {

		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if (lebat.name == "Batiment") {

				//Debug.Log("Ancien" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

				Material[] mesmats = lebat.GetComponent<Renderer> ().materials;

				mesmats[0].color = Random.ColorHSV();

				lebat.GetComponent<Renderer> ().materials = mesmats;

				//Debug.Log("Nouveau" + lebat.GetComponent<Renderer>().sharedMaterials[0]);

			}		
		}
	}

	//Fonction de couleurs aléatoires dans notre palette
	public void Change_height () {

		Transform[] Bat_all = All_buildings.GetComponentsInChildren<Transform> ();

		foreach (Transform lebat in Bat_all) {

			if (lebat.name == "Batiment") {

				//Je lui donne une hauteur aléatoire entre 1 et 1.2

				lebat.transform.localScale.Set (lebat.transform.localScale.x, 1 + (Random.value * 0.1f), lebat.transform.localScale.z);

			}
		}
	}
}