using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(LineRenderer))]
public class ProjectileLine : MonoBehaviour
{
    static List<ProjectileLine> PROJ_LINES = new List<ProjectileLine>();
    private const float DIM_MULT = 0.75f;

    // Underscores remind us that these are private
    private LineRenderer _line;
    private bool _drawing = true;
    private Projectile _projectile;

    void Start()
    {
        _line = GetComponent<LineRenderer>();
        _line.positionCount = 1;
        _line.SetPosition(0, transform.position);   // Get a reference to the linerenderer and assign its first position

        _projectile = GetComponentInParent<Projectile>();   // Allows us to search up Hierarchy from this to look for components

        ADD_LINE(this);
    }

    void FixedUpdate()
    {
        if (_drawing)
        {
            _line.positionCount++;
            _line.SetPosition(_line.positionCount - 1, transform.position); // This causes projectileline to follow behind projectile
            // If the Projectile Rigidbody is sleeping, stop drawing
            if (_projectile != null)
            {
                if (!_projectile.awake)     // Line continues drawing until projectile is not awake
                {
                    _drawing = false;
                    _projectile = null;
                }
            }
        }
    }

    private void OnDestroy()
    {
        // Remove this ProjectileLine from PROJ_LINES
        PROJ_LINES.Remove(this);
    }

    static void ADD_LINE(ProjectileLine newLine)
    {
        Color col;
        // Iterate over all the old lines and dim them
        foreach (ProjectileLine pl in PROJ_LINES)
        {
            col = pl._line.startColor;
            col = col * DIM_MULT;
            pl._line.startColor = pl._line.endColor = col;
        }
        // Add newLine to the List
        PROJ_LINES.Add(newLine);
    }
}
