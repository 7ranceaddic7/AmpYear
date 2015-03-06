﻿/**
 * Utilities.cs
 *
 * AmpYear power management.
 * (C) Copyright 2015, Jamie Leighton
 * The original code and concept of AmpYear rights go to SodiumEyes on the Kerbal Space Program Forums, which was covered by GNU License GPL (no version stated).
 * As such this code continues to be covered by GNU GPL license.
 * Kerbal Space Program is Copyright (C) 2013 Squad. See http://kerbalspaceprogram.com/. This
 * project is in no way associated with nor endorsed by Squad.
 *
 *  This file is part of AmpYear.
 *
 *  AmpYear is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  AmpYear is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with AmpYear.  If not, see <http://www.gnu.org/licenses/>.
 *
 */

using KSP.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AY
{
    public static class Utilities
    {
        
        //Geometry and space

        public static double DistanceFromHomeWorld(Vessel vessel)
        {
            Vector3d vslPos = vessel.GetWorldPos3D();
            CelestialBody HmePlanet = Planetarium.fetch.Home;
            Log_Debug("AmpYear", "Home = " + HmePlanet.name + " Pos = " + HmePlanet.position.ToString());
            Log_Debug("AmpYear", "Vessel Pos = " + vslPos.ToString());
            Vector3d hmeplntPos = HmePlanet.position;
            double DstFrmHome = Math.Sqrt(Math.Pow((vslPos.x - hmeplntPos.x), 2) + Math.Pow((vslPos.y - hmeplntPos.y), 2) + Math.Pow((vslPos.z - hmeplntPos.z), 2));
            Log_Debug("AmpYear", "Distance from Home Planet = " + DstFrmHome);
            return DstFrmHome;
        }
        
        //Formatting time functions
        
        public static String formatTime(double seconds)
        {
            int y = (int)(seconds / (6.0 * 60.0 * 60.0 * 426.08));
            seconds = seconds % (6.0 * 60.0 * 60.0 * 426.08);
            int d = (int)(seconds / (6.0 * 60.0 * 60.0));
            seconds = seconds % (6.0 * 60.0 * 60.0);
            int h = (int)(seconds / (60.0 * 60.0));
            seconds = seconds % (60.0 * 60.0);
            int m = (int)(seconds / (60.0));
            seconds = seconds % (60.0);

            List<String> parts = new List<String>();

            if (y > 0)
            {
                parts.Add(String.Format("{0}:year ", y));
            }

            if (d > 0)
            {
                parts.Add(String.Format("{0}:days ", d));
            }

            if (h > 0)
            {
                parts.Add(String.Format("{0}:hours ", h));
            }

            if (m > 0)
            {
                parts.Add(String.Format("{0}:mins ", m));
            }

            if (seconds > 0)
            {
                parts.Add(String.Format("{0:00}:secs ", seconds));
            }

            if (parts.Count > 0)
            {
                return String.Join(" ", parts.ToArray());
            }
            else
            {
                return "0s";
            }
        }
        
        // Get Config Node Values out of a config node Methods

        public static bool GetNodeValue(ConfigNode confignode, string fieldname, bool defaultValue)
        {
            bool newValue;
            if (confignode.HasValue(fieldname) && bool.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static int GetNodeValue(ConfigNode confignode, string fieldname, int defaultValue)
        {
            int newValue;
            if (confignode.HasValue(fieldname) && int.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static float GetNodeValue(ConfigNode confignode, string fieldname, float defaultValue)
        {
            float newValue;
            if (confignode.HasValue(fieldname) && float.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static double GetNodeValue(ConfigNode confignode, string fieldname, double defaultValue)
        {
            double newValue;
            if (confignode.HasValue(fieldname) && double.TryParse(confignode.GetValue(fieldname), out newValue))
            {
                return newValue;
            }
            else
            {
                return defaultValue;
            }
        }

        public static string GetNodeValue(ConfigNode confignode, string fieldname, string defaultValue)
        {
            if (confignode.HasValue(fieldname))
            {
                return confignode.GetValue(fieldname);
            }
            else
            {
                return defaultValue;
            }
        }

        public static T GetNodeValue<T>(ConfigNode confignode, string fieldname, T defaultValue) where T : IComparable, IFormattable, IConvertible
        {
            if (confignode.HasValue(fieldname))
            {
                string stringValue = confignode.GetValue(fieldname);
                if (Enum.IsDefined(typeof(T), stringValue))
                {
                    return (T)Enum.Parse(typeof(T), stringValue);
                }
            }
            return defaultValue;
        }

        // GUI & Window Methods

        public static bool WindowVisibile(Rect winpos)
        {
            float minmargin = 20.0f; // 20 bytes margin for the window
            float xMin = minmargin - winpos.width;
            float xMax = Screen.width - minmargin;
            float yMin = minmargin - winpos.height;
            float yMax = Screen.height - minmargin;
            bool xRnge = (winpos.x > xMin) && (winpos.x < xMax);
            bool yRnge = (winpos.y > yMin) && (winpos.y < yMax);
            return xRnge && yRnge;
        }

        public static Rect MakeWindowVisible(Rect winpos)
        {
            float minmargin = 20.0f; // 20 bytes margin for the window
            float xMin = minmargin - winpos.width;
            float xMax = Screen.width - minmargin;
            float yMin = minmargin - winpos.height;
            float yMax = Screen.height - minmargin;

            winpos.x = Mathf.Clamp(winpos.x, xMin, xMax);
            winpos.y = Mathf.Clamp(winpos.y, yMin, yMax);

            return winpos;
        }

        public static double ShowTextField(string label, GUIStyle labelStyle, double currentValue, int maxLength, GUIStyle editStyle, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, labelStyle);
            GUILayout.FlexibleSpace();
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, editStyle, options);
            GUILayout.EndHorizontal();

            double newValue;
            if (double.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static double ShowTextField(double currentValue, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
            double newValue;
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, style, options);
            if (double.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static float ShowTextField(float currentValue, int maxLength, GUIStyle style, params GUILayoutOption[] options)
        {
            float newValue;
            string result = GUILayout.TextField(currentValue.ToString(), maxLength, style, options);
            if (float.TryParse(result, out newValue))
            {
                return newValue;
            }
            else
            {
                return currentValue;
            }
        }

        public static bool ShowToggle(string label, GUIStyle labelStyle, bool currentValue)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(label, labelStyle);
            GUILayout.FlexibleSpace();
            bool result = GUILayout.Toggle(currentValue, "");
            GUILayout.EndHorizontal();

            return result;
        }

        // Logging Functions
        // Name of the Assembly that is running this MonoBehaviour
        internal static String _AssemblyName
        { get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; } }

        public static void Log(this UnityEngine.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + obj.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log(this System.Object obj, string message)
        {
            Debug.Log(obj.GetType().FullName + "[" + obj.GetHashCode().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log(string context, string message)
        {
            Debug.Log(context + "[][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(this UnityEngine.Object obj, string message)
        {
            AYSettings AYsettings = AmpYear.Instance.AYsettings;
            if (AYsettings.debugging)
            Debug.Log(obj.GetType().FullName + "[" + obj.GetInstanceID().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(this System.Object obj, string message)
        {
            AYSettings AYsettings = AmpYear.Instance.AYsettings;
            if (AYsettings.debugging)
            Debug.Log(obj.GetType().FullName + "[" + obj.GetHashCode().ToString("X") + "][" + Time.time.ToString("0.00") + "]: " + message);
        }

        public static void Log_Debug(string context, string message)
        {
            AYSettings AYsettings = AmpYear.Instance.AYsettings;
            if (AYsettings.debugging)
                Debug.Log(context + "[][" + Time.time.ToString("0.00") + "]: " + message);
        }        
    }
}