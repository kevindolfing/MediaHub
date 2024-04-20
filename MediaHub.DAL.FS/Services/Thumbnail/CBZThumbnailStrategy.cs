﻿using System.IO.Abstractions;
using System.IO.Compression;
using System.Net.Mime;
using System.Xml;
using MediaHub.DAL.FS.Services.MediaPath;
using Xabe.FFmpeg;

namespace MediaHub.DAL.FS.Services.Thumbnail;

public class CBZThumbnailStrategy : ThumbnailStrategy
{
    public CBZThumbnailStrategy(RootPathService rootPath, ThumbnailPathService thumbnailPath, IFileSystem fileSystem) :
        base(rootPath, thumbnailPath, fileSystem)
    {
    }

    public override IEnumerable<string> SupportedExtensions { get; } = new[] { "cbz" };

    public override async Task ExtractThumbnail(string path)
    {
        // Construct the full path of the media file
        string mediaFilePath = RootPath.CombineRootPath(path);

        // Construct the output thumbnail file path
        string thumbnailFilePath = ThumbnailPath.CombineRootPath(path + ".webp");

        using ZipArchive zip = ZipFile.OpenRead(mediaFilePath);

        //find first page
        ZipArchiveEntry firstEntry = zip.Entries.OrderBy(e => e.FullName).First();
        
        await using Stream imageStream = firstEntry.Open();
        string tempImagePath = Path.GetTempFileName();

        try
        {
            await using (FileSystemStream fileStream = FileSystem.File.Create(tempImagePath))
            {
                imageStream.CopyTo(fileStream);
            }

            IConversion conversion =
                await FFmpeg.Conversions.FromSnippet.Snapshot(tempImagePath, thumbnailFilePath,
                    TimeSpan.FromSeconds(0));
            //set image resolution to 480p
            conversion.AddParameter("-vf scale=480:-1");
            await conversion.Start();
        }
        finally
        {
            FileSystem.File.Delete(tempImagePath);
        }
    }
}