using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public static class LocalizationManager
{
    public static string Language { get; set; } = "English";  

    private static Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>()
    {
        { "English", new Dictionary<string, string>()
            {
                { "Start", "Click On Start" },
                { "Lobby.Mission", "MISSION" },
                { "Lobby.Setting", "SETTING" },
                { "Lobby.Sound", "Sound" },
                { "Lobby.Music", "Music" },
                { "Lobby.Language", "Language" },
                { "Lobby.Control", "Control" },
                { "Lobby.CameraSpeed", "Camera Speed" },
                { "Lobby.Run", "Run" },
                { "Lobby.UseBow", "Use Bow" },
                { "Lobby.UseSword", "Use Sword" },
                { "Lobby.Jump", "Jump" },
                { "Lobby.Attack", "Attack" },
                { "Lobby.Blocking", "Blocking" },
                { "Quest.One", "Quest 1 Kill: 5 Goat or Sheep" },
                { "Quest.Two", "Quest 2 Kill: 10 Goat or Sheep" },
                { "Quest.Three", "Quest 3 Kill: 40 Goat or Sheep" },

            }
        },
        { "Vietnamese", new Dictionary<string, string>()
            {
                { "Start", "Nhấn để bắt đầu" },
                { "Lobby.Mission", "NHIỆM VỤ" },
                { "Lobby.Setting", "Cài Đặt" },
                { "Lobby.Sound", "Âm thanh" },
                { "Lobby.Music", "Nhạc" },
                { "Lobby.Language", "Ngôn ngữ" },
                { "Lobby.Control", "Điều khiển" },
                { "Lobby.CameraSpeed", "Tốc độ Camera" },
                { "Lobby.Run", "Chạy" },
                { "Lobby.UseBow", "Cung" },
                { "Lobby.UseSword", "Kiếm" },
                { "Lobby.Jump", "Nhảy" },
                { "Lobby.Attack", "Tấn công" },
                { "Lobby.Blocking", "Chặn đòn" },
                { "Quest.One", "Nhiệm vụ 1: Giết 5 con Dê hoặc Cừu" },
                { "Quest.Two", "Nhiệm vụ 2: Giết 10 con Dê hoặc Cừu" },
                { "Quest.Three", "Nhiệm vụ 3: Giết 40 con Dê hoặc Cừu" },
            }
        }
    };

    public static string GetLocalizedText(string key)
    {
        if (translations.ContainsKey(Language) && translations[Language].ContainsKey(key))
        {
            return translations[Language][key];
        }
        return key; // Nếu không tìm thấy, trả về key
    }
}