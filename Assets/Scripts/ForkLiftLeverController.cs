using UnityEngine;

public class ForkLiftLeverController : MonoBehaviour
{
    [Header("Movement Settings")]
    public Transform ridge;                       // Reference to the ridge object (forklift)
    public float poleSpeed = 3f;                  // Speed of the pole's vertical movement
    public float upperLimitPole = 10f;            // Upper movement limit for the pole
    public float lowerLimitPole = 0f;             // Lower movement limit for the pole

    [Header("Blades Settings")]
    public Transform blades;                      // Reference to the blades object
    public Transform secondaryObject;             // Reference to the second object that moves with blades
    public float bladesUpDownSpeed = 3f;          // Speed of the blades' vertical movement
    public float bladesInOutSpeed = 3f;           // Speed of the blades' in/out movement
    public float threshold = 1f;                  // Max distance blades and secondary object can move
    public float upperLimitBladesUpDown = 10f;    // Upper limit for blades' vertical movement
    public float lowerLimitBladesUpDown = 0f;     // Lower limit for blades' vertical movement
    public float maxInOutDistance = 5f;           // Maximum in/out distance for blades
    public float minInOutDistance = -5f;          // Minimum in/out distance for blades

    [Header("Lever Settings")]
    public Transform leverPole;                   // Reference to the pole lever (lever 1)
    public Transform leverBladesUpDown;           // Reference to the vertical movement lever (lever 2)
    public Transform leverBladesInOut;            // Reference to the in/out movement lever (lever 3)
    public float leverSensitivity = 1f;           // Sensitivity for lever movement effect
    public float neutralLeverRotationX = -90f;    // Set this to match the lever's starting x rotation

    private Vector3 initialBladesPosition;        // Store the initial position of blades
    private Vector3 initialSecondaryObjectPosition; // Store initial position of secondary object

    void Start()
    {
        initialBladesPosition = blades.localPosition;
        initialSecondaryObjectPosition = secondaryObject.localPosition;
    }

    void Update()
    {
        // Update the movement for all levers
        MovePole();
        MoveBladesUpDown();
        MoveBladesInOut();
    }

    // Controls the vertical movement of the pole (lever 1)
    private void MovePole()
    {
        Vector3 newPosition = ridge.localPosition;

        float leverRotationX = leverPole.localEulerAngles.x;
        if (leverRotationX > 180f) leverRotationX -= 360f;
        float rotationOffset = leverRotationX - neutralLeverRotationX;

        newPosition.y += Mathf.Sign(rotationOffset) * Mathf.Abs(rotationOffset) * leverSensitivity * poleSpeed * Time.deltaTime;

        newPosition.y = Mathf.Clamp(newPosition.y, lowerLimitPole, upperLimitPole);

        ridge.localPosition = newPosition;
    }

    // Controls the vertical movement of the blades and secondary object (lever 2)
    private void MoveBladesUpDown()
    {
        float leverRotationX = leverBladesUpDown.localEulerAngles.x;
        if (leverRotationX > 180f) leverRotationX -= 360f;
        float rotationOffset = leverRotationX - neutralLeverRotationX;

        Vector3 newBladesPosition = blades.localPosition;
        Vector3 newSecondaryPosition = secondaryObject.localPosition;

        newBladesPosition.y += Mathf.Sign(rotationOffset) * Mathf.Abs(rotationOffset) * leverSensitivity * bladesUpDownSpeed * Time.deltaTime;
        newSecondaryPosition.y += Mathf.Sign(rotationOffset) * Mathf.Abs(rotationOffset) * leverSensitivity * bladesUpDownSpeed * Time.deltaTime;

        newBladesPosition.y = Mathf.Clamp(newBladesPosition.y, lowerLimitBladesUpDown, upperLimitBladesUpDown);
        newSecondaryPosition.y = Mathf.Clamp(newSecondaryPosition.y, lowerLimitBladesUpDown, upperLimitBladesUpDown);

        blades.localPosition = newBladesPosition;
        secondaryObject.localPosition = newSecondaryPosition;
    }

    // Controls the in/out movement of the blades (lever 3)
    private void MoveBladesInOut()
    {
        float leverRotationX = leverBladesInOut.localEulerAngles.x;
        if (leverRotationX > 180f) leverRotationX -= 360f;
        float rotationOffset = leverRotationX - neutralLeverRotationX;

        Vector3 newPosition = blades.localPosition;

        newPosition.z += Mathf.Sign(rotationOffset) * Mathf.Abs(rotationOffset) * leverSensitivity * bladesInOutSpeed * Time.deltaTime;
        newPosition.z = Mathf.Clamp(newPosition.z, minInOutDistance, maxInOutDistance);

        blades.localPosition = newPosition;
    }
}

