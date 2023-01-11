using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class water5Script : MonoBehaviour
{

    public float damage, knockBack;
    float time, rotation;
    bool started = false, left;
    public dLRSNode.types type;
    water5AttackScript spellScript;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerSource = player.transform.GetChild(0).position;
        rotate(playerSource, transform.position);
        transform.position = playerSource;

        if (transform.position.x < player.transform.position.x)
            left = true;
    }

    private void Update()
    {
        if (started) {
            time += Time.deltaTime;
            if (time >= 5f)
            {
                animator.SetTrigger("fade");
                spellScript.end(false);
            }   
        }
    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void rotate(Vector2 start, Vector2 end)
    {
        if (start.x > end.x) { transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y); }
        float startX = start.x;
        float startY = start.y;
        float endX = end.x;
        float endY = end.y;
        float slope = (endY - startY) / (endX - startX);
        rotation = Mathf.Rad2Deg * Mathf.Atan(slope);
        transform.Rotate(0, 0, rotation);
    }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum + 1);
    }

    public void createSpell() {
        GameObject childBeam = Instantiate(Resources.Load("Spells/" + getName(gameObject.name) + "Attack") as GameObject);
        spellScript = childBeam.GetComponent<water5AttackScript>();
        spellScript.parent = transform;
        spellScript.type = type;
        spellScript.damage = damage;
        spellScript.knockBack = knockBack;
        spellScript.rotation = rotation;
        spellScript.left = left;
        started = true;
    }
}
