using Cubase.Hub.Services.Album;
using Cubase.Hub.Services.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace Cubase.Hub.Controls.CompletedMixes
{
    public class CompletedMixesMenu : TreeView
    {
        private readonly IServiceProvider serviceProvider;

        private IAlbumService albumService;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<AlbumLocation> OnAlbumSelected { get; set; }

        public CompletedMixesMenu(IServiceProvider serviceProvider) : base()
        {
            this.serviceProvider = serviceProvider;
            this.albumService = this.serviceProvider.GetService<IAlbumService>();
        }

        public void LoadAlbums()
        {
            var albums = this.albumService.GetAlbumList((err) => { });
            this.Nodes.Clear();
            this.Nodes.Add(new AlbumRootNode(albums));
            this.ExpandAll();
        }

        protected override void OnAfterSelect(TreeViewEventArgs e)
        {
            base.OnAfterSelect(e);
            if (e.Node is AlbumNode album)
            {
                {
                    this.OnAlbumSelected?.Invoke(album.Album);
                }
            }
        }

        public class AlbumRootNode : TreeNode
        {
            public AlbumRootNode(List<AlbumLocation> albums)
            {
                this.Text = "Albums";
                foreach (var album in albums)
                {
                    this.Nodes.Add(new AlbumNode(album));
                }
            }
        }


        public class AlbumNode : TreeNode
        {
            public AlbumLocation Album { get; set; }

            public AlbumNode(AlbumLocation album)
            {
                this.Album = album;
                this.Text = album.AlbumName;
            }
        }
    }
}
