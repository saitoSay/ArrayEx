using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ans : MonoBehaviour
{
    GameObject[,] m_cubeArray;
    int m_selectNumX = 0;
    int m_selectNumY = 0;
    [SerializeField] int m_cubeWidthSize;
    [SerializeField] int m_cubeHeigthSize;
    bool[,] m_destroyCheck;
    void Start()
    {
        m_cubeArray = new GameObject[m_cubeWidthSize,m_cubeHeigthSize];
        m_destroyCheck = new bool[m_cubeWidthSize,m_cubeHeigthSize];
        for (int i = 0; i < m_cubeWidthSize; i++)
        {
            for (int k = 0; k < m_cubeHeigthSize; k++)
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(-m_cubeWidthSize + 1 + (i * 2), (k * 2), 0);
                m_cubeArray[i,k] = cube;
                m_destroyCheck[i, k] = false;
                var r = cube.GetComponent<Renderer>();
                r.material.color = (i == 0 && k == 0 ? Color.red : Color.white);
            }
        }
        Debug.Log(m_cubeArray.GetLength(0));
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            for (int i = 1; i <= m_cubeArray.GetLength(0); i++)
            {
                if (m_selectNumX + i < m_cubeArray.GetLength(0) && !m_destroyCheck[m_selectNumX + i, m_selectNumY])
                {
                    m_selectNumX += i;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumX + i == m_cubeArray.GetLength(0) && m_destroyCheck[0, m_selectNumY])
                {
                    for (int k = 1; k < m_destroyCheck.GetLength(0) - 1; k++)
                    {
                        if (!m_destroyCheck[k, m_selectNumY])
                        {
                            m_selectNumX = k;
                            ColorChange(m_selectNumX, m_selectNumY);
                            break;
                        }
                    }
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;

                }
                else if (m_selectNumX + i == m_cubeArray.GetLength(0) && !m_destroyCheck[0, m_selectNumY])
                {
                    m_selectNumX = 0;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            for (int i = -1; i >= -m_cubeArray.GetLength(0); i--)
            {
                if (m_selectNumX + i == -1 && m_destroyCheck[m_cubeWidthSize - 1, m_selectNumY])
                {
                    for (int k = m_cubeWidthSize - 2; k > 1; k--)
                    {
                        if (!m_destroyCheck[k, m_selectNumY])
                        {
                            m_selectNumX = k;
                            ColorChange(m_selectNumX, m_selectNumY);
                            break;
                        }
                    }
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumX + i == -1 && !m_destroyCheck[m_cubeWidthSize - 1, m_selectNumY])
                {
                    m_selectNumX = m_cubeWidthSize - 1;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumX + i > -m_cubeArray.GetLength(0) && !m_destroyCheck[m_selectNumX + i, m_selectNumY])
                {
                    m_selectNumX += i;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            for (int i = 1; i <= m_cubeArray.GetLength(1); i++)
            {
                if (m_selectNumY + i < m_cubeArray.GetLength(1) && !m_destroyCheck[m_selectNumX, m_selectNumY + i])
                {
                    m_selectNumY += i;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumY + i == m_cubeArray.GetLength(1) && m_destroyCheck[m_selectNumX, 0])
                {
                    for (int k = 1; k < m_destroyCheck.GetLength(1) - 1; k++)
                    {
                        if (!m_destroyCheck[m_selectNumX, 0])
                        {
                            m_selectNumY = k;
                            ColorChange(m_selectNumX, m_selectNumY);
                            break;
                        }
                    }
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;

                }
                else if (m_selectNumY + i == m_cubeArray.GetLength(1) && !m_destroyCheck[m_selectNumX, 0])
                {
                    m_selectNumY = 0;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            for (int i = -1; i >= -m_cubeArray.GetLength(1); i--)
            {
                if (m_selectNumY + i == -1 && m_destroyCheck[m_selectNumX, m_cubeHeigthSize - 1])
                {
                    for (int k = m_cubeHeigthSize - 2; k > 1; k--)
                    {
                        if (!m_destroyCheck[m_selectNumX, k])
                        {
                            m_selectNumY = k;
                            ColorChange(m_selectNumX, m_selectNumY);
                            break;
                        }
                    }
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumY + i == -1 && !m_destroyCheck[m_selectNumX, m_cubeHeigthSize - 1])
                {
                    m_selectNumY = m_cubeHeigthSize - 1;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
                else if (m_selectNumY + i > -m_cubeArray.GetLength(1) && !m_destroyCheck[m_selectNumX, m_selectNumY + i])
                {
                    m_selectNumY += i;
                    ColorChange(m_selectNumX, m_selectNumY);
                    Debug.Log(m_selectNumX + " " + m_selectNumY);
                    break;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(m_cubeArray[m_selectNumX, m_selectNumY]);
            m_destroyCheck[m_selectNumX, m_selectNumY] = true;

        }
    }
    public void ColorChange(int wnum, int hnum)
    {
        for (int i = 0; i < m_cubeArray.GetLength(0); i++)
        {
            for (int k = 0; k < m_cubeArray.GetLength(1); k++)
            {
                if (!m_destroyCheck[i, k])
                {
                    m_cubeArray[i, k].GetComponent<Renderer>().material.color = (i == wnum && k == hnum ? Color.red : Color.white);
                }
            }
            
        }
    }
}
