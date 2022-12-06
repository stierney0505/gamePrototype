using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constructScript : MonoBehaviour, spell { //Mostly a copy of onPosScript, but with added methods to create objects in the environment
    [SerializeField] float damage, knockBack; //to decide the damage number
    [SerializeField] char type; //to decide the spell type

    private void Start()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (transform.position.x < player.transform.position.x)
            transform.eulerAngles = new Vector2(0, 180);

    }
    public Vector3 getVector()
    {
        Vector3 postion = Input.mousePosition;
        return postion;

    }
    public void remove() { Destroy(gameObject); }
    public void end() { }
    public float getDamage() { return damage; }
    public char getType() { return type; }

    public void enableCollider()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = true;
    }
    public void disableCollider() { Collider2D col = GetComponent<Collider2D>(); col.enabled = false; }
    public float getKnockBack() { return knockBack; }

    public void createConstruct()//Creates an construct object based on the name of the object the script is attached to
    {
        string name = getName(gameObject.name);
        GameObject construct = Instantiate(Resources.Load("Constructs/" + name + "Construct")) as GameObject;
        if (transform.eulerAngles.y == 180)
            construct.transform.eulerAngles = new Vector2(0, 180);

        construct.transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
        construct.GetComponent<construct>().setDamageType(damage, type);
        construct.transform.position = new Vector2(transform.position.x, transform.position.y);

    }

    public string getName(string objName)
    {
        int indexNum = objName.IndexOfAny("123456".ToCharArray());
        return objName.Substring(0, indexNum+1);
    }

    public void addEnemy(Transform enemyTransform)
    {
        
    }
}
