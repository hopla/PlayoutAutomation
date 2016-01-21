﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TAS.Common;
using TAS.Remoting.Client;
using TAS.Server.Common;
using TAS.Server.Interfaces;

namespace TAS.Client.Model
{
    public class MediaManager : ProxyBase, IMediaManager
    {
        public void ArchiveMedia(IEnumerable<IMedia> mediaList, bool deleteAfter)
        {
            Invoke(parameters: new object[] { mediaList, deleteAfter });
        }

        public IEnumerable<MediaDeleteDenyReason> DeleteMedia(IEnumerable<IMedia> mediaList)
        {
            return Query<List<MediaDeleteDenyReason>>(parameters: mediaList);
        }

        public void Export(IEnumerable<MediaExport> exportList, IIngestDirectory directory)
        {
            Invoke(parameters: new object[] { exportList, directory });
        }

        public IAnimationDirectory AnimationDirectoryPGM { get; set; }

        public IAnimationDirectory AnimationDirectoryPRV { get; set; }

        public IArchiveDirectory ArchiveDirectory { get { return Get<ArchiveDirectory>(); } set { Set(value); } }

        public IEngine getEngine()
        {
            throw new NotImplementedException();
        }

        public IFileManager FileManager { get { return Get<IFileManager>(); }  set { Set(value); } }

        public VideoFormatDescription FormatDescription { get { return Get<VideoFormatDescription>(); } set { Set(value); } }

        public List<IIngestDirectory> IngestDirectories
        {
            get { return Get<List<IIngestDirectory>>(); }
            set { Set(value); }
        }

        public void GetLoudness(IEnumerable<IMedia> mediaList)
        {
            Invoke(parameters: mediaList);
        }

        public IServerDirectory MediaDirectoryPGM { get { return Get<ServerDirectory>(); } set { Set(value); } }

        public IServerDirectory MediaDirectoryPRV { get { return Get<ServerDirectory>(); } set { Set(value); } }

        public IMedia GetPRVMedia(IMedia media)
        {
            return Query<Media>(parameters: media);
        }

        public TVideoFormat VideoFormat { get { return Get<TVideoFormat>(); } set { Set(value); } }

        public void CopyMediaToPlayout(IEnumerable<IMedia> mediaList, bool toTop) { Invoke(parameters: new object[] { mediaList, toTop }); }
        
        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ReloadIngestDirs()
        {
            Invoke();
        }

        public void SynchronizePrvToPgm(bool deleteNotExisted)
        {
            Invoke(parameters: deleteNotExisted);
        }
    }
}
