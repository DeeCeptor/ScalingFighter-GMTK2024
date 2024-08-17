using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using System.IO;
using UnityEditor;

public static class ExtensionMethods
{
    public static Vector2 North = Vector2.up;
    public static Vector2 East = Vector2.right;
    public static Vector2 South = Vector2.down;
    public static Vector2 West = Vector2.left;
    public static Vector2 NorthWest = new Vector2(-1, 1);
    public static Vector2 NorthEast = new Vector2(1, 1);
    public static Vector2 SouthWest = new Vector2(-1, -1);
    public static Vector2 SouthEast = new Vector2(1, -1);
    public static Vector2 NorthWest_norm = new Vector2(-1, 1).normalized;
    public static Vector2 NorthEast_norm = new Vector2(1, 1).normalized;
    public static Vector2 SouthWest_norm = new Vector2(-1, -1).normalized;
    public static Vector2 SouthEast_norm = new Vector2(1, -1).normalized;

    public static List<Vector2> NormalizedDirections = new List<Vector2> { North, East, South, West, NorthWest_norm, NorthEast_norm, SouthEast_norm, SouthWest_norm };

    public static Dictionary<Vector2, string> VectorToDirection = new Dictionary<Vector2, string>
        { { West, "West" }, { NorthWest, "North West" }, { North, "North" }, { NorthEast, "North East" },
        { East, "East" }, { SouthEast, "South East" }, { South, "South" }, { SouthWest, "South West" } };
    public static Dictionary<string, Vector2> DirectionToVector = new Dictionary<string, Vector2> (StringComparer.InvariantCultureIgnoreCase)
        { { "West", West }, { "North West", NorthWest }, { "NorthWest", NorthWest }, { "North", North }, { "North East", NorthEast }, { "NorthEast", NorthEast },
        { "East", East }, { "South East", SouthEast }, { "SouthEast", SouthEast }, { "South", South }, { "South West", SouthWest }, { "SouthWest", SouthWest } };
    // Ordered so compound/diagonal directions appear before cardinal directions
    public static List<string> Directions_upper = new List<string> { "NORTH WEST", "SOUTH WEST", "NORTH EAST", "SOUTH EAST", "EAST", "NORTH", "SOUTH", "WEST"  };
    public static List<string> Directions_lower = new List<string> { "north west", "south west", "north east", "south east", "east", "north", "south", "west" };
    //public static List<string> Directions_lower = new List<string> { "north west", "northwest", "south west", "southwest", "north east", "northeast", "south east", "southeast", "east", "north", "south", "west" };
    public static Dictionary<char, string> letter_to_NATO = new Dictionary<char, string> { 
        { 'A', "Alfa" }
        ,{ 'B', "Bravo" }
        ,{ 'C', "Charlie" }
        ,{ 'D', "Delta" }
        ,{ 'E', "Echo" }
        ,{ 'F', "Foxtrot" }
        ,{ 'G', "Golf" }
        ,{ 'H', "Hotel" }
        ,{ 'I', "India" }
        ,{ 'J', "Juliett" }
        ,{ 'K', "Kilo" }
        ,{ 'L', "Lima" }
        ,{ 'M', "Mike" }
        ,{ 'N', "November" }
        ,{ 'O', "Oscar" }
        ,{ 'P', "Papa" }
        ,{ 'Q', "Quebec" }
        ,{ 'R', "Romeo" }
        ,{ 'S', "Sierra" }
        ,{ 'T', "Tango" }
        ,{ 'U', "Uniform" }
        ,{ 'V', "Victor" }
        ,{ 'W', "Whiskey" }
        ,{ 'X', "X-ray" }
        ,{ 'Y', "Yankee" }
        ,{ 'Z', "Zulu" }
    };
        public static Dictionary<string , char> NATO_to_letter = new Dictionary<string, char> { 
        { "Alfa", 'A' }
        ,{ "Bravo", 'B' }
        ,{ "Charlie", 'C'}
        ,{  "Delta", 'D'}
        ,{  "Echo", 'E'}
        ,{  "Foxtrot", 'F'}
        ,{  "Golf", 'G'}
        ,{  "Hotel", 'H'}
        ,{  "India", 'I'}
        ,{  "Juliett", 'J'}
        ,{  "Kilo", 'K'}
        ,{  "Lima", 'L'}
        ,{  "Mike", 'M'}
        ,{  "November", 'N'}
        ,{  "Oscar", 'O'}
        ,{  "Papa", 'P'}
        ,{  "Quebec", 'Q' }
        ,{  "Romeo", 'R'}
        ,{  "Sierra", 'S'}
        ,{  "Tango", 'T'}
        ,{  "Uniform", 'U'}
        ,{  "Victor", 'V'}
        ,{  "Whiskey", 'W'}
        ,{  "X-ray", 'X'}
        ,{  "Yankee", 'Y'}
        ,{  "Zulu", 'Z' }
    };

    public static string location_colour = "#004A80";


    // Returns the string direction from a given 2D vector2 (must be exact)
    public static string GetDirection(this Vector2 vector)
    {
        return VectorToDirection[vector];
    }
    // Returns the closest direction to given vector. Ex: north, southeast, etc.
    public static string GeneralizeDirection(Vector2 vector)
    {
        KeyValuePair<Vector2, string> pair = GetNearestDirection(vector);
        return pair.Value;
    }
    public static Vector2 GetClosestCardinalDirection(Vector2 dir)
    {
        return GetNearestDirection(dir).Key;
    }
    public static KeyValuePair<Vector2, string> GetNearestDirection(Vector2 dir)
    {
        Vector2 normalized_vec = dir.normalized;
        // Loop through each of the 8 possible directions, keeping track of the closest, distancewise
        float closest_dist = 999;
        Vector2 closest_dir = Vector2.left;
        KeyValuePair<Vector2, string> closest_pair = new KeyValuePair<Vector2, string>();
        foreach (KeyValuePair<Vector2, string> pair in VectorToDirection)
        {
            float dist = Vector2.Distance(pair.Key, normalized_vec);
            if (dist < closest_dist)
            {
                closest_dist = dist;
                closest_pair = pair;
            }
        }
        return closest_pair;
    }

    // Returns the closest position from a list of positions to a starting point, as well as the measured distance between starting point and closest point
    public static KeyValuePair<Vector2, float> GetNearestPosition(Vector2 starting_pos, List<Vector2> potential_nearest)
    {
        // Loop through each possible direction, keeping track of the closest, distancewise
        float closest_dist = 999;
        Vector2 closest_pos = potential_nearest[0];
        foreach (Vector2 pos in potential_nearest)
        {
            float dist = Vector2.Distance(starting_pos, pos);
            if (dist < closest_dist)
            {
                closest_dist = dist;
                closest_pos = pos;
            }
        }
        return new KeyValuePair<Vector2, float>(closest_pos, closest_dist);
    }


    // Returns a vector that is exactly X_distance away from the target, in the direction of start position
    public static Vector2 MoveToWithin(Vector2 starting_pos, Vector2 destination, float X_distance)
    {
        // Get direction from here to there
        Vector2 direction_to_move = (destination - starting_pos).normalized;
        // Get distance between here and there
        float distance = Vector2.Distance(destination, starting_pos);
        // Figure out how far we need to move to be exactly X away
        float distance_to_move = (distance - X_distance);
        Vector2 return_pos = starting_pos + direction_to_move * distance_to_move;
        return return_pos;
    }

    public static IEnumerator RebuildLayoutCo(RectTransform t)
    {
        RebuildLayoutImmediate(t);
        Transform first_child = null;
        if (t.transform.childCount > 0)
        {
            first_child = t.GetChild(0);
            first_child.SetAsLastSibling();
        }
        yield return null;
        RebuildLayoutImmediate(t);
        if (first_child != null)
        {
            first_child.SetAsFirstSibling();
        }
    }
    public static void RebuildLayoutImmediate(RectTransform t)
    {
        if (t != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(t); 
    }


    public static string CleanForCoroutine(this string in_string)
    {
        in_string = in_string.Replace(" ", "");
        //in_string = in_string.Replace("!", "");
        in_string = new string(in_string.Where(c => !char.IsPunctuation(c)).ToArray()); // Remove all punctuation

        // Remove all punctuation?
        return in_string;
    }
    /// <summary>
    /// Destroys all CHILDREN objects of this transform
    /// </summary>
    public static Transform Clear(this Transform transform, List<Transform> exceptions, bool destroy_inactive=true) // Should destroy inactive
    {
        foreach (Transform child in transform)
        {
            if (!exceptions.Contains(child) && (destroy_inactive || child.gameObject.activeSelf))
                GameObject.Destroy(child.gameObject);
        }
        return transform;
    }
    /// <summary>
    /// Destroys all CHILDREN objects of this transform
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="destroy_inactive"></param>
    /// <returns></returns>
    public static Transform Clear(this Transform transform, bool destroy_inactive=true, bool destroy_immediate=false) // Should destroy inactive
    {
        foreach (Transform child in transform)
        {
            if (destroy_inactive || child.gameObject.activeSelf)
            {
                if (destroy_immediate)
                {
                    GameObject.DestroyImmediate(child.gameObject);

                }
                else
                {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        return transform;
    }

    public static void Shuffle<T>(this IList<T> list)  
    {  
        var rand = new System.Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rand.Next(n+1);
            //int k = UnityEngine.Random.Range(0, n+1);
            //int k = rng.Next(n + 1);      // Seems to be giving same results every time
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }

    /// <summary>
    /// Returns random item from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T Random<T>(this IList<T> list, bool remove_after=false)  
    {
        if ((list == null) || (list.Count < 1)) return default(T);
        T return_val = list[UnityEngine.Random.Range(0, list.Count)];
        if (remove_after)
        {
            list.Remove(return_val);
        }
        return return_val;
    }
    /// <summary>
    /// Returns random items from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<T> Random<T>(this IList<T> list, int count, bool remove_from_original_list=false)
    {
        List<T> return_list = new List<T>();
        if (list == null || count <= 0)
            return return_list;
        List<T> search_list = new List<T>(list);
        if (remove_from_original_list)
            search_list = (List<T>)list;
        int remaining_draws = count;
        while (remaining_draws > 0 && search_list.Count > 0)
        {
            T return_val = search_list[UnityEngine.Random.Range(0, search_list.Count)];
            return_list.Add(return_val);
            remaining_draws--;
            // Remove item after so we can't accidentally draw copies
            search_list.Remove(return_val);
        }
        return return_list;
    }
    /// <summary>
    /// Returns and deletes random item from list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveReturnRandom<T>(this IList<T> list)
    {
        if ((list == null) || (list.Count < 1)) return default(T);
        int index = UnityEngine.Random.Range(0, list.Count);
        T value = list[index];
        list.RemoveAt(index);
        return value;
    }
    public static List<T> ReturnXDistinctItems<T>(this IList<T> list, int num_draws, IList<T> potential_additions)
    {
        List<T> available_items_to_draw = new List<T>();
        foreach (T potential in potential_additions)
        {
            // Check if we own the item
            if (!list.Contains(potential))
            {
                // We don't already own item, add it as a potential draw
                available_items_to_draw.Add(potential);
            }
        }
        // Draw X items, NO DUPLICATES!
        int remaining_draws = num_draws;
        List<T> returned_items = new List<T>();
        while (remaining_draws > 0 && available_items_to_draw.Count > 0)
        {
            T added_item = available_items_to_draw.RemoveReturnRandom();
            returned_items.Add(added_item);
            remaining_draws--;
            //Debug.Log("DrawAddXDistinctItems added " + added_item);
        }
        if (remaining_draws > 0)
        {
            //Debug.LogError("ReturnXDistinctItems ran out of unique items to draw. Remaining draws " + remaining_draws);
        }
        return returned_items;
    }


    /// <summary>
    /// Splits STRING into LIST. If empty, empty list is returned
    /// </summary>
    /// <param name="source"></param>
    /// <param name="list_split_char"></param>
    /// <returns></returns>
    public static List<string> ParseStringList(this string source, char list_split_char = '|')
    {
        if (!string.IsNullOrEmpty(source))
        {
            return source.Split(list_split_char).ToList<string>();
        }
        return new List<string>();
    }
    public static string CleanForCoroutineName(this string source)
    {
        string s = source;
        s = s.Replace(" ", "");
        s = s.Replace("\n", "");
        s = s.Replace("\r", "");
        return s;
    }


    /// <summary>
    /// Add EVERY sprite to appear in text here. In assets window > create > textmeshpro > sprite asset WHILE texture is selected (texture set to multiple, sprites split & named)
    /// </summary>
    public static List<string> TextMeshSpriteList = new List<string>
    {
        "WaterDmg", "ArcaneDmg", "FireDmg", "EarthDmg", "AirDmg", "BlackDmg", "WhiteDmg"
        //, "Water", "Arcane", "Fire", "Water", "Air", "Black", "White" // Could cause problems
    };
    public const string TextMeshSpriteTag = " <sprite name=\"" + TextMeshSpriteTag_ReplaceThis + "\">"; // Added extra space BEFORE sprite to give more room
    public const string TextMeshSpriteTag_ReplaceThis = "REPLACE_THIS";
    public static string InsertTextMeshSprites(this string source)
    {
        string s = source;
        foreach (string sprite_name_to_insert in TextMeshSpriteList)
        {
            string tag_to_replace = TextMeshSpriteTag.Replace(TextMeshSpriteTag_ReplaceThis, sprite_name_to_insert);
            s = s.Replace(sprite_name_to_insert, tag_to_replace);
            // Remove extra " " AFTER replaced part if necessary
            s = s.Replace(tag_to_replace + " ", tag_to_replace);
        }
        return s;
    }


    public static string Boldify(string msg)
    {
        return "<b>" + msg + "</b>";
    }
    public static string Italify(string msg)
    {
        return "<i>" + msg + "</i>";
    }
    public static string Underline(string msg)
    {
        return "<u>" + msg + "</u>";
    }
    public static string Colourify(string color, string msg)
    {
        return "<color=" + color + ">" + msg + "</color>";
    }

    // https://www.hexcolortool.com/#154715
    // colours are html hex colours #rrggbbaa
    // add ff to end of colour for alpha values
    public const string dark_green = "#154715ff";


    public static void SendAllowDraggingMessage(GameObject recipient, bool allow)
    {
        recipient.SendMessage("AllowDragging", allow, SendMessageOptions.DontRequireReceiver);
    }


    // Returns how many ACTIVE children this transform has
    public static int NumActiveChildren(Transform t)
    {
        int count = 0;
		foreach(Transform child in t)
		{
			if(child.gameObject.activeSelf)
				count++;
		}
        return count;
    }


    public static void SetLayerRecursively(GameObject go, int layerNumber)
    {
        foreach (Transform trans in go.GetComponentsInChildren<Transform>(true))
        {
            trans.gameObject.layer = layerNumber;
        }
    }


    static float PercentArrowHeadSize = 0.1f;
    public static void SetLineRendererArrowShape(LineRenderer lr, Vector3 start_pos,  Vector3 end_pos)
    {
        float AdaptiveSize = (float)(PercentArrowHeadSize / Vector3.Distance(start_pos, end_pos));

		Vector3[] positions = {start_pos
				, Vector3.Lerp(start_pos, end_pos, 0.999f - AdaptiveSize)
				, Vector3.Lerp(start_pos, end_pos, 1f - AdaptiveSize)
				, end_pos };
		lr.positionCount = positions.Length;

		lr.widthCurve = new AnimationCurve(
			new Keyframe(0, .4f)
			, new Keyframe(0.999f - AdaptiveSize, .4f)  // neck of arrow
			, new Keyframe(1f - AdaptiveSize, 1f)  // max width of arrow head
			, new Keyframe(1f, 0f));  // tip of arrow
		lr.SetPositions(positions);
    }



    public static void DestroyThisGameObjectList(this List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            MonoBehaviour.Destroy(obj);
        }
    }



    public static void ToggleActive(this GameObject obj)
    {
        obj.SetActive(!obj.activeSelf);
    }


    public static string PrintList<T>(List<T> l, bool use_newlines, bool debug_log=true)
    {
        string msg = "";
        string end = "";
        if (use_newlines)
            end = "\n";
        foreach (T o in l)
        {
            msg += o.ToString() + end;
        }
        if (debug_log)
            Debug.Log(msg);
        return msg;
    }
        public static string PrintList<T>(T[] l, bool use_newlines, bool debug_log=true)
    {
        string msg = "";
        string end = "";
        if (use_newlines)
            end = "\n";
        foreach (T o in l)
        {
            msg += o.ToString() + end;
        }
        if (string.IsNullOrEmpty(msg))
            return msg;
        if (debug_log)
            Debug.Log(msg);
        return msg;
    }




    public static bool GetPlayerPrefBool(string key)
    {
        return System.Convert.ToBoolean(PlayerPrefs.GetInt(key, 0));
    }
    public static bool GetPlayerPrefBool(string key, bool default_value)
    {
        return System.Convert.ToBoolean(PlayerPrefs.GetInt(key,
            System.Convert.ToInt32(default_value)));
    }
    public static void SetPlayerPrefBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, System.Convert.ToInt32(value));
        PlayerPrefs.Save();
    }


    /// <summary>
    /// Writes the given object instance to a binary file.
    /// <para>Object type (and all child types) must be decorated with the [Serializable] attribute.</para>
    /// <para>To prevent a variable from being serialized, decorate it with the [NonSerialized] attribute; cannot be applied to properties.</para>
    /// </summary>
    /// <typeparam name="T">The type of object being written to the binary file.</typeparam>
    /// <param name="filePath">The file path to write the object instance to.</param>
    /// <param name="objectToWrite">The object instance to write to the binary file.</param>
    /// <param name="append">If false the file will be overwritten if it already exists. If true the contents will be appended to the file.</param>
    public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
    {
        using (Stream stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            binaryFormatter.Serialize(stream, objectToWrite);
        }
    }

    /// <summary>
    /// Reads an object instance from a binary file.
    /// </summary>
    /// <typeparam name="T">The type of object to read from the binary file.</typeparam>
    /// <param name="filePath">The file path to read the object instance from.</param>
    /// <returns>Returns a new instance of the object read from the binary file.</returns>
    public static T ReadFromBinaryFile<T>(string filePath)
    {
        using (Stream stream = File.Open(filePath, FileMode.Open))
        {
            var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            return (T)binaryFormatter.Deserialize(stream);
        }
    }
    public static T LoadFileFromBinary<T>(FileInfo f)
    {
        T loaded_file = default(T);
        if (f.Exists)
        {
            //Debug.Log("Loading saved replay file: " + f.Name);
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            FileStream file = f.Open(FileMode.Open);
            try
            {
                loaded_file = (T)bf.Deserialize(file);
                //Debug.Log("Loaded replay " + full_file_path);
                //Debug.Log(loaded_replay + "");
            }
            catch (Exception e)
            {
                Debug.LogError("Exception loading file: " + f.Directory + "." + e);
            }
            file.Close();
        }
        else
            Debug.Log("Could not find save file: " + f.FullName);
        return loaded_file;
    }


    public static Vector3 SmoothStepVector3(Vector3 from, Vector3 to, float progress)
    {
        return Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, Mathf.SmoothStep(0f, 1f, progress)));
        /* old
        return new Vector3(
					Mathf.Smooth(Mathf.SmoothStep(from.x, to.x, progress)),
					Mathf.SmoothStep(from.y, to.y, progress),
					Mathf.SmoothStep(from.z, to.z, progress)
				);
            */
    }


    // DON'T ADD "/TEXT/" to path - it adds them automatically
    public static string PrepareFilePath(string path_1, string path_2)
    {
        string p1 = path_1;
        string p2 = path_2;
        // Make our path separators platform specific
        /*
        p1 = p1.Replace('\\', Path.DirectorySeparatorChar);
        p1 = p1.Replace('/', Path.DirectorySeparatorChar);
        p2 = p2.Replace('\\', Path.DirectorySeparatorChar);
        p2 = p2.Replace('/', Path.DirectorySeparatorChar);
        */

        // Combine them properly
        string new_path = Path.Combine(p1, p2);
        //Debug.Log("OG: " + path_1 + " " + path_2 + " new: " + new_path + " system separator: " + Path.DirectorySeparatorChar);
        return new_path;
    }
        public static string PrepareFilePath(string[] paths)
    {
        // Make our path separators platform specific
        /*
        p1 = p1.Replace('\\', Path.DirectorySeparatorChar);
        p1 = p1.Replace('/', Path.DirectorySeparatorChar);
        p2 = p2.Replace('\\', Path.DirectorySeparatorChar);
        p2 = p2.Replace('/', Path.DirectorySeparatorChar);
        */
        // Combine them properly
        string new_path = Path.Combine(paths);
        //Debug.Log("OG: " + paths + " new: " + new_path + " system separator: " + Path.DirectorySeparatorChar);
        return new_path;
    }


    public static float Round(float value, int digits)
    {
        float mult = Mathf.Pow(10.0f, (float)digits);
        return Mathf.Round(value * mult) / mult;
    }

    public static int TextWithoutRichTextLength(string richText)
    {
        int len = 0;
        bool inTag = false;
        foreach (var ch in richText)
        {
            if (ch == '<')
            {
                inTag = true;
            }
            else if (ch == '>')
            {
                inTag = false;
            }
            if (inTag)
            {
                continue;
            }
            len++;
        }
        return len;
    }    // Ex: 0.7 = 70% chance of occurring
    public static bool RandomChance(float percent_chance_of_occuring)
    {
        return UnityEngine.Random.Range(0f, 1f) <= percent_chance_of_occuring;
    }


    public static Color KeepAlpha(Color new_color, Color old_color_alpha_to_keep)
    {
        new_color.a = old_color_alpha_to_keep.a;
        return new_color;
    }
    public static Color SetAlpha(Color c, float alpha)
    {
        c.a = alpha;
        return c;
    }
    public static Color SetThisAlpha(this Color color, float alpha)
    {
        return ExtensionMethods.SetAlpha(color, alpha);
    }


    public static IEnumerator LerpColour(SpriteRenderer sr, Color new_color, float transition_time)
    {
        Color original_colour = sr.color;
        float t = 0f;
        while (t < 1f)
        {
            t += (Time.deltaTime / transition_time);
            sr.color = Color.Lerp(original_colour, new_color, t);
            yield return null;
        }
    }

    public static Color Desaturate(Color c, bool keep_transparency = true)
    {
        return Desaturate(c, 0f, keep_transparency: keep_transparency);
    }
    // Amount: multiple by amount. 0.5 halves saturation, 0 completely desaturates
    public static Color Desaturate(Color c, float amount, bool keep_transparency = true)
    {
        Color.RGBToHSV(c, out float h, out float s, out float v);
        s = s * amount; // Desaturate
        Color return_c = Color.HSVToRGB(h, s, v);
        if (keep_transparency)
            return_c.a = c.a;
        return return_c;
    }

    public static IEnumerator FadeOutSwapSprite(SpriteRenderer sr, Sprite new_sprite, Color new_color)
    {
        Color original_colour = new_color;
        while (sr.color.a > 0)
        {
            Color c = sr.color;
            c.a -= Time.deltaTime;
            sr.color = c;
            yield return null;
        }
        if (new_sprite == null)
        {
            sr.enabled = false;
        }
        else
        {
            sr.enabled = true;
        }
        sr.sprite = new_sprite;
        while (sr.color.a < original_colour.a)
        {
            Color c = sr.color;
            c.a = Mathf.Min(c.a + Time.deltaTime, original_colour.a);   // So we don't overshoot
            sr.color = c;
            yield return null;
        }
    }
    public static IEnumerator FadeOutSprite(SpriteRenderer sr, float speed = 1f, bool destroy_after = false, float start_delay = 0f, bool deactivate_after = false)
    {
        if (start_delay > 0)
        {
            yield return new WaitForSeconds(start_delay);
        }
        if (sr == null)
            yield break;
        Color original_colour = sr.color;
        Color c_a = sr.color;
        sr.color = c_a;
        while (sr != null && sr.color.a > 0f)
        {
            Color c = sr.color;
            c.a = Mathf.Max(c.a - (Time.deltaTime * speed), 0f);   // So we don't overshoot
            sr.color = c;
            yield return null;
        }
        if (deactivate_after && sr != null)
            sr.gameObject.SetActive(false);
        if (destroy_after && sr != null)
            MonoBehaviour.Destroy(sr.gameObject);
    }
    // Speed. 1 = 1s, 2 = 0.5s, 0.5 = 2s
    public static IEnumerator FadeInSprite(SpriteRenderer sr, float speed = 1f, float initial_delay = 0f)
    {
        Color original_colour = sr.color;
        Color c_a = sr.color;
        c_a.a = 0f;
        sr.color = c_a;
        if (initial_delay > 0f)
        {
            yield return new WaitForSeconds(initial_delay);
        }
        while (sr != null && sr.color.a < 1f)
        {
            Color c = sr.color;
            c.a = Mathf.Min(c.a + (Time.deltaTime * speed), 1f);   // So we don't overshoot
            sr.color = c;
            yield return null;
        }
    }
    public static IEnumerator FadeInSprite(SpriteRenderer sr, Sprite new_sprite)
    {
        Color original_colour = sr.color;
        sr.sprite = new_sprite;
        Color c_a = sr.color;
        c_a.a = 0f;
        sr.color = c_a;
        while (sr != null && sr.color.a < 1f)
        {
            Color c = sr.color;
            c.a = Mathf.Min(c.a + Time.deltaTime, 1f);   // So we don't overshoot
            sr.color = c;
            yield return null;
        }
    }

    // 2 speed = 0.5 seconds, 0.5 speed is 2 seconds
    public static IEnumerator ChangeAlphaCanvasGroup(CanvasGroup group, float from, float to, bool reset_alpha_immediately, float speed = 1f)
    {
        if (reset_alpha_immediately)
        {
            group.alpha = from;
        }

        while (group.alpha != to)
        {
            group.alpha = Mathf.MoveTowards(group.alpha, to, Time.deltaTime * speed);
            yield return null;
        }
    }


    public static void EnableDisableGameObjectList(List<GameObject> objects, bool enable)
    {
        if (objects == null)
            return;
        foreach (GameObject obj in objects)
        {
            if (obj == null)
                continue;
            obj.SetActive(enable);
        }
    }
    public static void EnableDisableTransformList(List<Transform> objects, bool enable)
    {
        if (objects == null)
            return;
        foreach (Transform obj in objects)
        {
            if (obj == null)
                continue;
            obj.gameObject.SetActive(enable);
        }
    }
    public static void EnableDisableComponentsList(List<Component> components, bool enable)
    {
        if (components == null)
            return;
        foreach (var component in components)
        {
            EnableDisableComponent(component, enable);
        }
    }
    public static void EnableDisableComponent(Component component, bool enable)
    {
        if (component is Renderer)
        {
            (component as Renderer).enabled = enable;
        }
        else if (component is Collider)
        {
            (component as Collider).enabled = enable;
        }
        else if (component is Behaviour)
        {
            (component as Behaviour).enabled = enable;
        }
    }



    /// <summary>
    /// THIS.vector2 is Start https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/#lerp_with_easing
    /// </summary>
    /// <param name="Finish"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector2 SmoothStep(this Vector2 Start, Vector2 Finish, float t)
    {
        Vector2 new_pos = new Vector2(
            Mathf.SmoothStep(Start.x, Finish.x, t)
            , Mathf.SmoothStep(Start.y, Finish.y, t)
            );
        return new_pos;
    }

    public static void SetValueFromString(this TMP_Dropdown dropdown, string value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        var listAvailableStrings = dropdown.options.Select(option => option.text).ToList();
        int index = listAvailableStrings.IndexOf(value);
        if (index < 0)
        {
            Debug.LogError("Can't set dropdown " + dropdown + " with value " + value);
        }
        else
        {
            dropdown.value = index;
        }
    }
    public static void SetValueFromEnum(this TMP_Dropdown dropdown, Enum value)
    {
        string str = value.ToString();
        SetValueFromString(dropdown, str);
    }



    public static IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }
    public static IEnumerator SlerpToTarget(GameObject obj, Vector2 Start, Vector2 Finish, float time_to_target, bool destroy_after=false)
    {
        float t = 0f;
        float time_multipier = 1f / time_to_target;
        while (t < 1f)
        {
            obj.transform.position = Start.SmoothStep(Finish, t);//Vector2.smsmo(Start, Finish, (t));
            t += (time_multipier * Time.deltaTime);
            yield return null;
        }
        // Make sure we got there
        obj.transform.position = Finish;
        if (destroy_after)
        {
            MonoBehaviour.Destroy(obj);
        }
    }

    public const string TextFolderName = "Text";
    /// <summary>
    /// Ex: Spells.csv returns C:/Programming/GeomancersGameJam/Geomancers/Geomancers/Assets/Text/Spells.csv
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string GetTextFilepath(string filename)
    {
        return ExtensionMethods.PrepareFilePath(new string[] { Application.dataPath, TextFolderName, filename });
            //Application.dataPath + Path.DirectorySeparatorChar + "Text" + Path.DirectorySeparatorChar + filename;
    }

    /// <summary>
    /// Takes CSV textasset string, returns List<row<column>>https://stackoverflow.com/a/63202295/2471482
    /// </summary>
    /// <param name="original_csv_text"></param>
    /// <returns></returns>
    public static List<List<string>> ParseCsv(string original_csv_text)
    {
        // Remove \r - they're just too tiresome
        string full_csv_text = original_csv_text.Replace("\r", "");
        var parsedCsv = new List<List<string>>();
        var row = new List<string>();
        string field = "";
        bool inQuotedField = false;
        for (int i = 0; i < full_csv_text.Length; i++)
        {
            char current = full_csv_text[i];
            char next = i == full_csv_text.Length - 1 ? ' ' : full_csv_text[i + 1];

            // if current character is not a quote or comma or carriage return or newline (or not a quote and currently in an a quoted field), just add the character to the current field text
            if ((current != '"' && current != ',' && current != '\r' && current != '\n') || (current != '"' && inQuotedField))
            {
                field += current;
            }
            else if (current == ' ' || current == '\t')
            {
                continue; // ignore whitespace outside a quoted field
            }
            else if (current == '"')
            {
                if (inQuotedField && next == '"')
                { // quote is escaping a quote within a quoted field
                    i++; // skip escaping quote
                    field += current;
                }
                else if (inQuotedField)
                { // quote signifies the end of a quoted field
                    row.Add(field);
                    if (next == ',')
                    {
                        i++; // skip the comma separator since we've already found the end of the field
                    }
                    field = "";
                    inQuotedField = false;
                }
                else
                { // quote signifies the beginning of a quoted field
                    inQuotedField = true;
                }
            }
            else if (current == ',')
            { //
                row.Add(field);
                field = "";
            }
            else if (current == '\n')
            {
                row.Add(field);
                parsedCsv.Add(new List<string>(row));
                field = "";
                row.Clear();
            }
        }
        return parsedCsv;
    }

    public static string Multiply(this string source, int multiplier)
    {
        StringBuilder sb = new StringBuilder(multiplier * source.Length);
        for (int i = 0; i < multiplier; i++)
        {
            sb.Append(source);
        }
        return sb.ToString();
    }

    // C:\Programming\RadioGeneral2Git\RadioGeneral2Unity // Just before /Assets/
    public static string GetEditorPath()
    {
        //string path = Application.dataPath; // C:/Programming/RadioGeneral2Git/RadioGeneral2Unity/Assets
        //path = path.Replace("/Assets/", "/Builds/");
        string path = Directory.GetCurrentDirectory();
        Debug.Log("Current directory: " + path);
        return path;
    }
    // Ex: "Uploads" "Builds"
    public static string GetEditorFolderPath(string folder_name_no_slashes)
    {
        string path = Directory.GetCurrentDirectory();
        folder_name_no_slashes = "\\" + folder_name_no_slashes + "\\";
        string full_path = path + folder_name_no_slashes;
        return full_path;
    }

#if UNITY_EDITOR
    public static void CopyFolderTo(string full_path_from, string full_path_to)
    {
        UnityEngine.Debug.Log("Copying from " + full_path_from + " to " + full_path_to);
        if (!Directory.Exists(full_path_to))
        {
            Directory.CreateDirectory(full_path_to);
            UnityEngine.Debug.Log("Created folder " + full_path_to);
        }
        FileUtil.ReplaceDirectory(full_path_from, full_path_to);
    }
#endif
}
public static class StringExtension
{
    public static string CapitalizeFirst(this string s)
    {
        bool IsNewSentence = true;
        var result = new StringBuilder(s.Length);
        int bracket_level = 0;
        for (int i = 0; i < s.Length; i++)
        {
            if (s[i] == '<')
                bracket_level++;
            if (s[i] == '>')
                bracket_level--;
            if (IsNewSentence && bracket_level == 0 && s[i] != ' ')//char.IsLetter(s[i]) && bracket_level == 0)
            {
                // This is a new sentence, and we found a letter, don't bother capitalizing it if the previous character is a bracket <
                result.Append (char.ToUpper(s[i]));
                IsNewSentence = false;
            }
            else
                result.Append (s[i]);

            if (s[i] == '!' || s[i] == '?' || s[i] == '.')
            {
                IsNewSentence = true;
            }
        }

        return result.ToString();
    }
}