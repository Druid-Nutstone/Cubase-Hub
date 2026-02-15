using System;
using System.Collections.Generic;
using System.Text;

namespace Cubase.Hub.Controls.Album.Manage
{
    public enum AlbumCommandType
    {
        RefreshTracks,
    }

    public sealed class AlbumCommands
    {
        private static AlbumCommands _instance;

        private List<Action<AlbumCommandType>> albumCommandCallbacks = new List<Action<AlbumCommandType>>();

        public static AlbumCommands Instance 
        { 
            get 
            { 
              if (_instance == null)
                {
                    _instance = new AlbumCommands();
                }
                return _instance;
            } 
        }

        public void RegisterForAlbumCommand(Action<AlbumCommandType> onAlbumCommand)
        {
            if (!this.albumCommandCallbacks.Contains(onAlbumCommand))
            {
                this.albumCommandCallbacks.Add(onAlbumCommand); 
            }
        }

        public void RefreshTracks()
        {
            foreach (var callback in this.albumCommandCallbacks)
            {
                callback(AlbumCommandType.RefreshTracks);
            }
        }   
    }
}
