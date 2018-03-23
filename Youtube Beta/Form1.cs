using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Threading;
using CefSharp;
using CefSharp.WinForms;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace Youtube_Beta
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();            
        }

        ChromiumWebBrowser chrome;

        public async void button2_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.AppStarting;
            flowLayoutPanel1.Controls.Clear();
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyB82SdueHZ - KNAEnjkfhcZDCMwkm_ahg2o",
                ApplicationName = this.GetType().ToString()
            });
            var searchListRequest = youtubeService.Search.List("snippet");
            searchListRequest.Q = textBox1.Text;
            searchListRequest.Type = "video";
            searchListRequest.MaxResults = 15;
            var searchListResponse = await searchListRequest.ExecuteAsync();
            foreach (var searchResult in searchListResponse.Items)
            {
                PictureBox thumb = new PictureBox();
                thumb.Size = new Size(120, 90);
                thumb.ImageLocation = searchResult.Snippet.Thumbnails.Default__.Url;
                thumb.Cursor = Cursors.Hand;
                TextBox videoNome = new TextBox();
                videoNome.Size = new Size(120, 45);
                videoNome.BorderStyle = BorderStyle.None;
                videoNome.BackColor = SystemColors.Control;
                videoNome.TextAlign = HorizontalAlignment.Center;
                videoNome.Multiline = true;
                videoNome.Cursor = Cursors.Hand;
                videoNome.Text = searchResult.Snippet.Title;
                flowLayoutPanel1.Controls.Add(thumb);
                flowLayoutPanel1.Controls.Add(videoNome);
                thumb.Click += delegate
                {
                    string page = $"<html><body bgcolor='#f2f2f2'><iframe width='560' height='315' src='https://www.youtube.com/embed/{searchResult.Id.VideoId}?autoplay=1' frameborder='0' allow='autoplay; encrypted-media' allowfullscreen></iframe></body></html>";                    
                    chrome.LoadHtml(page);
                };
                }
            this.Cursor = Cursors.Default;
            }

        private void Form1_Load(object sender, EventArgs e)
        {
            CefSettings settings = new CefSettings();
            Cef.Initialize(settings);
            chrome = new ChromiumWebBrowser("");
            this.panel1.Controls.Add(chrome);
            chrome.Dock = DockStyle.Fill;
            chrome.LoadHtml("<html><body bgcolor='#f2f2f2'></body></html>");       
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cef.Shutdown();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Clear();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                button2_Click(sender, e);
            }
        }
    }
}