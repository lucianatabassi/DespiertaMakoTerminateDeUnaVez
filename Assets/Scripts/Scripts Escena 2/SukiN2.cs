using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SukiN2 : MonoBehaviour
{
    public GameObject Suki;
    public Transform PuntoPartida;
    public Transform PuntoLimite;
    public float velSuki;
    private Vector3 MoverHacia;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        MoverHacia = PuntoLimite.position; //cuando se ejecute otra vez, q se mueva hacia el punto final
    }

    
    void Update()
    {
        Suki.transform.position = Vector3.MoveTowards(Suki.transform.position, MoverHacia, velSuki * Time.deltaTime);

       /* if (objetoAmover.transform.position == PuntoFin.position) { //si llega al punto final  que vuelva hacia el punto de partida
            MoverHacia = PuntoPartida.position;
        }*/

        if (Suki.transform.position == PuntoPartida.position) { //si llega al punto inicial  que vuelva hacia el punto final
            MoverHacia = PuntoLimite.position;
            
        }


        if (Suki.transform.position == PuntoLimite.position)
        {
            anim.SetBool("run", false);
            velSuki = 0;
        }
    }
}
