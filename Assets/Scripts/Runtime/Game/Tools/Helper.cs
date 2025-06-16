using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.Game.Tools
{
    public class Helper
    {
        public static void Shuffle<T>(List<T> list) where T : class
        {
            var count = list.Count;
            for (int i = 0; i < count; i++)
            {
                var item = list[i];
                var randomIndex = UnityEngine.Random.Range(0, count);
                list[i] = list[randomIndex];
                list[randomIndex] = item;
            }
        }

        public static string FormatTime(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            return $"{span.Minutes:D2}:{span.Seconds:D2}";
        }

        public static string FormatTimeWithHours(float time)
        {
            TimeSpan span = TimeSpan.FromSeconds(time);
            return $"{span.Hours:D2}:{span.Minutes:D2}:{span.Seconds:D2}";
        }
        
        public static void Shuffle<T>(Stack<T> stack) where T : class
        {
            var list = stack.ToList();
            Shuffle(list);
            stack.Clear();
            foreach (var item in list)
                stack.Push(item);
        }

        public static WormHole IsPointerOverWormHole()
        {
            List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();

            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("WormHoleLayer"))
                    return curRaysastResult.gameObject.GetComponent<WormHole>();
            }
            return null;
        }
        
        public static bool IsPointerOverChickenBathing()
        {
            return IsPointerOverLayer("ChickenWashLayer");
        }

        public static bool IsPointerOverUIElement()
        {
            return IsPointerOverLayer("UI");
        }

        private static bool IsPointerOverLayer(string layerName)
        {
            List<RaycastResult> eventSystemRaysastResults = GetEventSystemRaycastResults();

            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer(layerName))
                    return true;
            }
            return false;
        }
        
        private static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}