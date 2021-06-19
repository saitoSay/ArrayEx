using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    GameObject[] m_cubeArray;
    int m_selectNum = 0;
    [SerializeField] int m_cubeNumber;
    bool[] m_destroyCheck;
    int m_count;
    void Start()
    {
        m_cubeArray = new GameObject[m_cubeNumber];
        m_destroyCheck = new bool[m_cubeNumber];
        for (int i = 0; i < m_cubeNumber; i++)
        {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (m_cubeNumber % 2 == 0)
            {
                cube.transform.position = new Vector3(-m_cubeNumber + 1 + (i * 2), 0, 0);
            }
            else
            {
                cube.transform.position = new Vector3(-m_cubeNumber + 1 + (i * 2), 0, 0);
            }
            m_cubeArray[i] = cube;
            m_destroyCheck[i] = false;
            var r = cube.GetComponent<Renderer>();
            r.material.color = (i == 0 ? Color.red : Color.white);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 1; i <= m_cubeArray.Length; i++)
            {
                if (m_selectNum + i < m_cubeArray.Length && !m_destroyCheck[m_selectNum + i])
                {
                    m_selectNum += i;
                    ColorChange(m_selectNum);
                    Debug.Log(m_selectNum);
                    break;
                }
                else if(m_selectNum + i == m_cubeArray.Length && m_destroyCheck[0])
                {
                    for (int k = 1; k < m_destroyCheck.Length - 1; k++)
                    {
                        if (!m_destroyCheck[k])
                        {
                            m_selectNum = k;
                            ColorChange(m_selectNum);
                            break;
                        }
                    }
                    Debug.Log(m_selectNum);
                    break;

                }
                else if (m_selectNum + i == m_cubeArray.Length && !m_destroyCheck[0])
                {
                    m_selectNum = 0;
                    ColorChange(m_selectNum);
                    Debug.Log(m_selectNum);
                    break;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = -1; i >= -m_cubeArray.Length; i--)
            {
                if (m_selectNum + i == -1 && m_destroyCheck[m_cubeNumber - 1])
                {
                    for (int k = m_cubeNumber - 2; k > 1; k--)
                    {
                        if (!m_destroyCheck[k])
                        {
                            m_selectNum = k;
                            ColorChange(m_selectNum);
                            break;
                        }
                    }
                    Debug.Log(m_selectNum);
                    break;
                }
                else if (m_selectNum + i == -1 && !m_destroyCheck[m_cubeNumber - 1])
                {
                    m_selectNum = m_cubeNumber - 1;
                    ColorChange(m_selectNum);
                    Debug.Log(m_selectNum);
                    break;
                }
                else if (m_selectNum + i > -m_cubeArray.Length && !m_destroyCheck[m_selectNum + i])
                {
                    m_selectNum += i;
                    ColorChange(m_selectNum);
                    Debug.Log(m_selectNum);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(m_cubeArray[m_selectNum]);
            m_destroyCheck[m_selectNum] = true;
            
        }
    }
    public void ColorChange(int num)
    {
        for (int i = 0; i < m_cubeArray.Length; i++)
        {
            if (!m_destroyCheck[i])
            {
                m_cubeArray[i].GetComponent<Renderer>().material.color = (i == num ? Color.red : Color.white);
            }
        }
    }
}
