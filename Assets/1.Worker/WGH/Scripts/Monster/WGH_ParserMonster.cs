using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class WGH_ParserMonster : MonoBehaviour
{
    public string googleSheetID = "13iRdTgb36DHjiYSFFnCrDZdKu0DCo2iMDwsWUFpu7V8";
    private string sheetURL => $"https://docs.google.com/spreadsheets/d/13iRdTgb36DHjiYSFFnCrDZdKu0DCo2iMDwsWUFpu7V8/export?format=csv";

    public List<WGH_MonsterData> monsterDataList = new List<WGH_MonsterData>();

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
                int stage = int.Parse(tokens[0].Trim());
                string monType = tokens[1].Trim();
                bool isBoss = bool.Parse(tokens[2].Trim());
                string theme = tokens[3].Trim();

                var data = new WGH_MonsterData(stage, monType, isBoss, theme);
                monsterDataList.Add(data);
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"[{i + 1}줄] 파싱 에러: {ex.Message}");
            }
        }

        Debug.Log($"✅ 총 {monsterDataList.Count}개의 몬스터 데이터 로드 완료");
    }
}
