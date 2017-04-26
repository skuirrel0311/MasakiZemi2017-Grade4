using System;
using System.Reflection;

//クライアントからのメッセージとサーバーのメソッドを紐付けます
public class RemoteCall
{
    //クライアントが呼びたいサーバーのメソッド名
    public string name { get; private set; }
    //引数がある場合は型名
    string model;

    /// <summary>
    /// メソッドを実行します
    /// </summary>
    public Action<byte[]> run;

    //全てのTaskで共通
    static BaseNetworkManager m_networkManager;
    static Type m_t;
    static Type t
    {
        get
        {
            if (m_t == null) m_t = m_networkManager.GetType();
            return m_t;
        }
        set
        {
            m_t = value;
        }
    }
    
    MethodInfo mi;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="networkManager"></param>
    /// <param name="model"></param>
    public RemoteCall(string name, BaseNetworkManager networkManager, string model = "")
    {
        this.name = name;
        m_networkManager = networkManager;
        MethodInfo mi = t.GetMethod(name);

        //初回のみrunに追加する
        run = (data) =>
        {
            if (model == "")
                mi.Invoke(m_networkManager, null);
            else
                mi.Invoke(m_networkManager, new object[] { encodeData(data, model) });
        };
    }

    /// <summary>
    /// byte[] からobjectに変換
    /// </summary>
    object encodeData(byte[] data, string model)
    {
        if (model == "int")
        {
            return BitConverter.ToInt32(data, 0);
        }
        if (model == "float")
        {
            return BitConverter.ToSingle(data, 0);
        }
        if (model == "bool")
        {
            return BitConverter.ToBoolean(data, 0);
        }
        if (model == "string")
        {
            return BitConverter.ToString(data);
        }
        if (model == "double")
        {
            return BitConverter.ToDouble(data, 0);
        }

        return default(object);
    }

}
