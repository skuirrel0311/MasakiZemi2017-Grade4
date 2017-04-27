using System;
using System.Reflection;
using UnityEngine;

//クライアントからのメッセージとサーバーのメソッドを紐付けます
public class RemoteCall
{
    //クライアントが呼びたいサーバーのメソッド名
    public string name { get; private set; }
    //引数がある場合は型名
    string type;

    /// <summary>
    /// メソッドを実行します（同じメソッドを呼ぶ場合は使いまわす）
    /// </summary>
    public Action<byte[]> run;

    //全てのTaskで共通
    static object m_networkManager;
    static Type networkManagetType;
    
    MethodInfo mi;
    
    public static void Initialize(MyNetworkServer networkManager)
    {
        m_networkManager = networkManager;
        networkManagetType = m_networkManager.GetType();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="name">サーバー側で呼ばれるメソッドの名前</param>
    /// <param name="type">メソッド引数の型名</param>
    public RemoteCall(string name,  string type)
    {
        this.name = name;
        this.type = type;
        
        if(m_networkManager == null)
        {
            Debug.LogError("RemoteCall is no initialize");
            return;
        }
        
        mi = networkManagetType.GetMethod(name);

        //初回のみrunに追加する
        run = (data) =>
        {
            if (type == "")
                mi.Invoke(m_networkManager, null);
            else
                mi.Invoke(m_networkManager, new object[] { decodeData(data, type) });
        };
    }

    /// <summary>
    /// byte[] からobjectに変換
    /// </summary>
    object decodeData(byte[] data, string type)
    {
        if (type == "int")
        {
            return BitConverter.ToInt32(data, 0);
        }
        if (type == "float")
        {
            return BitConverter.ToSingle(data, 0);
        }
        if (type == "bool")
        {
            return BitConverter.ToBoolean(data, 0);
        }
        if (type == "string")
        {
            return System.Text.Encoding.Unicode.GetString(data, 0, data.Length);
        }
        if (type == "double")
        {
            return BitConverter.ToDouble(data, 0);
        }

        return default(object);
    }

}
