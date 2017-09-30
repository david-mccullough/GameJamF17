using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public float influenceScale = 0f; //-1 to 1
    public int maxUnits = 50;

    public int numPlayerUnits;
    public int numAIUnits;
    private float radius = 5f;

    [SerializeField]
    private List<Node> neighbors;

    public int lengthOfLineRenderer = 20;

    public LineRenderer lineRenderer;
    public Color c1 = Color.yellow;
    public Color c2 = Color.red;


    void Start()
    {
        neighbors = new List<Node>();
        GetNeighbors();

        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
    }

    void Update()
    {

        influenceScale = UpdateInfluenceScale(numPlayerUnits, numAIUnits);
        lineRenderer.enabled = false;
        RaycastHit hit;
        if (MouseCast(out hit))
        {
            if (hit.transform == this.transform)
            {
                var t = Time.time;
                foreach(var node in hit.transform.GetComponent<Node>().neighbors)
                {
                    //Debug.Log(node.gameObject.name + " " + hit.transform.name);
                    lineRenderer.enabled = true;
                    node.lineRenderer.SetPosition(0, node.transform.position);
                    node.lineRenderer.SetPosition(lineRenderer.numPositions-1, hit.transform.position);

                    // A simple 2 color gradient with a fixed alpha of 1.0f.
                    float alpha = 1.0f;
                    Gradient gradient = new Gradient();
                    gradient.SetKeys(
                        new GradientColorKey[] { new GradientColorKey(c1, 0.0f), new GradientColorKey(c2, 1.0f) },
                        new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
                        );
                    lineRenderer.colorGradient = gradient;
                }

            }
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    // Influence lies between -1 and 1 and is determined by the difference
    // between player and ai units over the max unit count.
    // example: influence is -0.75 where there are 80 ai units and 5 player units
    // at a 100 unit capacity node 
    private int UpdateInfluenceScale(int playerUnits, int aiUnits)
    {
        int diff = playerUnits - aiUnits;
        return (diff / maxUnits);
    }

    private void GetNeighbors()
    {
        Collider[] hitNodes = Physics.OverlapSphere(this.transform.position, radius);
        int i = 0;
        while (i < hitNodes.Length)
        {
            if (hitNodes[i] != gameObject.GetComponent<Collider>())
            {
                Debug.Log(gameObject.name + " adding " + hitNodes[i].gameObject.name + " as neighbor");
                neighbors.Add(hitNodes[i].gameObject.GetComponent<Node>());
            }
            i++;
        }
    }

    bool MouseCast(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, 9999f);
    }
}
