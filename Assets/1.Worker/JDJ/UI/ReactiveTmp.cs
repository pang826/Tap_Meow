using TMPro;
using UnityEngine;

public class ReactiveTmp : MonoBehaviour, IRreceiver
{
    [SerializeField] private E_ReactiveType _type;
    private TMP_Text _tmp;

    private void Start()
    {
        ReactiveContainer.RegistReceiver(_type, this);
        _tmp = GetComponent<TMP_Text>();
    }

    public void SendValue(object m_value)
    {
        _tmp.text = (string)m_value;
    }
}
