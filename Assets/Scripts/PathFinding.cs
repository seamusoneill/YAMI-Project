using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour {

    public class Noeud
    {
        public Vector2 position;
        public float distanceSource;
        public float distanceTarget;
        public Noeud pere;
        public Noeud left;
        public Noeud right;
        
    }

    Noeud arbre;
    Vector2 target;
    float colliderSize;
    static bool state;
    bool right;
    delegate Vector2 Delegate(Vector2 min, Vector2 max, Vector2 origin, Vector2 hitpoint);
    

    public Vector2 getNearest(GameObject origine, GameObject cible)
    {/*
        List<Noeud> resultat = new List<Noeud>();
        target = cible.transform.position;
        */
        target = cible.transform.position;
        colliderSize = origine.GetComponent<CircleCollider2D>().radius;
        arbre = new Noeud();
        arbre.position = origine.transform.position;
        arbre.distanceSource = 0;
        arbre.distanceTarget = Vector2.Distance(origine.transform.position, cible.transform.position);
        if(!calculerNoeud(arbre))
        {
            if (state)//arbre.left.distanceSource+ arbre.left.distanceTarget < arbre.right.distanceSource+ arbre.right.distanceTarget)
                return arbre.left.position;
            else
                return arbre.right.position;
        }
        return cible.transform.position;// arbre[next].position;


    }

    private bool calculerNoeud(Noeud noeud)
    {
        noeud.left = find(noeud, findLeftPoint);
        if (noeud.left == null)
            return true;

        noeud.right = find(noeud, findRightPoint);
        if (noeud.right == null)
            return true; 

        return false;
    }


    private Noeud find(Noeud pere, Delegate callback)
    {
        Noeud result = new Noeud();
        Vector2 leftPoint;
        Vector2 firstLeftPoint;
        RaycastHit2D hit = Physics2D.Raycast(pere.position, target - pere.position, Vector2.Distance(target, pere.position));
        if (hit.collider == null)
            return null;
       
        firstLeftPoint = callback(hit.collider.bounds.min, hit.collider.bounds.max, pere.position, hit.point-pere.position);
        leftPoint = firstLeftPoint;
        //do
        {
            hit = Physics2D.Raycast(pere.position, leftPoint - pere.position, Vector2.Distance(leftPoint, pere.position));
            if(hit.collider!=null)
                leftPoint = callback(hit.collider.bounds.min, hit.collider.bounds.max, pere.position, hit.point - pere.position);
        } //while (hit.collider != null && (leftPoint.x != firstLeftPoint.x || leftPoint.y != firstLeftPoint.y));
        result.position = leftPoint;
        result.distanceSource = pere.distanceSource + Vector2.Distance(pere.position, result.position);
        result.distanceTarget = Vector2.Distance(result.position, target);
        result.pere = pere;
        return result;
    }

    private Vector2 findLeftPoint(Vector2 min, Vector2 max, Vector2 origin, Vector2 hitpoint)
    {
        Vector2[] points = new Vector2[4];
        points[0] = min - origin;
        points[1] = new Vector2(min.x, max.y) - origin;
        points[2] = new Vector2(max.x, min.y) - origin;
        points[3] = max - origin;

        Vector2 result = points[0];
        for (int i = 1; i < 4; i++)
        {
            if (GetAngle(points[i], hitpoint) > GetAngle(result, hitpoint))
            {
                result = points[i];

            }
        }
        result += origin;
        if (result.x == min.x)
            result.x -= colliderSize;
        else
            result.x += colliderSize;
        if (result.y == min.y)
            result.y -= colliderSize;
        else
            result.y += colliderSize;
        return result;
    }

    private Vector2 findRightPoint(Vector2 min, Vector2 max, Vector2 origin, Vector2 hitpoint)
    {
        Vector2[] points = new Vector2[4];
        points[0] = min - origin;
        points[1] = new Vector2(min.x, max.y) - origin;
        points[2] = new Vector2(max.x, min.y) - origin;
        points[3] = max - origin;

        Vector2 result = points[0];
        for (int i = 1; i < 4; i++)
        {
            if (GetAngle(points[i], hitpoint) < GetAngle(result, hitpoint))
            {
                result = points[i];
            }
        }
        result += origin;
        if (result.x == min.x)
            result.x -= colliderSize;
        else
            result.x += colliderSize;
        if (result.y == min.y)
            result.y -= colliderSize;
        else
            result.y += colliderSize;
        return result;
    }

    private static float GetAngle(Vector2 v1, Vector2 v2)
    {
        float ang = Vector2.Angle(v1, v2);
        Vector3 cross = Vector3.Cross(v1, v2);
        if (cross.z > 0)
            ang *= -1;
        //print("v1:" + v1 + "v2:" + v2 + "ang:" + ang);
        return ang;
    }


    // Use this for initialization
    void Start () {
        if (state)
            right = false;
        else
            right = true;
        state=!state;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
