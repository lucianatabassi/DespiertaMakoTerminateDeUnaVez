using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;
public class Boss : MonoBehaviour
{
    //Movimiento
    public Transform Mako3;
    public float velocidad;
    public float distancia_frenado;
    public float distancia_regreso;
    //range
    public float range;
    private float distToPlayer;
    private bool canShoot;
    public float tiempoDisparoE;
    public GameObject LaBala;
    public Transform PuntoDisparo;
    private Rigidbody2D m_rig;
    //    public float speed;
    //Vida enemigo
    public float PuntosVidaE;  //Conteo de vida
    public float VidaMaximaE = 2;  //Vida maxim
    public Image barraDeVida;
    //Animator
    public Animator anim;
    public bool movimiento;
    [Header("Sonidos")]
    public GameObject[] SonidoBossHerido;

    // Start is called before the first frame update
    void Start()
    {
        m_rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        movimiento = true;
        canShoot = false;
        Mako3 = GameObject.Find("MakoN3").transform; //accede a la posicion de Mako
        gameObject.GetComponent <Animator>().SetBool("bossWalk", false);
        gameObject.GetComponent <Animator>().SetBool("bossIdle", true);
        this.transform.eulerAngles = new Vector3(0,transform.eulerAngles.y + 180,0); //QUITAR CUANDO SE ARREGLE EL SPRITE
        PuntosVidaE = VidaMaximaE;
    }

    // Update is called once per frame
    void Update()
    {
        barraDeVida.fillAmount = PuntosVidaE / VidaMaximaE;

        if (Mako3 == null) { // cuando muere el personaje que deja de ejecutarse el codigo de seguimiento
         return;
        }

        if( movimiento == true){
           Movimiento();
          
            if(canShoot)
            {
               Shoot();
            }

          }
    }
        //COMPORTAMIENTO ENEMIGO - MOVIMIENTO
        //Para separar funcionalidades, a medida que el codigo se hace muy extenso usa region
#region
 void Movimiento()
    {
          distToPlayer = Vector2.Distance(transform.position, Mako3.position);

 if(distToPlayer <= range)
 {
    canShoot = true;
        if(Vector2.Distance(transform.position, Mako3.position)>distancia_frenado)
        {
        transform.position = Vector2.MoveTowards(transform.position, Mako3.position, velocidad*Time.deltaTime);// que traslade su posicion hacia Mako
        gameObject.GetComponent <Animator>().SetBool("bossWalk", true);
        gameObject.GetComponent <Animator>().SetBool("bossShoot", true);
         }
         if(Vector2.Distance(transform.position, Mako3.position)<distancia_regreso)
        {
        transform.position = Vector2.MoveTowards(transform.position, Mako3.position, -velocidad*Time.deltaTime);// que traslade su posicion hacia atras cuando Mako este muy cerca --- al restarle a velocidad, va hacia atras
        gameObject.GetComponent <Animator>().SetBool("bossWalk", true);
        gameObject.GetComponent <Animator>().SetBool("bossShoot", true);

         } 
         if(Vector2.Distance(transform.position, Mako3.position)<distancia_frenado && Vector2.Distance(transform.position, Mako3.position)>distancia_regreso)
        {
            
        transform.position = transform.position;// que se quede quieto entre la distancia de seguir a Mako y la de regreso cuando Mako esta muy cerca
           gameObject.GetComponent <Animator>().SetBool("bossShoot", true);
           gameObject.GetComponent <Animator>().SetBool("bossWalk", false);
           canShoot=true;
       //
         }
        //  transform.position = transform.position;
  
/* 
            if(canShoot)
            {
               Shoot();
           m_rig.velocity = Vector2.zero;
            } */
    }else{

         //m_rig.velocity = new Vector2(velocidad,m_rig.velocity.y);
         transform.position = transform.position;
         canShoot = false;
         gameObject.GetComponent <Animator>().SetBool("bossShoot", false);
         gameObject.GetComponent <Animator>().SetBool("bossWalk", false);
         gameObject.GetComponent <Animator>().SetBool("bossIdle", true);
          // m_rig.velocity = Vector2.zero;
        }
#endregion
        //COMPORTAMIENTO ENEMIGO - MIRAR A MAKO
#region 
       //Flip
       if(Mako3.position.x>this.transform.position.x)
       {

            this.transform.eulerAngles = new Vector3 (0, 0, 0);

    } else {
 
            this.transform.eulerAngles = new Vector3 (0, 180, 0);
    }
          // this.transform.localScale = new Vector2(3,3); //cuando el enemigo esta hacia la derecha
          // this.transform.localScale = new Vector2(-3,3); //cuando esta hacia la izq 
    }
    
#endregion


         //COMPORTAMIENTO ENEMIGO - DISPARAR A MAKO
         void Shoot()
    {
        gameObject.GetComponent <Animator>().SetBool("bossShoot", true);
        //transform.position += transform.right * velBala * Time.deltaTime;
        tiempoDisparoE += Time.deltaTime;
        //m_rig.velocity = Vector2.zero;
        if (tiempoDisparoE >= 1)
    {
       GameObject prefab = Instantiate(LaBala, PuntoDisparo.position, transform.rotation) as GameObject;
        tiempoDisparoE = 0;
        NuevoSonido(SonidoBossHerido[1], 1f);
        
        Destroy(prefab, 2f);
        
        
    }
    }

    //Salto
/*     void OnTriggerEnter2D (Collider2D col)
    {

    } */
/* #region 
        tiempo += Time.deltaTime;
        if(tiempo >= 2)
        {
           Instantiate(bala, punto_instancia.position, transform.rotation); //bala, posicion, rotacion
            tiempo = 0; //el tiempo se setea nuevamente en cero
        }
#endregion */
/* 
      if(mustPatrol)
      {
        Patrol();
      }
      */
    
         //VIDA ENEMIGO

    public void TakeHit(float golpe)
    {
        PuntosVidaE -= golpe;
        // gameObject.GetComponent <Animator>().SetBool("bossHurt", true);
         NuevoSonido(SonidoBossHerido[0], 1f);
         anim.SetTrigger ("bossHurt");
        if(PuntosVidaE <= 0 )
        {
            anim.SetTrigger("bossDead");
            Destroy(gameObject,1f);
              CambiarEscenaGameOver();
        }
    }

    void NuevoSonido (GameObject prefab, float duracion = 5f) {
         Destroy (Instantiate(prefab), duracion);
    }

    void CambiarEscenaGameOver()
     {
        SceneManager.LoadScene(10);
     }

}
