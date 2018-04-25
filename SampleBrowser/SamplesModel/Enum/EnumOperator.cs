using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamplesModel.Enum
{
    public class EnumOperator
    {
        /// <summary>
        /// Translate language string to Language enum value
        /// </summary>
        /// <param name="languageString"></param>
        /// <returns></returns>
        public static Language GetLanguageEnumValue(string languageString)
        {
            try
            {
                switch (languageString.ToUpper())
                {
                    case "C++":
                        return Language.Cpp;
                    case "C#":
                        return Language.CSharp;
                    case "F#":
                        return Language.FSharp;
                    case "VB":
                        return Language.VisualBasic;
                    default:
                        return (Language)System.Enum.Parse(typeof(Language), languageString, true);
                }
            }
            catch
            {
                return Language.Others;
            }
        }

        /// <summary>
        /// Translate Language enum value to string
        /// </summary>
        /// <param name="languageEnumValue"></param>
        /// <returns></returns>
        public static string GetLanguageStringValue(Language languageEnumValue)
        {
            switch (languageEnumValue)
            {
                case Language.Cpp:
                    return "C++";
                case Language.CSharp:
                    return "C#";
                case Language.FSharp:
                    return "F#";
                case Language.VisualBasic:
                    return "VB";
                default:
                    return languageEnumValue.ToString();
            }
        }

        /// <summary>
        /// Translate technology string to Technology enum value
        /// </summary>
        /// <param name="technologyString"></param>
        /// <returns></returns>
        public static Technology GetTechnologyEnumValue(string technologyString)
        {
            try
            {
                switch (technologyString.ToLower())
                {
                    case "asp.net":
                        return Technology.ASPNET;
                    case "windows forms":
                        return Technology.WindowsForms;
                    case "windows 7":
                        return Technology.Windows7;
                    case "data platform":
                        return Technology.DataPlatform;
                    case "windows ui":
                        return Technology.WindowsUI;
                    case "windows shell":
                        return Technology.WindowsShell;
                    case "ipc and rpc":
                        return Technology.IPCandRPC;
                    case "file system":
                        return Technology.FileSystem;
                    case "windows service":
                        return Technology.WindowsService;
                    case "windows phone":
                        return Technology.WindowsPhone;
                    case "visual studio":
                        return Technology.VisualStudio;
                    case "sql server":
                        return Technology.SQLServer;
                    case "windows general":
                        return Technology.WindowsGeneral;
                    case "windows security":
                        return Technology.WindowsSecurity;
                    default:
                        return (Technology)System.Enum.Parse(typeof(Technology), technologyString, true);
                }
            }
            catch
            {
                return Technology.Others;
            }
        }

        /// <summary>
        /// Translate Technology enum value to technology string
        /// </summary>
        /// <param name="technologyEnumValue"></param>
        /// <returns></returns>
        public static string GetTechnologyStringValue(Technology technologyEnumValue)
        {
            switch (technologyEnumValue)
            {
                case Technology.ASPNET:
                    return "ASP.NET";
                case Technology.WindowsForms:
                    return "Windows Forms";
                case Technology.Windows7:
                    return "Windows 7";
                case Technology.DataPlatform:
                    return "Data Platform";
                case Technology.WindowsUI:
                    return "Windows UI";
                case Technology.WindowsShell:
                    return "Windows Shell";
                case Technology.IPCandRPC:
                    return "IPC and RPC";
                case Technology.FileSystem:
                    return "File System";
                case Technology.WindowsService:
                    return "Windows Service";
                case Technology.WindowsPhone:
                    return "Windows Phone";
                case Technology.VisualStudio:
                    return "Visual Studio";
                case Technology.SQLServer:
                    return "SQL Server";
                case Technology.WindowsGeneral:
                    return "Windows General";
                case Technology.WindowsSecurity:
                    return "Windows Security";
                default:
                    return technologyEnumValue.ToString();
            }
        }

        /// <summary>
        /// Validate whether the parameter can be parse to enum
        /// </summary>
        /// <param name="enumType">Try to parse's enum Type, eg: typeof(Language)</param>
        /// <param name="param">The value want to be parsed</param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static bool CanParseToEnum(Type enumType, string param, bool ignoreCase)
        {
            try
            {
                System.Enum.Parse(enumType, param, ignoreCase);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
