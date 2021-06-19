# Project_USoMK

<p align="center">
<img src="Assets/z_GitHubImages/project_usomk_ss_git.png" width="720">
</p>

Project_USoMK, izometrik kamera açısına sahip Action-Adventure tarzda bir oyun ortaya çıkarmak için atılan ilk adımdır ve teknoloji demosu olarak geliştirilmeye devam etmektedir. 
Bu projenin başlangıcında temel olarak, yeni oyun mekanikleri oluşturmak, platform ve puzzle tasarlamak, amaçlanmıştır. 
Sonraki aşamalarda, dövüş/savaş sistemi, düşmanlar, envanter sistemi, diyalog sistemi, yeni hareket mekanikleri geliştirilmesi planlanmaktadır. 
Bu gelişmeler göz önünde bulundurularak oyun türü ilerleyen zamanlarda değişebilir.

## Controls
  
<p align="center">
<img src="Assets/z_GitHubImages/controls_git.png" width="720">
</p>


## Map

Level -2 : Pressure Plates | Level -1 : Buttons
:-------------------------:|:-------------------------:
| ![](/Assets/z_GitHubImages/Map_SS2_part1_git.png)|  ![](/Assets/z_GitHubImages/Map_SS2_part2_git.png) 
  Level 0 : Objects and Prefabs            | Level 1-2-3-4 : Basic Examples
|  ![](/Assets/z_GitHubImages/Map_SS2_part3_git.png) | ![](/Assets/z_GitHubImages/Map_SS2_part4_git.png)
Level -2 : Pressure Plates 
| ![](/Assets/z_GitHubImages/Map_SS2_part1_git.png)|


[![Project_USoMK Playlist](/Assets/z_GitHubImages/USOMK_youtube.png)](https://www.youtube.com/watch?v=auG90HygEpo&list=PLrzMY5OA4-O7x6NI5SJ2GlP84X5tN_9cX&index=1)




### 16.06.2021 Update:
* Observer: Objeleri el ile birbirine bağlama yerine Observer patterne geçildi. Custom Game Events kullanıldı. Kontrolcü objeler artık "string targetID" ile hedef objeleri tetikleyebiliyor.
* Object Pooling: Spawner ve Duplicatorların sürekli instantiate ve destroy yapması bellek problemlerine sebep oluyordu. Object pooling ile bu durumun önüne geçilmesi planlandı.
* Base classes such as Object Base, Controller Base were coded and integrated to objects used.
* Color change of materials is optimized using material property block.
* Mesh was arranged to prevent unnecessary copying of some mesh. Mesh is optimized.
* Adjusted the quality and size of some textures to reduce memory usage of textures.

#### Profiler Sonuçları
* As a result of the build, the demo used memory between 185-210 MB. In the memory profiler, a large part of this ram usage is due to the settings of the scene camera such as anti-aliasing and bloom under the name of RenderTexture.

<p align="center">
<img src="/profilerSonuclar/MemoryProfiler.jpg" width="720">
<img src="/profilerSonuclar/profiler1.jpg" width="720">
<img src="/profilerSonuclar/profiler2profilers%20sze.jpg" width="720">
  
* The amount of memory the profiler uses.
</p>
  
