using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Youle.Mobile.Infrastructure.AppStore.Models
{
    public enum ElementType
    {
        多选列表 = 1,
        单选按钮,
        输入框,
        多行输入框
    }

    public enum IsValid
    {
        有效 = 1,
        无效 = 0
    }

    public enum IsTrue
    {
        是 = 1,
        否 = 0
    }

    public enum UpdateType
    {
        手动更新 = 0,
        强制更新 = 1,
        后台更新 = 2,
        后台更新并运行 = 3
    }

    public enum CategoryForApp
    {
        游戏 = 1,
        应用,
        平台,
        链接,
        方法,
        Java游戏,
        书籍

    }
    public enum MobileParamKeys
    {
        empty = 0,
        imei,
        smsc,
        batch,
        pht,
        dh,
        Pf,
        mpm,
        mod,
        lbyver,
        tm,
        lcd,
        mcode,
        SIM,
        tcard,
        touch,
        kb,
        gs,
        cap,
        os,
        nt,
        java,
        c,
        lua,
        lbs,
        ud,
    }

    public enum MobileParamValues
    {
        不选 = 0,
        IMEI号_imei,
        短信中心号码_smsc,
        芯片类型_batch,
        客户端类型_pht,
        设计公司_dh,
        生产厂商_Pf,
        品牌型号_mpm,
        硬件版本_mod,
        大厅版本_lbyver,
        出厂日期_tm,
        分辨率_lcd,
        码机_mcode,
        第几张SIM卡_SIM,
        是否有T卡_tcard,
        是否带触屏_touch,
        是否带键盘_kb,
        是否带重力传感_gs,
        是否带电容屏_cap,
        客户端操作系统_OS,
        /// <summary>
        /// wifi, 3g, 2g
        /// </summary>
        网络连接类型_nt,
        java信息_java,
        C信息_c,
        lua信息_lua,
        地理位置_lbs,
        客户端编译版本_ud,

    }

    public enum OTAConfigValues
    {
        不选 = 0,
        是否桌面弹出消息提示用户有OTA更新_IsShowOTAInfo,
        OTA更新提示内容_OTAUpdateContent,
        下次OTA检查更新时间间隔秒数_NextCheckTime,
        是否自动下载_IsAutoDownload,
    }

    public enum OTAConfigKeys
    {
        empty = 0,
        IsShowOTAInfo,
        OTAUpdateContent,
        NextCheckTime,
        IsAutoDownload,
    }
}
