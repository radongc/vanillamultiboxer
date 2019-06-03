﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multiboxer
{
    class ConfigurationManager
    {
        /* ConfigurationManager
         * Handles loading, saving, and config condition checking. */
        
        public string ConfigFilePath { get; }
        public ToolStripStatusLabel MainStatusLabel { get; private set; }

        public ConfigurationManager(string cfgFilePath, ToolStripStatusLabel mainStatusLabel)
        {
            MainStatusLabel = mainStatusLabel;
            ConfigFilePath = cfgFilePath;
        }

        public bool IsFirstRun()
        {
            if (File.Exists(ConfigFilePath))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void UpdateStatus(string text, Color color)
        {
            MainStatusLabel.ForeColor = color;
            MainStatusLabel.Text = text;
        }

        // Save config file

        public void SaveToConfig(string[] contents)
        {
            string[] cfgContent = new string[contents.Length];

            int i = 0;

            foreach (string line in contents)
            {
                cfgContent[i] = line;
                i++;
            }

            WriteToConfig(cfgContent);
        }

        private void WriteToConfig(string[] contents)
        {
            string path = ConfigFilePath;

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string line in contents)
                {
                    sw.WriteLine(line);
                }
            }
        }

        // Read config file

        public void LoadFromConfig(RichTextBox rtb)
        {
            string path = ConfigFilePath;

            string[] cfgLines = File.ReadAllLines(path);

            int i = 0;

            foreach (string line in cfgLines)
            {
                if (i == (cfgLines.Length - 1))
                {
                    rtb.AppendText(cfgLines[i]);
                    i++;
                }
                else
                {
                    rtb.AppendText(cfgLines[i] + "\n");
                    i++;
                }
            }
        }
    }
}
