using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Operations
{
    ADD,
    SUB,
    MUL,
    DIV,
    COUNT
}
public class EquationManager : MonoBehaviour
{
    public static EquationManager manager;
    public Operations opType;
    public float numberOne, numberTwo, numberThree;
    public int minNumber, maxNumber;
    public float selectedNum;

    private void Awake()
    {
        if (manager != null && manager != this)
        {
            Destroy(gameObject);
            return;
        }
        manager = this;
    }

    public void GenerateNewOperation()
    {
        minNumber = 0;
        maxNumber = 10;
        opType = (Operations)Random.Range(0, (int)Operations.COUNT);
        if (opType.Equals(Operations.MUL))
            minNumber = 1;
        if (opType.Equals(Operations.DIV))
            minNumber = 1;
            
        numberOne = Random.Range(minNumber, maxNumber);
        numberTwo = Random.Range(minNumber, maxNumber);
        switch (opType)
        {
            case Operations.ADD:
                numberThree = numberOne + numberTwo;
                break;
            case Operations.SUB:
                numberThree = numberOne - numberTwo;
                break;
            case Operations.MUL:
                numberThree = numberOne * numberTwo;
                break;
            case Operations.DIV:
                numberThree = numberOne / numberTwo;
                break;
            default:
                break;
        }
        UIManager.manager.UpdateEquationDisplay();
    }
    // Start is called before the first frame update
    void Start()
    {
        GenerateNewOperation();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
            GenerateNewOperation();
    }
}
