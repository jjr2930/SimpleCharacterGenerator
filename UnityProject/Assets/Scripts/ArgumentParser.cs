using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 커맨드 라인 인자를 파싱하는 클래스
/// 파싱할 인자
/// 1. -port : 포트번호 , 기본 값은 9090 , 대항 인자가 없으면 기본값을 리턴한다. 이를 수행하는 함수의 이름은 GetPort
/// </summary>
public static class ArgumentParser 
{
    public static int GetPort()
    {
        var args = System.Environment.GetCommandLineArgs();
        int port = 9090;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "-port")
            {
                if (i + 1 < args.Length)
                {
                    port = int.Parse(args[i + 1]);
                    break;
                }
            }
        }

        return port;
    }
}