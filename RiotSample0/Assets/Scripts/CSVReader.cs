using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class CSVReader
{
    static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
    static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";//줄바꾸기
    static char[] TRIM_CHARS = { '\"' };

    public static List<Dictionary<string, object>> Read(string file)//빈칸이 있으면 에러가 남으로 null값이라도 넣어줘라
    {

        var list = new List<Dictionary<string, object>>();
        TextAsset data = Resources.Load(file) as TextAsset;//파일 안에 있는 것을 유니티에서 읽기 위해 텍스트에셋 형식으로 바꿔준다


        var lines = Regex.Split(data.text, LINE_SPLIT_RE);//regex는  csv의 줄을 나눠주는 역할

        if (lines.Length <= 1) return list;//목록이 없으면 바로 리턴
        
        var header = Regex.Split(lines[0], SPLIT_RE);//헤더역할

        for (var i = 1; i < lines.Length; i++)//열 탐색
        {
            var values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            var entry = new Dictionary<string, object>();
            for (var j = 0; j < header.Length && j < values.Length; j++)//행 탐색
            {
                string value = values[j];
                value = value.Trim(TRIM_CHARS).Replace("\\", "");//TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", ""); 괄호안에 문자를 찾아서 제거 앞쪽에서 지우고 뒤에서 지우고
                value = value.Replace("</ br>", "\n");//괄호 안 앞에 string을 뒤에 것으로 대체
                value = value.Replace("</ comma>", ",");
                object finalvalue = value;
                int n;
                float f;
                //나중에 형변환을 이용한 tryparse
                if (int.TryParse(value, out n))
                {
                    finalvalue = n;
                }
                else if (float.TryParse(value, out f))
                {
                    finalvalue = f;
                }
                entry[header[j]] = finalvalue;//키에 값을 넣는다
            }
            list.Add(entry);//리스트에 추가한다
            
        }
        return list;
    }
}