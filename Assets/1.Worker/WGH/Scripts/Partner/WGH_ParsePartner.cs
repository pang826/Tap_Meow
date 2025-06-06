using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WGH_ParsePartner : MonoBehaviour
{
    public string googleSheetID = "1BkeAr1C6uuQd42ow2jBS78k44cBH29HlI7MEkAZ2sGg";
    private string sheetURL => $"https://docs.google.com/spreadsheets/d/1BkeAr1C6uuQd42ow2jBS78k44cBH29HlI7MEkAZ2sGg/export?format=csv";

    public List<WGH_PartnerData> partnerDataList = new List<WGH_PartnerData>();

    private void Start()
    {
        StartCoroutine(DownloadCSV());
    }

    IEnumerator DownloadCSV()
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetURL);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"CSV 다운로드 실패: {www.error}");
        }
        else
        {
            ParseCSV(www.downloadHandler.text);
        }
    }

    void ParseCSV(string csv)
    {
        string[] lines = csv.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 첫 줄은 헤더
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] tokens = line.Split(',');

            if (tokens.Length < 4) continue;

            try
            {
                int num = int.Parse(tokens[0].Trim());
                string partnerName = tokens[1].Trim();
                float partnerDmg = float.Parse(tokens[2].Trim());
                float partnerAttackSpeed = float.Parse(tokens[3].Trim());
                long cost = long.Parse(tokens[4].Trim());
                Debug.Log($" 넘버{num}, 이름{partnerName}, 데미지{partnerDmg}, 공속{partnerAttackSpeed}, 비용{cost}");
                var data = new WGH_PartnerData(num, partnerName, partnerDmg, partnerAttackSpeed, cost);
                partnerDataList.Add(data);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[{i + 1}줄] 파싱 에러: {ex.Message}");
            }
        }

        Debug.Log($"✅ 총 {partnerDataList.Count}개의 파트너 데이터 로드 완료");
    }
}
