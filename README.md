# MediaHub
Media manager system

## Requirements

* File system
  * [ ] The application has exactly one media root. 
  * [ ] Any folder inside the media root is recognized as a media folder.
    * [ ] A media folder can contain media folders
  * [ ] Media files inside a folder are recognized.
  * [ ] A media folder can get optional metadata (Display name, description, image)
* Database
  * [ ] For each user: Keep track of read/watch progress.
  * [ ] Maybe permission system per media folder?
* API
  * [ ] Auth with oAuth.
  * [ ] Runs as docker container
  * [ ] Endpoint for media folders
* Frontend
  * Web
    * [ ] List all media folders
      * [ ] List all media files inside folder
      * [ ] Play media file (mp4, wav, video)
      * [ ] Read media file
      * [ ] Download media file
  * Mobile App
    * [ ] List all media folders
    * [ ] Sync folders (perhaps with watch progress?)

MoSCoW
Must have
Should have
Could have
Won't have