using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMM_Core
{
    /// <summary>
    /// d3dx.ini 配置文件读写类。
    /// 此版本已修改为手动保存模式。
    /// 更改会暂存在内存中，直到调用 Save() 方法才会写入文件。
    /// </summary>
    public class D3dxIniConfig
    {
        private readonly string d3dxini_path;
        private readonly List<string> lines;
        private bool fileExists;

        /// <summary>
        /// 构造函数，加载指定的ini文件到内存中。
        /// </summary>
        /// <param name="path">d3dx.ini文件的完整路径</param>
        public D3dxIniConfig(string path)
        {
            d3dxini_path = path;
            lines = new List<string>();
            fileExists = File.Exists(d3dxini_path);

            if (fileExists)
            {
                // 一次性读取所有行到内存列表
                lines.AddRange(File.ReadAllLines(d3dxini_path));
            }
        }

        /// <summary>
        /// 从内存中读取属性值。
        /// </summary>
        /// <param name="attributeName">属性名</param>
        /// <returns>属性值，如果不存在则返回空字符串</returns>
        public string ReadAttribute(string attributeName)
        {
            // 直接从内存列表中查找，不再读取文件
            foreach (string line in lines)
            {
                // 使用 TrimStart 防止属性名前的空格影响判断，并忽略大小写
                if (line.TrimStart().StartsWith(attributeName, StringComparison.OrdinalIgnoreCase) && line.Contains('='))
                {
                    // 只在第一个 '=' 处分割，以防值本身也包含'='
                    string[] splits = line.Split(new[] { '=' }, 2);
                    string arg_name = splits[0].Trim();

                    // 精确匹配属性名（忽略大小写）
                    if (arg_name.Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                    {
                        // 返回'='后的部分，并去除首尾空格
                        return splits.Length > 1 ? splits[1].Trim() : "";
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// 在内存中设置或更新一个属性值。此操作不会立即保存到文件。
        /// </summary>
        /// <param name="sectionName">节名，如 [Key] 或 [Shader]。如果属性不存在，会添加到此节下。</param>
        /// <param name="attributeName">属性名</param>
        /// <param name="attributeValue">要设置的属性值</param>
        public void SetAttribute(string sectionName, string attributeName, string attributeValue)
        {
            int attributeLineIndex = -1;
            // 1. 在内存列表中查找该属性是否已存在
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].TrimStart().StartsWith(attributeName, StringComparison.OrdinalIgnoreCase) && lines[i].Contains('='))
                {
                    string[] splits = lines[i].Split(new[] { '=' }, 2);
                    if (splits[0].Trim().Equals(attributeName, StringComparison.OrdinalIgnoreCase))
                    {
                        attributeLineIndex = i;
                        break;
                    }
                }
            }

            // 准备新行内容
            string newLine = attributeName + " = " + attributeValue;
            // 如果值为空或只有空格，则将该行注释掉
            if (string.IsNullOrWhiteSpace(attributeValue))
            {
                newLine = "; " + newLine;
            }

            // 2. 根据查找结果，更新或插入新行
            if (attributeLineIndex != -1)
            {
                // 如果找到了属性，则直接在内存列表中替换该行
                lines[attributeLineIndex] = newLine;
            }
            else
            {
                // 如果未找到属性，则在指定的Section下添加
                int sectionLineIndex = -1;
                // 查找节名所在行
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Trim().Equals(sectionName, StringComparison.OrdinalIgnoreCase))
                    {
                        sectionLineIndex = i;
                        break;
                    }
                }

                if (sectionLineIndex != -1)
                {
                    // 在节名的下一行插入新属性
                    lines.Insert(sectionLineIndex + 1, newLine);
                }
                else
                {
                    // 如果连Section都找不到，则在文件末尾添加新的Section和属性
                    if (lines.Count > 0)
                    {
                        lines.Add(""); // 添加一个空行以作分隔
                    }
                    lines.Add(sectionName);
                    lines.Add(newLine);
                }
            }
        }

        /// <summary>
        /// 【核心方法】将所有在内存中的更改保存到 d3dx.ini 文件。
        /// </summary>
        public void Save()
        {
            // 确保目录存在，如果不存在则创建
            string directory = Path.GetDirectoryName(d3dxini_path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 将内存中的所有行一次性写入文件，覆盖原有内容
            File.WriteAllLines(d3dxini_path, lines);
            // 更新文件存在状态，以防是首次创建文件
            fileExists = true;
        }
    }
}