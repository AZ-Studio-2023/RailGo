namespace RailGo.Core.Models;

// 下载进度
public class DownloadProgress
{
    public string Status
    {
        get; set;
    }
    public int Percentage
    {
        get; set;
    }
}

// 数据库信息
public class DatabaseInfo
{
    public string Path
    {
        get; set;
    }
    public long FileSize
    {
        get; set;
    }
    public System.DateTime LastModified
    {
        get; set;
    }
    public bool Exists
    {
        get; set;
    }
}