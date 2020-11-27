using UnityEngine;
using System.Collections;

public class Deer : MonoBehaviour {
    public Animator deer;
    private IEnumerator coroutine;
    public ParticleSystem dust;
    public ParticleSystem dustgallop;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.S))
        {
            deer.SetBool("idle", true);
            deer.SetBool("walking", false);
            deer.SetBool("turnleft", false);
            deer.SetBool("turnright", false);
            deer.SetBool("trotting", false);
            deer.SetBool("trotleft", false);
            deer.SetBool("trotright", false);
            deer.SetBool("galloping", false);
            deer.SetBool("eating", false);
            deer.SetBool("jumping", false);
            deer.SetBool("galloping", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            deer.SetBool("walking", true);
            deer.SetBool("backward", false);
            deer.SetBool("trotting", false);
            deer.SetBool("galloping", false);
            deer.SetBool("eating", false);
            deer.SetBool("up", false);
            deer.SetBool("idle", false);
            deer.SetBool("jumping", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.A))
        {
            deer.SetBool("turnleft", true);
            deer.SetBool("walking", false);
            deer.SetBool("idle", false);
            deer.SetBool("jumping", false);
            deer.SetBool("eating", false);
            StartCoroutine("walk");
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
            walk();
        }
        if (Input.GetKey(KeyCode.D))
        {
            deer.SetBool("turnright", true);
            deer.SetBool("walking", false);
            deer.SetBool("idle", false);
            deer.SetBool("jumping", false);
            deer.SetBool("eating", false);
            StartCoroutine("walk");
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
            walk();
        }
        if (Input.GetKey(KeyCode.Space))
        {
            deer.SetBool("jumping", true);
            deer.SetBool("idle", false);
            deer.SetBool("walking", false);
            deer.SetBool("turnleft", false);
            deer.SetBool("turnright", false);
            deer.SetBool("trotting", false);
            deer.SetBool("trotleft", false);
            deer.SetBool("trotright", false);
            dust.GetComponent<ParticleSystem>().enableEmission = true;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.B))
        {
            deer.SetBool("backward", true);
            deer.SetBool("walking", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.E))
        {
            deer.SetBool("eating", true);
            deer.SetBool("walking", false);
            deer.SetBool("turnleft", false);
            deer.SetBool("turnright", false);
            deer.SetBool("idle", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.F))
        {
            deer.SetBool("idle", false);
            deer.SetBool("attack", true);
            deer.SetBool("galloping", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = true;
            StartCoroutine("idle");
            idle();
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            deer.SetBool("lay", true);
            deer.SetBool("idle", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            deer.SetBool("lay", false);
            deer.SetBool("up", true);
            StartCoroutine("idle");
            idle();
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            deer.SetBool("jumping", false);
            deer.SetBool("died", true);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey("down"))
        {
            deer.SetBool("trotting", true);
            deer.SetBool("backward", false);
            deer.SetBool("walking", false);
            deer.SetBool("galloping", false);
            deer.SetBool("jumping", false);
            deer.SetBool("idle", false);
            dust.GetComponent<ParticleSystem>().enableEmission = true;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
        }
        if (Input.GetKey("up"))
        {
            deer.SetBool("galloping", true);
            deer.SetBool("trotting", false);
            deer.SetBool("trotleft", false);
            deer.SetBool("trotright", false);
            deer.SetBool("walking", false);
            deer.SetBool("jumping", false);
            deer.SetBool("idle", false);
            deer.SetBool("attack", false);
            dust.GetComponent<ParticleSystem>().enableEmission = false;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = true;
        }
        if (Input.GetKey("left"))
        {
            deer.SetBool("trotleft", true);
            deer.SetBool("trotting", false);
            deer.SetBool("jumping", false);
            deer.SetBool("idle", false);
            deer.SetBool("galloping", false);
            dust.GetComponent<ParticleSystem>().enableEmission = true;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
            StartCoroutine("trot");
            trot();
        }
        if (Input.GetKey("right"))
        {
            deer.SetBool("trotright", true);
            deer.SetBool("trotting", false);
            deer.SetBool("jumping", false);
            deer.SetBool("idle", false);
            deer.SetBool("galloping", false);
            dust.GetComponent<ParticleSystem>().enableEmission = true;
            dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
            StartCoroutine("trot");
            trot();
        }
        if (Input.GetKey(KeyCode.Keypad0))
        {
            deer.SetBool("died", true);
            deer.SetBool("idle",false);
        }
    }
    IEnumerator walk()
    {
        yield return new WaitForSeconds(1.4f);
        deer.SetBool("walking", true);
        deer.SetBool("turnleft", false);
        deer.SetBool("turnright", false);
        dust.GetComponent<ParticleSystem>().enableEmission = false;
        dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
    }
    IEnumerator trot()
    {
        yield return new WaitForSeconds(0.4f);
        deer.SetBool("trotting", true);
        deer.SetBool("trotleft", false);
        deer.SetBool("trotright", false);
        dust.GetComponent<ParticleSystem>().enableEmission = true;
        dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
    }
    IEnumerator idle()
    {
        yield return new WaitForSeconds(0.1f);
        deer.SetBool("attack", false);
        deer.SetBool("idle", true);
        deer.SetBool("up", false);
        dust.GetComponent<ParticleSystem>().enableEmission = false;
        dustgallop.GetComponent<ParticleSystem>().enableEmission = false;
    }

}
