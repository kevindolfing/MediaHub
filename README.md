# MediaHub
Media manager system

## Requirements

* File system
  * [x] The application has exactly one media root. 
  * [x] Any folder inside the media root is recognized as a media folder.
    * [x] A media folder can contain media folders
  * [x] Media files inside a folder are recognized.
  * [ ] A media folder can get optional metadata (Display name, description, image)
* Database
  * [ ] For each user: Keep track of read/watch progress.
  * [ ] Maybe permission system per media folder?
* API
  * [x] Auth with oAuth.
  * [x] Runs as docker container
  * [x] Endpoint for media folders
* Frontend
  * Web
    * [ ] List all media folders
      * [x] List all media files inside folder
      * [ ] Play media file (mp4, wav, video)
      * [ ] Read media file
      * [x] Download media file
  * Mobile App
    * [ ] List all media folders
    * [ ] Sync folders (perhaps with watch progress?)

MoSCoW
Must have
Should have
Could have
Won't have